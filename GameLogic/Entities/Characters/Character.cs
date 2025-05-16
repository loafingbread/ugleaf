namespace GameLogic.Entities.Characters;

using GameLogic.Config;

public class Character : IConfigurable<ICharacterData>
{
    private ICharacterData _config { get; set; }
    public string Id { get; private set; } = "";
    public string Name { get; private set; } = "";
    public int Health { get; private set; }
    public int Attack { get; private set; }
    public int Defense { get; private set; }

    public Character() { }

    public Character(ICharacterData config)
    {
        this.ApplyConfig(config);
    }

    public void ApplyConfig(ICharacterData config)
    {
        this._config = config;
        this.Id = config.Id;
        this.Name = config.Name;
        this.Health = config.Health;
        this.Attack = config.Attack;
        this.Defense = config.Defense;
    }

    public ICharacterData GetConfig() => this._config;
}
