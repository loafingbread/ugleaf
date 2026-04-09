namespace GameLogic.Actions.Effects;

public interface IEffectVariant { }

public interface IAttackEffectVariant : IEffectVariant
{
    public float Value { get; set; }
    public float CritChance { get; set; }
}

public interface IHealEffectVariant : IEffectVariant
{
    public float Value { get; set; }
}

public interface IBuffEffectVariant : IEffectVariant
{
    public float Value { get; set; }
}

public interface IDebuffEffectVariant : IEffectVariant
{
    public float Value { get; set; }
}

public interface IStatusEffectVariant : IEffectVariant
{
    public float Value { get; set; }
}
