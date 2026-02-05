namespace GameLogic.Entities.Stats.Capabilities;

public interface IRegen
{
    float RegenRate { get; set; }
    float RegenAmount { get; set; }
    float ApplyRegen(float value);
}

public class RegenCapability : IRegen
{
    public float RegenRate { get; set; }
    public float RegenAmount { get; set; }

    public RegenCapability(float regenRate, float regenAmount)
    {
        this.RegenRate = regenRate;
        this.RegenAmount = regenAmount;
    }

    public float ApplyRegen(float value)
    {
        return value + this.RegenAmount;
    }
}
