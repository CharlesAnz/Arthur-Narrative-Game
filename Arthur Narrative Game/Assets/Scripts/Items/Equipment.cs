using UnityEngine;

[CreateAssetMenu(fileName = "New Equipment", menuName = "Inventory/Equipment")]
public class Equipment : Item
{
    public EquipmentSlot equipSlot;

    public int armorMod;
    public int damageMod;

    ///equips piece of equipment
    public override void Use(GameObject interactor)
    {
        base.Use(interactor);
        EquipmentManager manager = interactor.GetComponent<EquipmentManager>();

        manager.Equip(this);
        RemoveFromInventory();
    }


}

//Slots that equipment can be placed in
public enum EquipmentSlot { Head, Chest, Legs, RHand, LHand, Feet }