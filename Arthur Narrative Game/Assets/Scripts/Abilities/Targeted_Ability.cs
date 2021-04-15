using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability/Targeted")]
public class Targeted_Ability : Ability
{
    public CharacterCombat targetHit;

    public override void Use(GameObject user)
    {
        base.Use(user);

        Vector3 point = Vector3.zero;

        if (!Setup(user)) return;

        if (targetType == TargetType.Self)
        {
            //ability does thing to itself
            OnAbilityUse.Invoke(user.GetComponent<CharacterCombat>());
            return;
        }

        else if (user.tag == "Player")
        {
            point = FindTargetWithMouse(maxDistance);
        }

        CharacterCombat targetCharacter = targetHit;
        if (targetCharacter == null) return;

        if (projectile != null)
        {
            if (delay > 0)
            {
                user.GetComponent<CharacterCombat>().SpawnProjectile(targetCharacter.transform.position, this);
            }
            else
            {
                SpawnProjectile(targetCharacter.transform.position);
            }
            return;
        }

        float distance = Vector3.Distance(targetCharacter.transform.position, user.transform.position);

        if (maxDistance > 0)
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
                        Debug.Log("We hit: " + targetCharacter.name + " " + point);
                        if (delay > 0)
                            user.GetComponent<CharacterCombat>().UseAbility(targetCharacter, this);
                        else
                            OnAbilityUse.Invoke(targetCharacter);
                    }
                    break;

                case TargetType.Enemy:
                    //ability affects targeted enemy
                    if (targetCharacter.gameObject.tag == "Enemy")
                    {
                        Debug.Log("We hit: " + targetCharacter.name + " at " + point);
                        if (delay > 0)
                            user.GetComponent<CharacterCombat>().UseAbility(targetCharacter, this);
                        else
                            OnAbilityUse.Invoke(targetCharacter);
                    }
                    break;

                case TargetType.Any:
                    //ability affects targeted character
                    Debug.Log("We hit: " + targetCharacter.name + " at " + point);
                    if (delay > 0)
                        user.GetComponent<CharacterCombat>().UseAbility(targetCharacter, this);
                    else
                        OnAbilityUse.Invoke(targetCharacter);
                    break;

                case TargetType.AnyExcludingSelf:
                    if (targetCharacter.gameObject != abilityUser)
                    {
                        Debug.Log("We hit: " + targetCharacter.name + " at " + point);
                        if (delay > 0)
                            user.GetComponent<CharacterCombat>().UseAbility(targetCharacter, this);
                        else
                            OnAbilityUse.Invoke(targetCharacter);
                    }

                    break;
            }
        }

    }

    public void FindTarget(CharacterCombat combat)
    {
        targetHit = combat;
    }

    protected override Vector3 FindTargetWithMouse(float maxCastDistance)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, maxCastDistance))
        {
            targetHit = hit.collider.GetComponent<CharacterCombat>();
            return hit.point;
        }

        else return Vector3.zero;
    }



}

