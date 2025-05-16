namespace GameLogic.Usables.Effects;

public class HealEffectConfig : EffectConfig
{
    public int Value { get; init; }

    public HealEffectConfig(string id, string type, string subtype, string variant, int value)
        : base(id, type, subtype, variant)
    {
        this.Value = value;
    }
}
