namespace GameLogic.Config;

// Associate configs with runtime classes
public interface IConfigurable<TConfig, TMetadata>
{
    public TMetadata Metadata { get; }
    public TConfig Config { get; }
}
