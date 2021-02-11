using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //[HideInInspector]
    public float cooldownTimer = 0;


    protected Camera cam;

    public virtual void Use(GameObject interactor)
    {
        Debug.Log(interactor.name + " is using " + name);
        user = interactor;
    }

    public void addBuff(Character_Stats statsAffected)
    {
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
                    statsAffected.TakeDam(buff.amount);
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

public enum StatBuffs { Damage, Armor, MoveSpeed, AttackSpeed, Health }  




