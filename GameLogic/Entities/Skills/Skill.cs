namespace GameLogic.Entities.Skills;

using GameLogic.Config;
using GameLogic.Targeting;
using GameLogic.Usables;

public class Skill : IConfigurable<SkillConfig>
{
    private SkillConfig _config { get; set; }
    public string Id { get; private set; } = "";
    public string Name { get; private set; } = "";
    public ITargeter? Targeter { get; private set; } = null;
    public List<UsableTemplate> Usables { get; private set; } = new();

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
            this.Targeter = new Targeter(config.Targeter);

        this.Usables = config.Usables;
    }

    public SkillConfig GetConfig() => this._config;
}
