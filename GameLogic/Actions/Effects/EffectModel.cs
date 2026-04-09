namespace GameLogic.Actions.Effects;

public interface IEffectModel
{
    public IAttackEffectVariant? Attack { get; }
    public IHealEffectVariant? Heal { get; }
    public IBuffEffectVariant? Buff { get; }
    public IDebuffEffectVariant? Debuff { get; }
    public IStatusEffectVariant? Status { get; }
}

public class EffectModel : IEffectModel
{
    public IAttackEffectVariant? Attack { get; init; }
    public IHealEffectVariant? Heal { get; init; }
    public IBuffEffectVariant? Buff { get; init; }
    public IDebuffEffectVariant? Debuff { get; init; }
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
        this.Debuff = debuff;
        this.Status = status;
    }

    public EffectResult<IAttackEffectVariant> ApplyAttack()
    {
        if (this.Attack == null)
        {
            throw new InvalidOperationException("Attack effect is not set.");
        }

        return new EffectResult<IAttackEffectVariant>(
            this.Attack,
            this.Attack.Value,
            1f,
            0f,
            false
        );
    }

    public EffectResult<IHealEffectVariant> ApplyHeal()
    {
        if (this.Heal == null)
        {
            throw new InvalidOperationException("Heal effect is not set.");
        }

        return new EffectResult<IHealEffectVariant>(this.Heal, this.Heal.Value, 1f, 0f, false);
    }

    public EffectResult<IBuffEffectVariant> ApplyBuff()
    {
        if (this.Buff == null)
        {
            throw new InvalidOperationException("Buff effect is not set.");
        }

        return new EffectResult<IBuffEffectVariant>(this.Buff, this.Buff.Value, 1f, 0f, false);
    }

    public EffectResult<IDebuffEffectVariant> ApplyDebuff()
    {
        if (this.Debuff == null)
        {
            throw new InvalidOperationException("Debuff effect is not set.");
        }

        return new EffectResult<IDebuffEffectVariant>(
            this.Debuff,
            this.Debuff.Value,
            1f,
            0f,
            false
        );
    }

    public EffectResult<IStatusEffectVariant> ApplyStatus()
    {
        if (this.Status == null)
        {
            throw new InvalidOperationException("Status effect is not set.");
        }

        return new EffectResult<IStatusEffectVariant>(
            this.Status,
            this.Status.Value,
            1f,
            0f,
            false
        );
    }

    public EffectResults Apply()
    {
        EffectResult<IAttackEffectVariant>? attackResult = null;
        EffectResult<IHealEffectVariant>? healResult = null;
        EffectResult<IBuffEffectVariant>? buffResult = null;
        EffectResult<IDebuffEffectVariant>? debuffResult = null;
        EffectResult<IStatusEffectVariant>? statusResult = null;

        if (this.Attack != null)
        {
            attackResult = this.ApplyAttack();
        }
        if (this.Heal != null)
        {
            healResult = this.ApplyHeal();
        }
        if (this.Debuff != null)
        {
            debuffResult = this.ApplyDebuff();
        }
        if (this.Status != null)
        {
            statusResult = this.ApplyStatus();
        }

        return new EffectResults(attackResult, healResult, buffResult, debuffResult, statusResult);
    }
}
