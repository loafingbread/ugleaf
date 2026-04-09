namespace GameLogic.Actions.Effects;

using GameLogic.Actions.Usables;
using GameLogic.Targeting;

public class EffectResults
{
    public EffectResult<IAttackEffectVariant>? Attack { get; set; }
    public EffectResult<IHealEffectVariant>? Heal { get; set; }
    public EffectResult<IBuffEffectVariant>? Buff { get; set; }
    public EffectResult<IStatusEffectVariant>? Status { get; set; }

    public EffectResults(
        EffectResult<IAttackEffectVariant>? attack,
        EffectResult<IHealEffectVariant>? heal,
        EffectResult<IBuffEffectVariant>? buff,
        EffectResult<IStatusEffectVariant>? status
    )
    {
        this.Attack = attack;
        this.Heal = heal;
        this.Buff = buff;
        this.Status = status;
    }
}

public class EffectResult<T>
    where T : IEffectVariant
{
    public T EffectVariant { get; set; }
    public IUser User { get; set; }
    public ITargetable Target { get; set; }
    public float Value { get; set; } = 0f;
    public float Resistance { get; set; } = 1f;
    public float Duration { get; set; } = 0f;
    public bool DidCrit { get; set; } = false;

    public EffectResult(
        T effectVariant,
        IUser user,
        ITargetable target,
        float value,
        float resistance,
        float duration,
        bool didCrit
    )
    {
        this.EffectVariant = effectVariant;
        this.User = user;
        this.Target = target;

        this.Value = value;
        this.Resistance = resistance;
        this.Duration = duration;
        this.DidCrit = didCrit;
    }

    public void Apply()
    {
    }
}
