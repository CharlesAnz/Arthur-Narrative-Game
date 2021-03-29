using UnityEngine;
using UnityEngine.UI;

public class AbilitySlot : MonoBehaviour
{
    public Image icon;
    public Text description;

    public Player_Controller playerController;

    public KeyCode activationKey;

    Ability ability;


    //Adds item to inventory slot
    public void AddAbility(Ability newAbility)
    {
        ability = newAbility;

        icon.sprite = ability.icon;
        icon.enabled = true;

        description.text = newAbility.description;
    }

    //clears inventory slot
    public void ClearSlot()
    {
        ability = null;

        icon.sprite = null;
        icon.enabled = false;
        description.text = "";
    }

    //uses item in inventory slot
    public void UseAbility()
    {
        if (ability != null)
        {
            ability.Use(playerController.gameObject);
        }
    }
}
