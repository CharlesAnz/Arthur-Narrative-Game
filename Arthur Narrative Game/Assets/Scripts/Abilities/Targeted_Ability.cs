using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability/Targeted")]
public class Targeted_Ability : Ability
{

    public float maxDistance;

    public override void Use(GameObject user)
    {
        base.Use(user);

        if (!Setup(user)) return;

        if (targetType == TargetType.Self)
        {
            //ability does thing to itself
            OnAbilityUse.Invoke(user.GetComponent<CharacterCombat>());
            return;
        }

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100))
        {
            CharacterCombat targetCharacter = hit.collider.GetComponent<CharacterCombat>();

            float distance = Vector3.Distance(targetCharacter.transform.position, user.transform.position);
            
            if(maxDistance > 0)
            {
                if (distance > maxDistance) return;
            }
            
            if (targetCharacter != null)
            {
                switch (targetType)
                {
                    case TargetType.Ally:
                        //ability affects targeted ally
                        if (targetCharacter.tag == "Ally" || targetCharacter.tag == "Player")
                        {
                            Debug.Log("We hit: " + hit.collider.name + " " + hit.point);
                            OnAbilityUse.Invoke(targetCharacter);
                        }
                        break;

                    case TargetType.Enemy:
                        //ability affects targeted enemy
                        if (targetCharacter.gameObject.tag == "Enemy")
                        {
                            Debug.Log("We hit: " + hit.collider.name + " at " + hit.point);
                            OnAbilityUse.Invoke(targetCharacter);
                        }
                        break;

                    case TargetType.Any:
                        //ability affects targeted character
                        Debug.Log("We hit: " + hit.collider.name + " at " + hit.point);
                        OnAbilityUse.Invoke(targetCharacter);

                        break;
                }
            }
        }
    }

}

