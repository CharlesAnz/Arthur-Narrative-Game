using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CC_Displacement
{
    public DisplacementEffect pushOrPull;

    public float distance;
}

public enum DisplacementEffect { Push, Pull}

