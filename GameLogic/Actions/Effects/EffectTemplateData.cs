namespace GameLogic.Actions.Effects;

using GameLogic.Registry;

public record EffectTemplateRef : TemplateRef<EffectTemplateData, EffectTemplatePatch> { }

public record EffectTemplateData
{
    public required string DefaultName { get; set; }
    public required string DefaultDescription { get; set; }
    public required List<string> DefaultTags { get; set; }
    public required EffectModelData DefaultModel { get; set; }

    public EffectTemplateData DeepCopy()
    {
        return new EffectTemplateData()
        {
            DefaultName = this.DefaultName,
            DefaultDescription = this.DefaultDescription,
            DefaultTags = [.. this.DefaultTags],
            DefaultModel = this.DefaultModel.DeepCopy(),
        };
    }
}

public record EffectModelData
{
    public required AttackEffectVariantData? Attack { get; set; }
    public required HealEffectVariantData? Heal { get; set; }
    public required BuffEffectVariantData? Buff { get; set; }
    public required StatusEffectVariantData? Status { get; set; }

    public EffectModelData DeepCopy()
    {
        return new EffectModelData()
        {
            Attack = this.Attack?.DeepCopy(),
            Heal = this.Heal?.DeepCopy(),
            Buff = this.Buff?.DeepCopy(),
            Status = this.Status?.DeepCopy(),
        };
    }
}

public record AttackEffectVariantData
{
    public required float Value { get; set; }
    public required float CritChance { get; set; }

    public AttackEffectVariantData DeepCopy()
    {
        return new AttackEffectVariantData() { Value = this.Value, CritChance = this.CritChance };
    }
}

public record HealEffectVariantData
{
    public required float Value { get; set; }

    public HealEffectVariantData DeepCopy()
    {
        return new HealEffectVariantData() { Value = this.Value };
    }
}

public record BuffEffectVariantData
{
    public required float Value { get; set; }
    public required float Duration { get; set; }

    public BuffEffectVariantData DeepCopy()
    {
        return new BuffEffectVariantData() { Value = this.Value, Duration = this.Duration };
    }
}

public record StatusEffectVariantData
{
    public required float Value { get; set; }
    public required float Duration { get; set; }

    public StatusEffectVariantData DeepCopy()
    {
        return new StatusEffectVariantData() { Value = this.Value, Duration = this.Duration };
    }
}

public record EffectTemplatePatch
{
    public string? DefaultType { get; init; }
    public string? DefaultSubtype { get; init; }
    public string? DefaultVariant { get; init; }
    public string? DefaultName { get; init; }
    public string? DefaultDescription { get; init; }
    public List<string>? DefaultTags { get; init; }
    public EffectConfigData? DefaultConfig { get; init; }
}
