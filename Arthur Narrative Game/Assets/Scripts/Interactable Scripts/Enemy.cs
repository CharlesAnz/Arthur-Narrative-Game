using UnityEngine;

public class Enemy : Interactable
{
    Character_Stats myStats;
    CharacterCombat interactorCombat;

    private void Start()
    {
        myStats = GetComponent<Character_Stats>();
    }

    public override void Interact(GameObject interactor)
    {
        base.Interact(interactor);

        interactorCombat = interactor.GetComponent<CharacterCombat>();
        if (interactorCombat != null)
        {
            interactorCombat.Attack(myStats);
        }
    }

    private void Update()
    {
        if (isFocus && myStats.dead)
        {
            interactorCombat.GetComponent<CharacterAnimator>().characterAnim.SetBool("basicAttack", false);
            return; 
        }

        //If this interactable is the focus of a character and not interacted with yet
        //check if character is close enough to interact, if so, set hasInteracted to true
        if (isFocus)
        {
            if (!hasInteracted)
            {
                float distance = Vector3.Distance(interactor.position, interactionTransform.position);
                if (distance <= radius)
                {
                    Interact(interactor.gameObject);
                    hasInteracted = true;
                }
            }

            //if the enemy has been attacked, then continue attacking as long they're still focused
            //and their attack cooldown allows it
            else if (hasInteracted && interactorCombat.GetAttackCooldown() <= 0)
            {
                float distance = Vector3.Distance(interactor.position, interactionTransform.position);
                if (distance <= radius)
                {
                    Interact(interactor.gameObject);
                }
                if (distance > radius)
                    interactorCombat.GetComponent<CharacterAnimator>().characterAnim.SetBool("basicAttack", false);
            }
        }
        else
        {
            if (interactorCombat != null)
            {
                interactorCombat.GetComponent<CharacterAnimator>().characterAnim.SetBool("basicAttack", false);
            }
        }

    }

}
