using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

//This is for holding the stats of any character and is used mostly for combat purposes
public class Character_Stats : MonoBehaviour
{
    public float curHP { get; private set; }

    public Stat maxHP;
    public Stat damage;
    public Stat armor;
    public Stat moveSpeed;
    public Stat attackSpeed;

    public List<Ability> abilities = new List<Ability>();

    public List<BufforDebuff> buffs = new List<BufforDebuff>();

    private void Start()
    {
        foreach(Ability ability in abilities)
        {
            ability.SetCam(Camera.main);
            ability.cooldownTimer = 0;
        }
        curHP = maxHP.GetValue();
    }

    //Method for taking damage, damage is subtracted by the amount of armor and damage min is 0
    public void TakeDam (float damage)
    {
        if (armor.GetValue() > 0)
        { 
            damage -= armor.GetValue(); 
        }
        
        damage = Mathf.Clamp(damage, 0, int.MaxValue);

        curHP -= damage;
        Debug.Log(gameObject + " takes " + damage + " damage");

        if (curHP <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        amount = Mathf.Clamp(amount, 0, int.MaxValue);

        curHP += amount;
        Debug.Log(gameObject + " heals for " + amount + " of health");

        if (curHP >= maxHP.GetValue())
        {
            curHP = maxHP.GetValue();
        }
    }

    public virtual void Die()
    {


    }
}
