namespace GameLogic.Entities.Characters;

using GameLogic.Config;

public class Character : IConfigurable<ICharacterConfig>
{
    private ICharacterConfig _config { get; set; }
    public Character() { }

    public Character(ICharacterConfig config)
    {
        this.ApplyConfig(config);
    }

    public void ApplyConfig(ICharacterConfig config)
    {
        this._config = config;
    }

    public ICharacterConfig GetConfig() => this._config;
}
