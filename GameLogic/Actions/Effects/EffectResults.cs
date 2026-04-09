namespace GameLogic.Actions.Effects;

public class EffectResults
{
    public EffectResult<IAttackEffectVariant>? Attack { get; set; }
    public EffectResult<IHealEffectVariant>? Heal { get; set; }
    public EffectResult<IBuffEffectVariant>? Buff { get; set; }
    public EffectResult<IDebuffEffectVariant>? Debuff { get; set; }
    public EffectResult<IStatusEffectVariant>? Status { get; set; }

    public EffectResults(
        EffectResult<IAttackEffectVariant>? attack,
        EffectResult<IHealEffectVariant>? heal,
        EffectResult<IBuffEffectVariant>? buff,
        EffectResult<IDebuffEffectVariant>? debuff,
        EffectResult<IStatusEffectVariant>? status
    )
    {
        this.Attack = attack;
        this.Heal = heal;
        this.Buff = buff;
        this.Debuff = debuff;
        this.Status = status;
    }
}

public class EffectResult<T>
    where T : IEffectVariant
{
    public T Effect { get; set; }
    public float Value { get; set; } = 0f;
    public float Resistance { get; set; } = 1f;
    public float Duration { get; set; } = 0f;
    public bool DidCrit { get; set; } = false;

    public EffectResult(T effect, float value, float resistance, float duration, bool didCrit)
    {
        this.Effect = effect;
        this.Value = value;
        this.Resistance = resistance;
        this.Duration = duration;
        this.DidCrit = didCrit;
    }
}
