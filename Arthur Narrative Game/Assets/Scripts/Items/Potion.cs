using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Potion")]
public class Potion : Item
{
    [SerializeField]
    private int value;

    [SerializeField]
    private bool doesHealing;

    private UnityEvent<CharacterCombat> OnPotionUse;

    public List<BufforDebuff> buffList = new List<BufforDebuff>();

    public override void Use(GameObject interactor)
    {
        base.Use(interactor);

        user = interactor;
        if (doesHealing) OnPotionUse.AddListener(Heal);
        if (buffList != null) OnPotionUse.AddListener(addBuff);

        RemoveFromInventory();
    }

    private void Heal(CharacterCombat target)
    {
        user.GetComponent<CharacterCombat>().AbilityHeal(target.GetMyStats(), value);
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
}
