namespace GameLogic.Entities.Characters;

using System.Diagnostics.CodeAnalysis;
using GameLogic.Config;

public class Character : IConfigurable<CharacterConfig>
{
    private CharacterConfig _config { get; set; }

    [SetsRequiredMembers]
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
