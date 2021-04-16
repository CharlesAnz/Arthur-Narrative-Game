using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBoss_Stats : Character_Stats
{
    PlayerManager playerManager;
    DragonSounds dragonsounds;

    private void Start()
    {
        playerManager = PlayerManager.instance;
        dragonsounds = GetComponent<DragonSounds>();

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
        playerManager.WinCondtion(gameObject);
    }
    public override void PlaySoundOnHit()
    {
        base.PlaySoundOnHit();
        dragonsounds.PlaySound(0);
    }
}
