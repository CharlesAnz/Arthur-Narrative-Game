using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Ability : ScriptableObject
{
    new public string name = "New Ability";

    public string description = "ability does thing";
    protected GameObject user = null;
    public Sprite icon = null;

    public List<BufforDebuff> buffList = new List<BufforDebuff>();

    public List<CC_Effect> cc_Effects = new List<CC_Effect>();

    [SerializeField]
    private CC_Displacement displacement;

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
        Debug.Log(interactor.name + " is using " + name);
        user = interactor;
    }

    //Checks if the cooldown is below, if not then nothing happens
    protected bool Setup(GameObject interactor)
    {
        CharacterCombat combat = interactor.GetComponent<CharacterCombat>();
        CharacterAnimator anim = interactor.GetComponent<CharacterAnimator>();

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

        cooldownTimer = cooldown;

        if (anim != null)
        {
            anim.characterAnim.SetTrigger(animatorTrigger);
        }

        combat.CastTime = castTime;
        combat.SetAttackCooldown(castTime);

        if (doesDamage) OnAbilityUse.AddListener(Damage);
        if (doesHealing) OnAbilityUse.AddListener(Heal);
        if (buffList != null) OnAbilityUse.AddListener(addBuff);
        if (cc_Effects != null) OnAbilityUse.AddListener(addCC_Effect);
        if (displacement.distance != 0) OnAbilityUse.AddListener(addDisplacement);


        return true;
    }

    private void Damage(CharacterCombat target)
    {
        user.GetComponent<CharacterCombat>().AbilityHit(target.GetMyStats(), abilityValue);
    }

    private void Heal(CharacterCombat target)
    {
        user.GetComponent<CharacterCombat>().AbilityHeal(target.GetMyStats(), abilityValue);
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
                    if(buff.amount < 0)
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

    }

    private void addCC_Effect(CharacterCombat target)
    {
        foreach (CC_Effect effect in cc_Effects)
        {
            effect.durationTimer = effect.duration;
            target.cc_Effects.Add(effect);
        }
    }

    public void SetCam(Camera newCam) {  cam = newCam; }

}

public enum TargetType { Self, Ally, Enemy, Any }

  




