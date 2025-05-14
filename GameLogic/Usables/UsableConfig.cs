using GameLogic.Targeting;

namespace GameLobic.Usables;

public record UsableConfig
{
    public required string Id { get; init; }
    public required TargeterConfig Targeter { get; init; }
    public required List<UsableEffectConfig> Effects { get; init; } = new();
}

public record UsableEffectConfig
{
    public required string Id { get; init; }
    public required EEffectType Type { get; init; }
    public string? BaseEffectId { get; init; }
    public Dictionary<string, object>? Parameters { get; init; }
}

public class StatusEffect
{
    public string Subtype { get; init; }
    public string Variant { get; init; }
    public int Duration { get; init; }
    public int DamagePerTurn { get; init; }
}

public class AttackEffect
{
    public string Subtype { get; init; }
    public string Variant { get; init; }
    public int Damage { get; init; }
}

public class HealEffect
{
    public string Subtype { get; init; }
    public string Variant { get; init; }
    public int Value { get; init; }
}

public enum EEffectType
{
    None,
    Attack,
    Heal,
    Buff,
    Debuff,
    Status,
}