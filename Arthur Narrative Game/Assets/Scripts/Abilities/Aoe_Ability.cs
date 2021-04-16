using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Ability", menuName = "Ability/Aoe")]
public class Aoe_Ability : Ability
{
    public float areaSize;

    [SerializeField]
    private AOEType aoeType;

    //public float endPoint;

    //[HideInInspector]
    private Vector3 origin;

    public bool selfOrigin = false;

    [SerializeField]
    private List<CharacterCombat> targets = new List<CharacterCombat>();

    public override void Use(GameObject user)
    {
        base.Use(user);

        if (!Conditions(user)) return;

        if (selfOrigin) origin = user.transform.position;

        else if (user.tag == "Player")
        {
            origin = FindTargetWithMouse(100);
        }

        displacePos = origin;

        if(projectile != null)
        {
            if (delay > 0)
                user.GetComponent<CharacterCombat>().SpawnProjectile(origin, this);
            else
                SpawnProjectile(origin);

            return;
        }

        

        if (origin != null)
        {
            if (aoeType == AOEType.Cube || aoeType == AOEType.Sphere)
                CheckArea();
            else if (aoeType == AOEType.Cone)
                CheckCone();
            else if (aoeType == AOEType.Line)
                CheckLine();


            foreach (var target in targets)
            {
                //Debug.Log(target + " was hit at coordinates: " + target.transform.position);
                //Debug.Log("Ability hits");
                if (delay > 0)
                    user.GetComponent<CharacterCombat>().UseAbility(target, this);
                else
                    OnAbilityUse.Invoke(target);
            }
        }

        targets.Clear();
    }

    private void CheckCone()
    {
        Collider[] colliderArray = Physics.OverlapSphere(origin, areaSize);

        if (colliderArray != null)
        {
            foreach (var collider in colliderArray)
            {

                Vector3 directionTowardT = collider.transform.position - origin;
                float angleFromConeCenter = Vector3.Angle(directionTowardT, abilityUser.transform.TransformDirection(Vector3.forward));


                CharacterCombat colliderCombat = collider.gameObject.GetComponent<CharacterCombat>();

                if (colliderCombat != null && angleFromConeCenter <= 45)
                {
                    switch (targetType)
                    {
                        case TargetType.Ally:
                            if (colliderCombat.tag == "Ally" || colliderCombat.tag == "Player")
                                targets.Add(colliderCombat);

                            break;

                        case TargetType.Enemy:
                            if (colliderCombat.tag == "Enemy")
                                targets.Add(colliderCombat);

                            break;

                        case TargetType.Any:
                            targets.Add(colliderCombat);
                            break;

                        default:
                            targets.Add(colliderCombat);
                            break;
                    }
                }
            }
        }
    }

    private void CheckLine()
    {
        Ray aoeLineRay = new Ray(abilityUser.transform.position, abilityUser.transform.TransformDirection(abilityUser.transform.TransformDirection(Vector3.forward)) * 2);
        RaycastHit[] collidersHit = Physics.RaycastAll(aoeLineRay, areaSize);

        if (collidersHit != null)
        {
            foreach (var rayHit in collidersHit)
            {
                CharacterCombat colliderCombat = rayHit.collider.gameObject.GetComponent<CharacterCombat>();

                if (colliderCombat != null)
                {
                    switch (targetType)
                    {
                        case TargetType.Ally:
                            if (colliderCombat.tag == "Ally" || colliderCombat.tag == "Player") targets.Add(colliderCombat);
                            break;

                        case TargetType.Enemy:
                            if (colliderCombat.tag == "Enemy") targets.Add(colliderCombat);
                            break;

                        case TargetType.Any:
                            targets.Add(colliderCombat);
                            break;
                    }
                }
            }
        }
    }

    private void CheckArea()
    {
        Collider[] collidersNear = null;

        if (aoeType == AOEType.Cube)
        {
            collidersNear = Physics.OverlapBox(origin, new Vector3(areaSize, areaSize, areaSize));
        }

        else if (aoeType == AOEType.Sphere)
            collidersNear = Physics.OverlapSphere(origin, areaSize);

        if (collidersNear != null)
        {
            foreach (var collider in collidersNear)
            {
                CharacterCombat colliderCombat = collider.gameObject.GetComponent<CharacterCombat>();

                if (colliderCombat != null)
                {
                    switch (targetType)
                    {
                        case TargetType.Ally:
                            if (colliderCombat.tag == "Ally" || colliderCombat.tag == "Player") targets.Add(colliderCombat);
                            break;

                        case TargetType.Enemy:
                            if (colliderCombat.tag == "Enemy") targets.Add(colliderCombat);
                            break;

                        case TargetType.Any:
                            targets.Add(colliderCombat);
                            break;
                    }
                }
            }
        }
    }

    public void SetOrigin(Vector3 point) { origin = point; }
}

public enum AOEType { Sphere, Cube, Cone, Line }


