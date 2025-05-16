namespace GameLogic.Entities.Skills;

using GameLogic.Config;
using GameLogic.Targeting;
using GameLogic.Usables;

public class Skill : IConfigurable<SkillConfig>
{
    private SkillConfig _config { get; set; }
    public string Id { get; private set; } = "";
    public string Name { get; private set; } = "";

    // public IUsable Usable { get; }
    public ITargeter? Targeter { get; private set; } = null;
    public List<IUsable> Usables { get; private set; } = new();

    public Skill(SkillConfig config)
    {
        this._config = config;
        this.ApplyConfig(config);
    }

    public bool CanTarget() => this.Targeter != null;

    public bool CanUse() => this.Usables.Count > 0;

    public void ApplyConfig(SkillConfig config)
    {
        this._config = config;

        this.Id = config.Id;
        this.Name = config.Name;

        if (config.Targeter != null)
        {
            this.Targeter = new Targeter(config.Targeter);
        }

        foreach (IUsableConfig usableConfig in config.Usables)
        {
            this.Usables.Add(new Usable(usableConfig));
        }
    }

    public SkillConfig GetConfig() => this._config;
}
