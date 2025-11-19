namespace GameLogic.Entities.Stats;

using GameLogic.Registry;
using GameLogic.Utils;

/// <summary>
/// A stat is a value that can be modified by modifiers.
/// </summary>
public abstract class Stat : IDeepCopyable<Stat>, IReferenceUnion
{
    public ReferenceUnionMetadata ReferenceMetadata { get; set; }
    public StatOverrideRecord? TemplateOverride { get; set; }
    public StatRecord? InstanceState { get; set; }
    public StatMetadataRecord Metadata { get; private set; }
    public IStatConfigRecord Config { get; private set; }
    public StatType Type { get; private set; }

    public StatModifiers Modifiers { get; private set; } = new();
    public int BaseValue { get; protected set; }
    public int CurrentValue { get; protected set; }

    public Stat(
        ReferenceUnionMetadata referenceMetadata,
        StatRecord? instanceState,
        StatTemplateRecord? templateRecord,
        StatOverrideRecord? templateOverride
    )
    {
        if (referenceMetadata.Kind == EReferenceKind.Inline && templateRecord is null)
        {
            throw new InvalidOperationException("Inline stats must have a template record");
        }
        else if (referenceMetadata.Kind == EReferenceKind.Override && templateOverride is null)
        {
            throw new InvalidOperationException("Override stats must have a template override");
        }
        else if (
            referenceMetadata.Kind != EReferenceKind.Instance
            && referenceMetadata.Kind != EReferenceKind.Ref
        )
        {
            throw new InvalidOperationException("Invalid reference kind");
        }

        this.ReferenceMetadata = referenceMetadata;

        this.ApplyTemplateRecord(templateRecord);
        this.TemplateOverride = templateOverride;

        this.InstanceState = instanceState;
        this.ApplyTemplateRecord(instanceState);

        this.Modifiers = new StatModifiers();
    }

    // TODO Remove ApplyInstanceState since this can replace it
    private void ApplyTemplateRecord(StatTemplateRecord? templateRecord)
    {
        if (templateRecord is null)
        {
            return;
        }

        this.Metadata = templateRecord.Metadata;
        this.Config = StatFactory.CopyStatConfig(templateRecord.Config);
        this.Type = templateRecord.Type;
    }

    public Stat(Stat stat)
    {
        this.ReferenceMetadata = stat.ReferenceMetadata;
        this.Metadata = new StatMetadataRecord
        {
            Name = stat.Metadata.Name,
            DisplayName = stat.Metadata.DisplayName,
            Description = stat.Metadata.Description,
            Tags = stat.Metadata.Tags,
        };
        this.Config = StatFactory.CopyStatConfig(stat.Config);
        this.Type = stat.Type;
        this.Modifiers = new StatModifiers();
    }

    public void LoadReferences(IRegistry registry) { }

    public abstract Stat DeepCopy();

    /// <summary>
    /// Checks if the stat is derived from a formula or a constant.
    ///
    /// If the stat is derived from a formula, the base value should be calculated using the formula
    /// every time the stat is refreshed.
    ///
    /// If the stat is a constant, the base value should be set using the constant formula and
    /// updated with ChangeBaseValue method. Formula should never be used except during initialization.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the stat is derived from a formula, <c>false</c> if it is a constant.
    /// </returns>
    public abstract bool IsFormulaCalculated();

    /// <summary>
    /// Updates the stat value to reflect the current base value and current modifiers.
    ///
    /// Recalculates the base value if the stat is derived from a formula otherwise
    /// it uses the current base value.
    ///
    /// Should be called when the stat is updated.
    /// </summary>
    public abstract void OnUpdate();
}

public sealed class StatReference : Reference<Stat>
{
    public StatReference(ReferenceUnionMetadata metadata, Stat? value)
        : base(metadata, value) { }
}
