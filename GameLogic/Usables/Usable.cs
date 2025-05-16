namespace GameLogic.Usables;

using GameLogic.Entities;

public class Usable : IUsable
{
    private UsableConfig _config { get; set; }
    public Usable(UsableConfig config)
    {
        this._config = config;
        this.ApplyConfig(config);
    }

    public void ApplyConfig(UsableConfig config)
    {
        this._config = config;
    }

    public UsableConfig GetConfig() => this._config;

    public IEnumerable<UsableResult> Use(Entity user, IEnumerable<Entity> targets)
    {
        // TODO: Implement usable result calculation
        List<UsableResult> results = new();
        foreach (Entity target in targets)
        {
            results.Add(new UsableResult(this, user, target));
        }

        return results;
    }
}