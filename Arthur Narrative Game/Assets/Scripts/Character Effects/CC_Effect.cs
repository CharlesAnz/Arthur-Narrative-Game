[System.Serializable]
public class CC_Effect
{
    public StatusEffects affect;

    public float duration;

    public float durationTimer;
}

public enum StatusEffects { Stun, Root, Silence, Disarm }
