using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_Controller : MonoBehaviour
{
    public float lookRadius = 10f;
    const float locoAnimSmoothTime = 0.1f;

    Enemy enemyInteractor;

    Transform target;
    NavMeshAgent agent;
    CharacterCombat combat;
    Character_Stats stats;
    PlayerManager playerManager;


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
                abilityCopy.origin = transform.forward * 1.2f;
            }
        }

        target = playerManager.activePerson.transform;
    }

    // Update is called once per frame
    void Update()
    {

        if (GetComponent<CharacterCombat>().CastTime > 0)
        {
            target = null;
            agent.velocity = Vector3.zero;
            return;
        }

        target = playerManager.activePerson.transform;

        float distance = Vector3.Distance(target.position, transform.position);
        if(distance <= lookRadius)
        {
            agent.SetDestination(target.position);

            if (distance <= agent.stoppingDistance)
            {
                //attack
                Character_Stats targetStats = target.GetComponent<Character_Stats>();
                if (targetStats != null && combat.CastTime < 0)
                {
                    List<Ability> myAbilities = stats.abilities;

                    if (myAbilities[0].cooldownTimer <= 0 && myAbilities.Count != 0)
                    {
                        //myAbilities[0].Use(gameObject);
                    }

                    if (myAbilities[1].cooldownTimer <= 0 && myAbilities.Count != 0)
                    {
                        if (myAbilities[1].GetType().Equals(typeof(Aoe_Ability)))
                        { 
                            Aoe_Ability ability = (Aoe_Ability)myAbilities[1];
                            ability.origin = transform.position + (transform.forward * 3);//transform.TransformDirection(Vector3.forward) * 1.2f;
                        }

                        myAbilities[1].Use(gameObject);
                    }
                    else
                    {
                        combat.Attack(targetStats);
                    }

                }
            }
            else
                GetComponent<CharacterAnimator>().characterAnim.SetBool("basicAttack", false);

            FaceTarget();
        }
        

    }

    //make sure to face target when attacking
    void FaceTarget()
    {
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);

        /*
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(
             transform.position + (transform.forward * 3), 
            new Vector3(4, 4, 4));
        */
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Vector3 direction = transform.TransformDirection(Vector3.forward) * 10;
        Gizmos.DrawRay(transform.position, direction);

        
    }
}
