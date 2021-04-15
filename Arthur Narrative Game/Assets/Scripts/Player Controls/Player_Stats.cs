using UnityEngine;

public class Player_Stats : Character_Stats
{
    EquipmentManager equipManager;
    PlayerManager playerManager;

    // Start is called before the first frame update
    void Start()
    {
        playerManager = PlayerManager.instance;

        equipManager = GetComponent<EquipmentManager>();
        equipManager.onEquipmentChanged += OnEquipmentChanged;

        foreach (Ability ability in abilities)
        {
            ability.SetCam(Camera.main);
        }
    }

    void OnEquipmentChanged(Equipment newItem, Equipment oldItem)
    {

        if (newItem != null)
        {
            armor.AddModifier(newItem.armorMod);
            damage.AddModifier(newItem.damageMod);
        }

        if (oldItem != null)
        {
            armor.RemoveModifier(oldItem.armorMod);
            damage.RemoveModifier(oldItem.damageMod);
        }

    }

    public override void Die()
    {
        base.Die();
        GetComponent<CharacterAnimator>().characterAnim.SetTrigger("death");
        playerManager.LoseCondition();
    }
}
