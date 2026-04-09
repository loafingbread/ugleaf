namespace GameLogic.Actions.Effects;

using GameLogic.Actions.Usables;
using GameLogic.Targeting;

public interface IEffectModel
{
    public IAttackEffectVariant? Attack { get; }
    public IHealEffectVariant? Heal { get; }
    public IBuffEffectVariant? Buff { get; }
    public IStatusEffectVariant? Status { get; }
}

public class EffectModel : IEffectModel
{
    public IAttackEffectVariant? Attack { get; init; }
    public IHealEffectVariant? Heal { get; init; }
    public IBuffEffectVariant? Buff { get; init; }
    public IStatusEffectVariant? Status { get; init; }

    public EffectModel(
        IAttackEffectVariant? attack,
        IHealEffectVariant? heal,
        IBuffEffectVariant? buff,
        IDebuffEffectVariant? debuff,
        IStatusEffectVariant? status
    )
    {
        this.Attack = attack;
        this.Heal = heal;
        this.Buff = buff;
        this.Status = status;
    }

    public EffectResult<IAttackEffectVariant> ApplyAttack(IUser user, ITargetable target)
    {
        if (this.Attack == null)
        {
            throw new InvalidOperationException("Attack effect is not set.");
        }

        return new EffectResult<IAttackEffectVariant>(
            this.Attack,
            user,
            target,
            this.Attack.Value,
            1f,
            0f,
            false
        );
    }

    public EffectResult<IHealEffectVariant> ApplyHeal(IUser user, ITargetable target)
    {
        if (this.Heal == null)
        {
            throw new InvalidOperationException("Heal effect is not set.");
        }

        return new EffectResult<IHealEffectVariant>(
            this.Heal,
            user,
            target,
            this.Heal.Value,
            1f,
            0f,
            false
        );
    }

    public EffectResult<IBuffEffectVariant> ApplyBuff(IUser user, ITargetable target)
    {
        if (this.Buff == null)
        {
            throw new InvalidOperationException("Buff effect is not set.");
        }

        return new EffectResult<IBuffEffectVariant>(
            this.Buff,
            user,
            target,
            this.Buff.Value,
            1f,
            0f,
            false
        );
    }

    public EffectResult<IStatusEffectVariant> ApplyStatus(IUser user, ITargetable target)
    {
        if (this.Status == null)
        {
            throw new InvalidOperationException("Status effect is not set.");
        }

        return new EffectResult<IStatusEffectVariant>(
            this.Status,
            user,
            target,
            this.Status.Value,
            1f,
            0f,
            false
        );
    }

    public EffectResults Apply(IUser user, ITargetable target)
    {
        EffectResult<IAttackEffectVariant>? attackResult = null;
        EffectResult<IHealEffectVariant>? healResult = null;
        EffectResult<IBuffEffectVariant>? buffResult = null;
        EffectResult<IStatusEffectVariant>? statusResult = null;

        if (this.Attack != null)
        {
            attackResult = this.ApplyAttack(user, target);
        }
        if (this.Heal != null)
        {
            healResult = this.ApplyHeal(user, target);
        }
        if (this.Buff != null)
        {
            buffResult = this.ApplyBuff(user, target);
        }
        if (this.Status != null)
        {
            statusResult = this.ApplyStatus(user, target);
        }

        return new EffectResults(attackResult, healResult, buffResult, statusResult);
    }
}
