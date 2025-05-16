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
    public IUsable? Usable { get; private set; } = null;

    public Skill(SkillConfig config)
    {
        this._config = config;
        this.ApplyConfig(config);
    }

    public bool CanTarget() => this.Targeter != null;

    public bool CanUse() => this.Usable != null;

    public void ApplyConfig(SkillConfig config)
    {
        this._config = config;

        this.Id = config.Id;
        this.Name = config.Name;

        if (config.Targeter != null)
        {
            this.Targeter = new Targeter(config.Targeter);
        }

        if (config.Usable != null)
        {
            this.Usable = new Usable(config.Usable);
        }
    }

    public SkillConfig GetConfig() => this._config;
}
