using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBoss_Stats : Character_Stats
{
    PlayerManager playerManager;

    private void Start()
    {
        playerManager = PlayerManager.instance;

        foreach (Ability ability in abilities)
        {
            ability.SetCam(Camera.main);
            ability.cooldownTimer = 0;
        }
        curHP = maxHP.GetValue();
    }

    public override void Die()
    {
        base.Die();
        GetComponent<CharacterAnimator>().characterAnim.SetTrigger("dead");
        playerManager.WinCondtion();
    }
}
