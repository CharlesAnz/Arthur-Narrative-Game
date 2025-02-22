﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class Ability : ScriptableObject
{
    public string description = "ability does thing";
    protected GameObject abilityUser = null;
    public Sprite icon = null;

    public bool listenersAdded;

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

    [SerializeField]
    protected bool doesDamage, doesHealing;

    [SerializeField]
    protected float abilityValue;

    [SerializeField]
    private float cooldown;

    [SerializeField]
    protected TargetType targetType;

    protected Vector3 displacePos;

    protected Vector3 projectileSpawnPos;

    /*
    public float delay;
    public GameObject projectile;
    public float maxDistance;
    public CC_Displacement displacement;
    public bool doesDamage, doesHealing;
    public float abilityValue;
    public float cooldown;
    public TargetType targetType;
    */

    public string animatorTrigger;


    public float castTime;

    //[HideInInspector]
    public float cooldownTimer = 0;


    protected Camera cam;

    protected UnityEvent<CharacterCombat> OnAbilityUse = new UnityEvent<CharacterCombat>();

    public virtual void Use(GameObject interactor)
    {
        abilityUser = interactor;

        displacePos = interactor.transform.position;
    }

    private void SetupListeners()
    {
        if (listenersAdded == true) return;

        if (doesDamage) OnAbilityUse.AddListener(Damage);
        if (doesHealing) OnAbilityUse.AddListener(Heal);
        if (buffList != null)
        {
            if(buffList.Count > 0)
                OnAbilityUse.AddListener(addBuff);
        }
        if (cc_Effects != null)
        {
            if (cc_Effects.Count > 0)
                OnAbilityUse.AddListener(addCC_Effect); 
        }
        if (displacement.distance != 0) OnAbilityUse.AddListener(addDisplacement);

        listenersAdded = true;
        Debug.Log("Setup completed");
    }

    //Checks if the cooldown is below, if not then nothing happens
    protected bool Conditions(GameObject interactor)
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
        combat.SetAttackCooldown(castTime); //+ (1f / combat.attackSpeed));


        SetupListeners();
        Debug.Log("Setup completed");
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
                    if (target.damageBuffIndicator != null) target.damageBuffIndicator.SetActive(true);
                    break;
                case StatBuffs.Armor:
                    statsAffected.armor.AddModifier(buff.amount);
                    if (target.armorBuffIndicator != null) target.armorBuffIndicator.SetActive(true);
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
                    {
                        statsAffected.Heal(buff.amount);
                        if (target.healingBuffIndicator != null) target.healingBuffIndicator.SetActive(true);
                    }
                        
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

        Vector3 direction = (hitPositiion - projectileSpawnPos).normalized;
        Debug.Log("Projectile Direction: " +  direction.normalized);

        spawnedProjectile.GetComponent<Rigidbody>().velocity = direction * (Time.deltaTime + 10);
        Debug.Log("Projectile Velocity: " + spawnedProjectile.GetComponent<Rigidbody>().velocity);
    }

    public void SetProjectileSpawnPos(Vector3 pos)
    {
        projectileSpawnPos = pos;
    }

    public float GetCooldown()
    {
        return cooldown;
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






