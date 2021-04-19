using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonBoss_Stats : Character_Stats
{
    PlayerManager playerManager;
    DragonSounds dragonsounds;
    Vector3 startPos;

    private void Start()
    {
        startPos = gameObject.transform.position;
        playerManager = PlayerManager.instance;
        dragonsounds = GetComponent<DragonSounds>();

        foreach (Ability ability in abilities)
        {
            ability.SetCam(Camera.main);
            ability.cooldownTimer = 0;
            ability.listenersAdded = false;
        }
        curHP = maxHP.GetValue();

        maxHP.statName = "maxHP";
        damage.statName = "damage";
        armor.statName = "armor";
        moveSpeed.statName = "moveSpeed";
        attackSpeed.statName = "attackspeed";
    }

    public void ResetStats()
    {
        foreach (Ability ability in abilities)
        {
            ability.cooldownTimer = 0;
        }

        for (int i = 0; i < buffs.Count; i++)
        {
            buffs[i].durationTimer = 0;
        }

        List<float> damageMods =  damage.GetMods();
        List<float> armorMods = armor.GetMods();

        foreach(float mod in damageMods)
        {
            damage.RemoveModifier(mod);
        }

        foreach (float mod in armorMods)
        {
            armor.RemoveModifier(mod);
        }

        GetComponent<CharacterAnimator>().characterAnim.SetBool("basicAttack", false);
        GetComponent<CharacterAnimator>().characterAnim.ResetTrigger(abilities[0].animatorTrigger);
        GetComponent<CharacterAnimator>().characterAnim.ResetTrigger(abilities[1].animatorTrigger);
        GetComponent<CharacterAnimator>().characterAnim.ResetTrigger(abilities[2].animatorTrigger);
        GetComponent<CharacterAnimator>().characterAnim.SetTrigger("reset");
        

        curHP = maxHP.GetValue();

        transform.position = startPos;
    }

    public override void Die()
    {
        base.Die();
        playerManager.WinCondition(gameObject);
        MusicManager.instance.StopLoop();
    }
    public override void PlaySoundOnHit()
    {
        base.PlaySoundOnHit();
        dragonsounds.PlaySound(0);
    }
}
