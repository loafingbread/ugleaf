namespace GameLogic.Config;

// Associate configs with runtime classes
public interface IConfigurable<TConfig>
{
    void ApplyConfig(TConfig config);
    TConfig GetConfig();
}