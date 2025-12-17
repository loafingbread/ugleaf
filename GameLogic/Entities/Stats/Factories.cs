namespace GameLogic.Entities.Stats;

using GameLogic.Registry;

public static class StatFactory
{
    public static Reference<Stat, Stat> CreateStatReferenceFromRecord(ReferenceSpec record)
    {
        switch (record.Metadata.Kind)
        {
            case EReferenceKind.Ref:
                return CreateStatFromReference(record);
            case EReferenceKind.Inline:
                return CreateStatFromInline(record);
            case EReferenceKind.Override:
                return CreateStatFromOverride(record);
            case EReferenceKind.Instance:
                return CreateStatFromInstance(record);
            default:
                throw new NotImplementedException();
        }
    }

    public static Reference<Stat, Stat> CreateStatFromReference(ReferenceSpec record)
    {
        var refSpec = record as RefSpec;
        if (refSpec is null)
        {
            throw new InvalidOperationException("Ref spec is not a stat");
        }

        return new Reference<Stat, Stat>(
            refSpec.Metadata,
            CreateStat(refSpec.Metadata, null, null, null),
            null
        );
    }

    // TODO: Create all references afterwards, do not instantiate
    // until they are resolved

    public static Stat CreateStat(
        ReferenceMetadata metadata,
        StatData? instanceState,
        StatTemplateSpec? templateRecord,
        StatOverrideSpec? overrideRecord
    )
    {
        if (metadata.TemplateSubType == ETemplateSubType.StatValue)
        {
            return new ValueStat(metadata, instanceState, templateRecord, overrideRecord);
        }
        else if (metadata.TemplateSubType == ETemplateSubType.StatResource)
        {
            return new ResourceStat(metadata, instanceState, templateRecord, overrideRecord);
        }

        throw new InvalidOperationException("Invalid stat sub type");
    }

    public static Reference<Stat, Stat> CreateStatFromInline(ReferenceSpec record)
    {
        var inlineSpec = record as InlineSpec<StatTemplateSpec>;
        if (inlineSpec is null)
        {
            throw new InvalidOperationException("Inline spec is not a stat");
        }

        return new Reference<Stat, Stat>(
            inlineSpec.Metadata,
            CreateStat(inlineSpec.Metadata, null, inlineSpec.Template, null),
            null
        );
    }

    public static Reference<Stat, Stat> CreateStatFromOverride(ReferenceSpec record)
    {
        var overrideSpec = record as OverrideSpec<StatTemplateSpec, StatOverrideSpec>;
        if (overrideSpec is null)
        {
            throw new InvalidOperationException("Override spec is not a stat");
        }

        return new Reference<Stat, Stat>(
            overrideSpec.Metadata,
            CreateStat(overrideSpec.Metadata, null, null, overrideSpec.Override),
            null
        );
    }

    public static Reference<Stat, Stat> CreateStatFromInstance(ReferenceSpec record)
    {
        var instanceSpec = record as InstanceSpec<StatTemplateSpec, StatData>;
        if (instanceSpec is null)
        {
            throw new InvalidOperationException("Instance spec is not a stat");
        }

        return new Reference<Stat, Stat>(
            instanceSpec.Metadata,
            CreateStat(instanceSpec.Metadata, instanceSpec.Instance, null, null),
            null
        );
    }

    public static StatBlock CreateStatBlockFromRecord(IStatBlockSpec record)
    {
        List<Stat> stats = new();
        foreach (ReferenceSpec StatData in record.Stats)
        {
            Reference<Stat, Stat> stat = StatFactory.CreateStatReferenceFromRecord(StatData);
            if (stat.Instance is not null)
            {
                stats.Add(stat.Instance);
            }
        }

        return new StatBlock(stats);
    }

    public static StatBlock CreateStatBlockFromReferences(List<ReferenceSpec> records)
    {
        List<Stat> stats = new();
        foreach (ReferenceSpec StatData in records)
        {
            Reference<Stat, Stat> stat = StatFactory.CreateStatReferenceFromRecord(StatData);
            if (stat.Instance is not null)
            {
                stats.Add(stat.Instance);
            }
        }

        return new StatBlock(stats);
    }

    public static IStatConfigData CopyStatConfig(IStatConfigData record)
    {
        switch (record)
        {
            case ValueStatConfigData:
                var valueConfigRecord = record as ValueStatConfigData;
                if (valueConfigRecord is null)
                {
                    throw new ArgumentException($"Invalid stat config type: {record.GetType()}");
                }

                return new ValueStatConfigData
                {
                    BaseValueCap = valueConfigRecord.BaseValueCap,
                    CurrentValueCap = valueConfigRecord.CurrentValueCap,
                    BaseValueFormula = valueConfigRecord.BaseValueFormula,
                };
            case ResourceStatConfigData:
                var resourceConfigRecord = record as ResourceStatConfigData;
                if (resourceConfigRecord is null)
                {
                    throw new ArgumentException($"Invalid stat config type: {record.GetType()}");
                }

                return new ResourceStatConfigData
                {
                    BaseCapacityCap = resourceConfigRecord.BaseCapacityCap,
                    CurrentCapacityCap = resourceConfigRecord.CurrentCapacityCap,
                    BaseCapacityFormula = resourceConfigRecord.BaseCapacityFormula,
                    StartingCurrentValue = resourceConfigRecord.StartingCurrentValue,
                };
        }

        throw new ArgumentException($"Invalid stat config type: {record.GetType()}");
    }
}
