namespace GameLogic.Entities.Stats;

using GameLogic.Registry;

public static class StatFactory
{
    public static Reference<Stat, Stat> CreateStatReferenceFromRecord<TStat>(
        ReferenceUnionSpec record
    )
    {
        switch(record.Metadata.Kind)
        {
            case EReferenceKind.Ref:
                return CreateStatFromReference<TStat>(record);
            case EReferenceKind.Inline:
                return CreateStatFromInline<TStat>(record);
            case EReferenceKind.Override:
                return CreateStatFromOverride<TStat>(record);
            case EReferenceKind.Instance:
                return CreateStatFromInstance<TStat>(record);
            default:
                throw new NotImplementedException();
        }
    }

    public static Reference<Stat, Stat> CreateStatFromReference<TStat>(
        ReferenceUnionSpec record
    )
    {
        var refSpec = record as RefSpec;
        if (refSpec is null)
        {
            throw new InvalidOperationException("Ref spec is not a stat");
        }

        return new Reference<Stat, Stat>(
            refSpec.Metadata,
            CreateStat<inlineSpec.Template.Type>(
                refSpec.Metadata,
                null, 
                null,
                null
            ),
            null
        );
    }
    // TODO: Create all references afterwards, do not instantiate
    // until they are resolved

    public static TStat CreateStat<TStat>(
        ReferenceUnionMetadata metadata,
        StatRecord? instanceState,
        StatTemplateRecord? templateRecord,
        StatOverrideRecord overrideRecord
    )
    {
        return new TStat(
            metadata,
            instanceState,
            templateRecord,
            overrideRecord
        )
    }

    public static Reference<Stat, Stat> CreateStatFromInline<TStat>(
        ReferenceUnionSpec record
    )
    {
        var inlineSpec = record as InlineSpec<StatTemplateRecord>;
        if (inlineSpec is null)
        {
            throw new InvalidOperationException("Inline spec is not a stat");
        }

        return new Reference<Stat, Stat>(
            inlineSpec.Metadata,
            CreateStat<inlineSpec.Template.Type>(
                inlineSpec.Metadata,
                null, 
                inlineSpec.Template,
                null
            ),
            null
        );
    }

    public static Reference<Stat, Stat> CreateStatFromOverride<TStat>(
        ReferenceUnionSpec record
    )
    {
        var overrideSpec = record as OverrideSpec<StatTemplateRecord, StatOverrideRecord>;
        if (overrideSpec is null)
        {
            throw InvalidOperationException("Override spec is not a stat");
        }

        return new Reference<Stat, Stat>(
            overrideSpec.Metadata,
            new TStat(overrideSpec, null, null, overrideSpec.Override),
            null
        );
        return new Reference<Stat, Stat>(
            overrideSpec.Metadata,
            CreateStat<inlineSpec.Template.Type>(
                inlineSpec.Metadata,
                null, 
                inlineSpec.Template,
                null
            ),
            null
        );
    }

    public static Reference<Stat, Stat> CreateStatFromInstance<TStat>(
        ReferenceUnionSpec record
    )
    {
        var instanceSpec = record as InstanceSpec<StatTemplateRecord, StatRecord>;
        if (instanceSpec is null)
        {
            throw new InvalidOperationException("Instance spec is not a stat");
        }

        return new Reference<Stat, Stat>(
            instanceSpec.Metadata,
            null,
        new TStat(instanceSpec.Metadata, instanceSpec.Instance, null, null)
        );
    }

    public static Stat CreateStatFromRecord(ReferenceUnionSpec record, StatType type)
    {
        switch (type)
        {
            case StatType.Value:
                return new 
        }
    }

    public static Stat CreateStatFromRecord(StatRecord record)
    {
        switch (record.Type)
        {
            case StatType.Value:
                return new ValueStat(record);
            case StatType.Resource:
                return new ResourceStat(record);
            default:
                throw new ArgumentException($"Invalid stat type: {record.Type}");
        }
    }

    public static Stat CreateStat(
        ReferenceUnionSpec record
    )
    {

    }

    public static ValueStat CreateValueStat(
        ReferenceUnionMetadata metadata,
        StatRecord? instanceState,
        StatTemplateRecord? templateRecord,
        StatOverrideRecord? templateOverride
    )
    {
        return new ValueStat(
            metadata,
            instanceState,
            templateRecord,
            templateOverride
        );
    }

    public static ResourceStat CreateResourceStat(
        ReferenceUnionMetadata metadata,
        StatRecord? instanceState,
        StatTemplateRecord? templateRecord,
        StatOverrideRecord? templateOverride
    )
    {
        return new ValueStat(
            metadata,
            instanceState,
            templateRecord,
            templateOverride
        );
    }

    public static StatBlock CreateStatBlockFromRecord(IStatBlockRecord record)
    {
        return new StatBlock(record);
    }

    public static IStatConfigRecord CopyStatConfig(IStatConfigRecord record)
    {
        switch (record)
        {
            case ValueStatConfigRecord:
                var valueConfigRecord = record as ValueStatConfigRecord;
                if (valueConfigRecord is null)
                {
                    throw new ArgumentException($"Invalid stat config type: {record.GetType()}");
                }

                return new ValueStatConfigRecord
                {
                    BaseValueCap = valueConfigRecord.BaseValueCap,
                    CurrentValueCap = valueConfigRecord.CurrentValueCap,
                    BaseValueFormula = valueConfigRecord.BaseValueFormula,
                };
            case ResourceStatConfigRecord:
                var resourceConfigRecord = record as ResourceStatConfigRecord;
                if (resourceConfigRecord is null)
                {
                    throw new ArgumentException($"Invalid stat config type: {record.GetType()}");
                }

                return new ResourceStatConfigRecord
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
