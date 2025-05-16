namespace GameLogic.Usables.Effects;

using System.Text.Json;

public static class EffectFactory
{
    public static IEffect CreateFromRecord(IEffectRecord record)
    {
        return record.Type switch
        {
            "Status" => CreateStatusEffect(record),
            _ => throw new NotSupportedException($"Effect type {record.Type} is not supported."),
        };
    }

    private static IEffect CreateStatusEffect(IEffectRecord record)
    {
        return record.Subtype switch
        {
            "Burn" => CreateBurn(record),
            "Poison" => CreatePoison(record),
            _ => throw new NotSupportedException(
                $"Effect subtype {record.Subtype} is not supported."
            ),
        };
    }

    private static IEffect CreateBurn(IEffectRecord record)
    {
        var burnRecord = record.Parameters.Deserialize<BurnStatusEffectParametersRecord>()!;
        var burnConfig = new BurnStatusEffectConfig(
            record.Id,
            record.Type,
            record.Subtype,
            record.Variant,
            burnRecord.Duration,
            burnRecord.DamagePerTurn
        );
        return new BurnStatusEffect(burnConfig);
    }

    private static IEffect CreatePoison(IEffectRecord record)
    {
        var poisonConfig = record.Parameters.Deserialize<PoisonStatusEffectConfig>()!;
        return new PoisonStatusEffect(poisonConfig);
    }
}
