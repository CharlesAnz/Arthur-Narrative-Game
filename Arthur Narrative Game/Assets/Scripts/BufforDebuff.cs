using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BufforDebuff
{
    public float amount;

    public StatBuffs affects;

    public float duration;

    public float durationTimer;

    public bool ramping;
}
