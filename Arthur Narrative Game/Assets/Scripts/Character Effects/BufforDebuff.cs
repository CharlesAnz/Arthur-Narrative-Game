[System.Serializable]
public class BufforDebuff
{
    public float amount;

    public StatBuffs affects;

    public float duration;

    public float durationTimer;

    public bool ramping;
}
public enum StatBuffs { Damage, Armor, MoveSpeed, AttackSpeed, Health }