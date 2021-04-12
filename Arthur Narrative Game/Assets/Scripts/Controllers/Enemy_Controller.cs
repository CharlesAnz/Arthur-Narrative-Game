using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Controller : MonoBehaviour
{
    public float lookRadius = 10f;

    Enemy enemyInteractor;

    Transform target;
    NavMeshAgent agent;
    CharacterCombat combat;
    Character_Stats stats;
    PlayerManager playerManager;


    bool retreating;
    Vector3 currentDestination;
    public List<Transform> retreatLocations = new List<Transform>();
    int locationsTraveled = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = PlayerManager.instance;

        agent = GetComponent<NavMeshAgent>();
        combat = GetComponent<CharacterCombat>();
        stats = GetComponent<Character_Stats>();
        enemyInteractor = GetComponent<Enemy>();

        agent.stoppingDistance = enemyInteractor.radius;

        agent.updateRotation = false;

        agent.speed = stats.moveSpeed.GetValue();

        foreach (Ability ability in stats.GetComponent<Character_Stats>().abilities)
        {
            ability.cooldownTimer = 0;

            if (ability.GetType().Equals(typeof(Aoe_Ability)))
            {
                Aoe_Ability abilityCopy = (Aoe_Ability)ability;
                abilityCopy.SetOrigin(transform.position + (transform.forward));
            }
        }

        //target = playerManager.activePerson.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (combat.CastTime > 0)
        {
            target = null;
            agent.velocity = Vector3.zero;
            
            return;
        }

        target = playerManager.activePerson.transform;


        if (retreating)
        {
            Retreating();
            return;
        }

        FaceTarget(target.position);
        Attacking();
    }


    void Attacking()
    {
        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= lookRadius)
        {
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance)
            {
                //attack
                Character_Stats targetStats = target.GetComponent<Character_Stats>();
                if (targetStats != null && combat.CastTime < 0)
                {
                    List<Ability> myAbilities = stats.abilities;

                    if (myAbilities.Count != 0)
                    {
                        if (stats.curHP <= stats.maxHP.GetValue() * 0.75)
                        {
                            if (myAbilities[1].cooldownTimer <= 0)
                            {
                                retreating = true;
                                locationsTraveled++;
                                
                                if (locationsTraveled >= 4) locationsTraveled = 1;

                                stats.armor.AddModifier(3);
                                stats.damage.AddModifier(2);

                                if (myAbilities[1].GetType().Equals(typeof(Aoe_Ability)))
                                {
                                    Aoe_Ability ability = (Aoe_Ability)myAbilities[1];
                                    ability.SetOrigin(transform.position + (transform.forward * 2));
                                }

                                myAbilities[1].Use(gameObject);

                                combat.CastTime += 2f;
                            }
                        }

                        if (myAbilities[0].cooldownTimer <= 0)
                        {
                            myAbilities[0].Use(gameObject);
                        }


                    }
                    combat.Attack(targetStats);
                }
            }
            else
                GetComponent<CharacterAnimator>().characterAnim.SetBool("basicAttack", false);


        }

    }

    void Retreating()
    {
        //target.GetComponent<CharacterAnimator>().characterAnim.SetBool("attacking", false);

        agent.speed = 6;
        enemyInteractor.radius = 0.1f;
        GetComponent<CharacterAnimator>().characterAnim.SetBool("basicAttack", false);

        if (currentDestination != Vector3.zero)
        {
            agent.SetDestination(currentDestination);
            FaceTarget(currentDestination);

            float distance = Vector3.Distance(currentDestination, transform.position);

            if (distance <= 5)
            {
                currentDestination = Vector3.zero;
                agent.speed = 2;
                GetComponent<CharacterAnimator>().characterAnim.SetTrigger("land");
                enemyInteractor.radius = 4f;
                retreating = false;
                
            }

            return;
        }

        if (stats.abilities[2].cooldownTimer <= 0)
        {
            if (stats.abilities[2].GetType().Equals(typeof(Targeted_Ability)))
            {
                Targeted_Ability ability = (Targeted_Ability)stats.abilities[2];
                ability.FindTarget(target.GetComponent<CharacterCombat>());
            }
            FaceTarget(target.position);
            stats.abilities[2].Use(gameObject);
        }


        currentDestination = retreatLocations[locationsTraveled - 1].position;

        agent.SetDestination(currentDestination);

    }


    //make sure to face target when attacking
    void FaceTarget(Vector3 position)
    {
        Vector3 direction = (position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);


        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(
             transform.position + (transform.forward * 2),
            new Vector3(4, 4, 4));

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 10;
        Gizmos.DrawRay(transform.position, direction);
    }
}
