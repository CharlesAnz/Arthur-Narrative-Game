using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Ability : ScriptableObject
{
    new public string name = "New Ability";

    public string description = "ability does thing";
    protected GameObject abilityUser = null;
    public Sprite icon = null;

    public List<BufforDebuff> buffList = new List<BufforDebuff>();

    public List<CC_Effect> cc_Effects = new List<CC_Effect>();

    [SerializeField]
    protected GameObject projectile;

    [SerializeField]
    protected float delay;

    [SerializeField]
    protected float maxDistance;

    [SerializeField]
    protected CC_Displacement displacement;

    protected Vector3 displacePos;

    protected Vector3 projectileSpawnPos;

    [SerializeField]
    protected bool doesDamage, doesHealing;

    [SerializeField]
    protected float abilityValue;

    [SerializeField]
    protected float cooldown;

    [SerializeField]
    protected string animatorTrigger;

    public float castTime;

    //[HideInInspector]
    public float cooldownTimer = 0;

    [SerializeField]
    protected TargetType targetType;

    protected Camera cam;

    protected UnityEvent<CharacterCombat> OnAbilityUse;

    public virtual void Use(GameObject interactor)
    {
        abilityUser = interactor;

        displacePos = interactor.transform.position;
    }

    //Checks if the cooldown is below, if not then nothing happens
    protected bool Setup(GameObject interactor)
    {
        CharacterCombat combat = interactor.GetComponent<CharacterCombat>();
        CharacterAnimator anim = abilityUser.GetComponent<CharacterAnimator>();

        if (cooldownTimer >= 0)
        {
            Debug.Log("Ability on cooldown");
            return false;
        }
        if (combat.CastTime >= 0)
        {
            Debug.Log("Casting another Ability");
            return false;
        }
        if (combat.silenced)
        {
            Debug.Log(interactor.name + " is using is silenced");
            return false;
        }

        if (anim != null)
        {
            anim.characterAnim.SetTrigger(animatorTrigger);
        }

        cooldownTimer = cooldown;

        combat.CastTime = castTime;
        combat.SetAttackCooldown(castTime);

        if (doesDamage) OnAbilityUse.AddListener(Damage);
        if (doesHealing) OnAbilityUse.AddListener(Heal);
        if (buffList.Count > 0) OnAbilityUse.AddListener(addBuff);
        if (cc_Effects.Count > 0) OnAbilityUse.AddListener(addCC_Effect);
        if (displacement.distance != 0) OnAbilityUse.AddListener(addDisplacement);


        return true;
    }

    protected virtual Vector3 FindTargetWithMouse(float maxCastDistance)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxCastDistance))
        {
            return hit.point;
        }

        else return Vector3.zero;
    }

    private void Damage(CharacterCombat target)
    {
        abilityUser.GetComponent<CharacterCombat>().AbilityHit(target.GetMyStats(), abilityValue);
    }

    private void Heal(CharacterCombat target)
    {
        abilityUser.GetComponent<CharacterCombat>().AbilityHeal(target.GetMyStats(), abilityValue);
    }

    private void addBuff(CharacterCombat target)
    {
        Character_Stats statsAffected = target.GetMyStats();

        foreach (BufforDebuff buff in buffList)
        {
            buff.durationTimer = buff.duration;
            switch (buff.affects)
            {
                case StatBuffs.Damage:
                    statsAffected.damage.AddModifier(buff.amount);
                    break;
                case StatBuffs.Armor:
                    statsAffected.armor.AddModifier(buff.amount);
                    break;
                case StatBuffs.MoveSpeed:
                    statsAffected.moveSpeed.AddModifier(buff.amount);
                    break;
                case StatBuffs.AttackSpeed:
                    statsAffected.attackSpeed.AddModifier(buff.amount);
                    break;
                case StatBuffs.Health:
                    if (buff.amount < 0)
                        statsAffected.TakePureDam(buff.amount);
                    else
                        statsAffected.Heal(buff.amount);
                    break;
                default:
                    break;
            }

            statsAffected.buffs.Add(buff);
        }
    }

    private void addDisplacement(CharacterCombat target)
    {

        Rigidbody rb = target.GetComponent<Rigidbody>();

        Debug.Log(target.gameObject.name + " is being displaced");

        if (rb != null)
        {
            Vector3 direction = Vector3.zero;

            if (displacement.pushOrPull == DisplacementEffect.Push)
                direction = target.transform.position - displacePos;

            else if (displacement.pushOrPull == DisplacementEffect.Pull)
                direction = displacePos - target.transform.position;

            direction.y = 0;

            //rb.AddForce(direction.normalized * displacement.distance, ForceMode.VelocityChange);

            rb.velocity = (direction.normalized * displacement.distance);// * (Time.deltaTime + 2));

            Debug.Log("Displacement velocity is:" + direction.normalized * displacement.distance);

            target.CastTime = 2f;
            target.SetAttackCooldown(2f);
            target.GetComponent<CharacterAnimator>().characterAnim.SetBool("basicAttack", false);
            target.GetComponent<NavMeshAgent>().destination = target.transform.position + rb.velocity;
            target.GetComponent<Player_Controller>().RemoveFocus();
        }
    }

    private void addCC_Effect(CharacterCombat target)
    {
        foreach (CC_Effect effect in cc_Effects)
        {
            effect.durationTimer = effect.duration;
            target.cc_Effects.Add(effect);
        }
    }

    protected void SpawnProjectile(Vector3 hitPositiion)
    {
        if(projectileSpawnPos == Vector3.zero) projectileSpawnPos = new Vector3(abilityUser.transform.localPosition.x, abilityUser.transform.localPosition.y + 1, abilityUser.transform.localPosition.z + 1);

        GameObject spawnedProjectile = Instantiate(projectile, projectileSpawnPos, Quaternion.identity);

        Debug.Log("Projectile Spawned ");

        displacePos = spawnedProjectile.transform.position;

        AbilityProjectile projectileScript = spawnedProjectile.GetComponent<AbilityProjectile>();

        projectileScript.OnHit = OnAbilityUse;
        projectileScript.targetType = targetType;
        projectileScript.user = abilityUser;

        Vector3 direction = (hitPositiion - abilityUser.transform.position).normalized;

        spawnedProjectile.GetComponent<Rigidbody>().velocity = direction * (Time.deltaTime + 10);
    }

    public void SetProjectileSpawnPos(Vector3 pos)
    {
        projectileSpawnPos = pos;
    }


    public void SetCam(Camera newCam) { cam = newCam; }

    public IEnumerator UseAbility(CharacterCombat target)
    {
        yield return new WaitForSeconds(delay);

        OnAbilityUse.Invoke(target);
    }

    public IEnumerator ProjectileSpawn(Vector3 pos)
    {
        yield return new WaitForSeconds(delay);

        SpawnProjectile(pos);
    }

}

public enum TargetType { Self, Ally, Enemy, Any, AnyExcludingSelf }






