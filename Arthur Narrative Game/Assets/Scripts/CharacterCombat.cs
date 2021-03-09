using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This controls combat between 2 characters
public class CharacterCombat : MonoBehaviour
{
    Character_Stats myStats;
    //Enemy focusedEnemy;

    float nextTime = 0;

    public float attackSpeed;
    private float attackCooldown = 0f;


    private void Start()
    {
        myStats = GetComponent<Character_Stats>();
        attackSpeed = myStats.attackSpeed.GetValue();
    }

    private void Update()
    {
        attackSpeed = myStats.attackSpeed.GetValue();

        attackCooldown -= Time.deltaTime;
        foreach(Ability ability in myStats.abilities)
        {
            ability.cooldownTimer -= Time.deltaTime;
        }

        ManageBuffs();
    }

    //method for attackng another character
    public void Attack (Character_Stats targetStats)
    {
        //if attack not on cooldown
        if (attackCooldown <= 0f)
        {
            //tells attack target's stats that they take damage
            targetStats.TakeDam(myStats.damage.GetValue());
            //resets attack timer
            attackCooldown = 1f / attackSpeed;
            //Keeps attacking target afterwards
        }
    }

    public Character_Stats GetMyStats(){ return myStats; }

    public void AbilityHit(Character_Stats targetStats, float mod)
    {
        //tells target's stats that they take damage equal myStats damage variable
        targetStats.TakeDam(myStats.damage.GetValue() + mod);
    }

    public void AbilityHeal(Character_Stats targetStats, float healAmount)
    {
        //tells target's stats that they heal for healAmount HP
        targetStats.Heal(healAmount);
    }

    void ManageBuffs()
    {
        for (int i = 0; i < myStats.buffs.Count; i++)
        {
            BufforDebuff buff = myStats.buffs[i];

            buff.durationTimer -= Time.deltaTime;

            //if the buff is a ramping effect that adds every second, then update the stat every second here
            if (buff.ramping)
            {
                if (Time.time >= nextTime)
                {
                    switch (buff.affects)
                    {
                        case StatBuffs.Armor:
                            myStats.armor.AddModifier(buff.amount);
                            break;
                        case StatBuffs.AttackSpeed:
                            myStats.attackSpeed.AddModifier(buff.amount);
                            break;
                        case StatBuffs.Health:
                            if (buff.amount < 0)
                                myStats.TakeDam(buff.amount);
                            else
                                myStats.Heal(buff.amount);
                            break;
                        case StatBuffs.Damage:
                            myStats.damage.AddModifier(buff.amount);
                            break;
                        case StatBuffs.MoveSpeed:
                            myStats.moveSpeed.AddModifier(buff.amount);
                            break;

                            
                    }
                    //do something here every interval seconds
                    nextTime = Mathf.FloorToInt(Time.time) + 1;
                }

            }
            
            //removes buff if it's duration is 0
            if (buff.durationTimer <= 0)
            {
                switch (buff.affects)
                {
                    case StatBuffs.Armor:
                        if (buff.ramping)
                        {
                            for (int y = 0; y < buff.duration; y++)
                            {
                                myStats.armor.RemoveModifier(buff.amount);
                            }
                        }
                        else
                            myStats.armor.RemoveModifier(buff.amount);
                        break;
                    
                    
                    case StatBuffs.AttackSpeed:
                        if (buff.ramping)
                        {
                            for (int y = 0; y < buff.duration; y++)
                            {
                                myStats.attackSpeed.RemoveModifier(buff.amount);
                            }
                        }
                        else
                            myStats.attackSpeed.RemoveModifier(buff.amount);
                        break;
                    
                    
                    case StatBuffs.Damage:
                        if (buff.ramping)
                        {
                            for (int y = 0; y < buff.duration; y++)
                            {
                                myStats.damage.RemoveModifier(buff.amount);
                            }
                        }
                        else
                            myStats.damage.RemoveModifier(buff.amount);
                        break;
                    
                    
                    case StatBuffs.MoveSpeed:
                        if (buff.ramping)
                        {
                            for (int y = 0; y < buff.duration; y++)
                            {
                                myStats.moveSpeed.RemoveModifier(buff.amount);
                            }
                        }
                        else
                            myStats.moveSpeed.RemoveModifier(buff.amount);
                        break;
                }
                myStats.buffs.RemoveAt(i);
            }
        }
    }

    public float GetAttackCooldown() { return attackCooldown; }




}
