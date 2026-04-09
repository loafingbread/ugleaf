namespace GameLogic.Actions.Effects;

using GameLogic.Targeting;

public interface IEffectResult
{
    IEffectVariant Source { get; }
    ITargetable Target { get; }
    void Apply();
}

/// <summary>Opt-in capability: this result may have critted.</summary>
public interface ICritCapable
{
    bool DidCrit { get; }
}

/// <summary>Opt-in capability: this result may have been blocked.</summary>
public interface IBlockable
{
    bool WasBlocked { get; }
}

/// <summary>
/// Opt-in capability: this result contains multiple discrete hits.
/// Animators can iterate Hits to play per-hit effects.
/// </summary>
public interface IMultiHit
{
    IReadOnlyList<HitInfo> Hits { get; }
}

public record HitInfo(float Damage, bool DidCrit);
