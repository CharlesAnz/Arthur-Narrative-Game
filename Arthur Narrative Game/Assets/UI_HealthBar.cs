using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{
    private Slider slider;

    private Text nameTxt;

    private void Start()
    {
        slider = GetComponent<Slider>();
        nameTxt = GetComponentInChildren<Text>();
    }

    public void SetMaxHP(int hp)
    {
        slider.maxValue = hp;
    }

    public void SetCurHP(int hp)
    {
        slider.value = hp;
    }

    public void SetNameTxt(string newName)
    {
        nameTxt.text = newName;
    }
}
