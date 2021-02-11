using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability/Targeted")]
public class Targeted_Ability : Ability
{
    [SerializeField]
    TargetType targetType;

    public override void Use(GameObject user)
    {
        base.Use(user);

        if (cooldownTimer >= 0)
        {
            Debug.Log("Ability on cooldown");
            return;
        }
        cooldownTimer = cooldown;

        if (targetType == TargetType.Self)
        {
            //ability does thing to itself
            addBuff(user.GetComponent<Character_Stats>());
            return;
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            CharacterCombat character = hit.collider.GetComponent<CharacterCombat>();

            if (character != null)
            {

                switch (targetType)
                {
                    case TargetType.Ally:
                        //ability affects targeted ally
                        if (character.tag == "Ally" || character.tag == "Player")
                        {
                            Debug.Log("We hit: " + hit.collider.name + " " + hit.point);
                        }
                        break;

                    case TargetType.Enemy:
                        //ability affects targeted enemy
                        if (character.gameObject.tag == "Enemy")
                        {
                            Debug.Log("We hit: " + hit.collider.name + " " + hit.point);
                        }
                        break;

                    case TargetType.Any:
                        //ability affects targeted character
                        Debug.Log("We hit: " + hit.collider.name + " " + hit.point);

                        break;

                    default:
                        //defaults to self
                        break;
                }
            }
        }
        

    }


}

