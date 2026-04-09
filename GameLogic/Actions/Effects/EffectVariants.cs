namespace GameLogic.Actions.Effects;

using GameLogic.Actions.Usables;
using GameLogic.Entities;
using GameLogic.Targeting;

public interface IEffectVariant
{
    public EffectResult<IEffectVariant> GetResult(IUser user, ITargetable target)
    {
        return new EffectResult<IEffectVariant>(this, user, target, 0f, 1f, 0f, false);
    }

    public void Apply();
}

public interface IAttackEffectVariant : IEffectVariant
{
    public float Value { get; set; }
    public float CritChance { get; set; }

    public new EffectResult<IAttackEffectVariant> GetResult(IUser user, ITargetable target)
    {
        return new EffectResult<IAttackEffectVariant>(
            this,
            user,
            target,
            this.Value,
            1f,
            0f,
            false
        );
    }

    public new void Apply()
    {
        this.Target.TakeDamage(this.Value);
    }
}

public interface IHealEffectVariant : IEffectVariant
{
    public float Value { get; set; }

    public new EffectResult<IHealEffectVariant> GetResult(IUser user, ITargetable target)
    {
        return new EffectResult<IHealEffectVariant>(this, user, target, this.Value, 1f, 0f, false);
    }
}

public enum EBuffType
{
    Positive,
    Negative
}

public interface IBuffEffectVariant : IEffectVariant
{
    public float Value { get; set; }
    public float Duration { get; set; }

    public new EffectResult<IBuffEffectVariant> GetResult(IUser user, ITargetable target)
    {
        return new EffectResult<IBuffEffectVariant>(this, user, target, this.Value, 1f, 0f, false);
    }
}

public interface IDebuffEffectVariant : IEffectVariant
{
    public float Value { get; set; }
    public float Duration { get; set; }

    public new EffectResult<IDebuffEffectVariant> GetResult(IUser user, ITargetable target)
    {
        return new EffectResult<IDebuffEffectVariant>(
            this,
            user,
            target,
            this.Value,
            1f,
            0f,
            false
        );
    }
}

public interface IStatusEffectVariant : IEffectVariant
{
    public float Value { get; set; }

    public new EffectResult<IStatusEffectVariant> GetResult(IUser user, ITargetable target)
    {
        return new EffectResult<IStatusEffectVariant>(
            this,
            user,
            target,
            this.Value,
            1f,
            0f,
            false
        );
    }
}
