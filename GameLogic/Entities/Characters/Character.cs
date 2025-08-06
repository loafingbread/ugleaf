namespace GameLogic.Entities.Characters;

using GameLogic.Config;

public class Character : IConfigurable<CharacterConfig>
{
    private CharacterConfig _config { get; set; }

    public Character() { }

    public Character(CharacterConfig config)
    {
        this.ApplyConfig(config);
    }

    public void ApplyConfig(CharacterConfig config)
    {
        this._config = config;
    }

    public CharacterConfig GetConfig() => this._config;
}
