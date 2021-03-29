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

    [SerializeField]
    protected bool doesDamage, doesHealing;

    [SerializeField]
    protected float abilityValue;
    
    [SerializeField]
    protected float cooldown;

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

        if (doesDamage) OnAbilityUse.AddListener(Damage);
        if (doesHealing) OnAbilityUse.AddListener(Heal);
        if (buffList != null) OnAbilityUse.AddListener(addBuff);
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

    public void SetCam(Camera newCam) {  cam = newCam; }

}

public enum TargetType { Self, Ally, Enemy, Any }

  




