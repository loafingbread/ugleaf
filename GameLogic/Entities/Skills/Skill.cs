namespace GameLogic.Entities.Skills;

using System.Diagnostics.CodeAnalysis;
using GameLogic.Config;
using GameLogic.Targeting;
using GameLogic.Usables;

public class Skill : IConfigurable<ISkillData>
{
    private SkillConfig _config { get; set; }
    private ISkillData _record { get; set; }
    public string Id { get; private set; } = "";
    public string Name { get; private set; } = "";

    // public IUsable Usable { get; }
    public ITargeter? Targeter { get; private set; } = null;
    public IUsable? Usable { get; private set; } = null;

    public Skill() { }

    public Skill(ISkillData record)
    {
        this.ApplyConfig(record);
    }

    public bool CanTarget() => this.Targeter != null;

    public bool CanUse() => this.Usable != null;

    public void ApplyConfig(ISkillData record)
    {
        this._config = new SkillConfig(record);
        // TODO: Remove the record dep since we don't need it.
        this._record = record;

        this.Id = this._config.Id;
        this.Name = this._config.Name;

        if (this._config.Targeter != null)
        {
            this.Targeter = new Targeter(this._config.Targeter);
        }

        if (this._config.Usable != null)
        {
            this.Usable = new Usable(this._config.Usable);
        }
    }

    public ISkillData GetConfig() => this._record;

    // TODO: fix this interface
    public SkillConfig GetConfig1() => this._config;
}
