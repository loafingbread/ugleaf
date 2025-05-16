namespace GameLogic.Usables;

using GameLogic.Entities;

public class Usable : IUsable
{
    private IUsableConfig _config { get; set; }
    public Usable(IUsableConfig config)
    {
        this._config = config;
        this.ApplyConfig(config);
    }

    public void ApplyConfig(IUsableConfig config)
    {
        this._config = config;
    }

    public IUsableConfig GetConfig() => this._config;

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