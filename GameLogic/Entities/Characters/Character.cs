namespace GameLogic.Entities.Characters;

using System.Diagnostics.CodeAnalysis;
using GameLogic.Config;
using GameLogic.Entities.Stats;

public class CharacterTemplate : IConfigurable<CharacterConfig, CharacterMetadataRecord>
{
    public CharacterMetadataRecord Metadata { get; private set; }
    public CharacterConfig Config { get; private set; }

    public CharacterTemplate(CharacterRecord record)
    {
        this.Metadata = record.Metadata;
        this.Config = record.Config;
    }
}

public class Character : IConfigurable<CharacterConfig>
{
    private CharacterConfig _config { get; set; }
    public StatBlock Stats { get; private set; }

    [SetsRequiredMembers]
    public Character(CharacterConfig config)
    {
        this._config = config;
        this.Stats = StatFactory.CreateStatBlockFromRecord(config);
        this.ApplyConfig(config);
    }

    public void ApplyConfig(CharacterConfig config)
    {
        this._config = config;
    }

    public CharacterConfig GetConfig() => this._config;
}

