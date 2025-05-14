namespace GameLogic.Entities.Skills;

using GameLogic.Config;
using GameLogic.Targeting;
using GameLogic.Usables;


public class Skill : IConfigurable<ISkillData>
{
    public string Id { get; private set; } = "";
    public string Name { get; private set; } = "";

    // public IUsable Usable { get; }
    public ITargeter? Targeter { get; private set; } = null;
    public IUsable? Usable { get; private set; } = null;

    public Skill() { }

    public Skill(ISkillData config)
    {
        this.ApplyConfig(config);
    }

    public void ApplyConfig(ISkillData config)
    {
        this.Id = config.Id;
        this.Name = config.Name;

        this.Targeter = new Targeter(config.Targeter);
        this.Usable = new Usable(config.Usable);
    }
}
