namespace GameLogic.Usables.Effects;

public interface IEffectResult
{
    IEffectVariant Variant { get; }
    void Apply();
}

/// <summary>Opt-in: this result may have critted.</summary>
public interface ICritCapable
{
    bool DidCrit { get; }
}

/// <summary>Opt-in: this result may have been blocked.</summary>
public interface IBlockable
{
    bool WasBlocked { get; }
}
