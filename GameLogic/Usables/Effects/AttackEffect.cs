namespace GameLogic.Usables.Effects;

public class AttackEffectConfig : EffectConfig
{
    public int Damage { get; init; }

    public AttackEffectConfig(string id, string type, string subtype, string variant, int damage)
        : base(id, type, subtype, variant)
    {
        this.Damage = damage;
    }
}
