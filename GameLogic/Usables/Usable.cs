namespace GameLogic.Usables;

using GameLobic.Usables;
using GameLogic.Config;
using GameLogic.Entities;

public class Usable : IUsable, IConfigurable<UsableConfig>
{
    public UsableConfig Config { get; private set; }
    public Usable(UsableConfig config)
    {
        this.Config = config;
    }

    public void ApplyConfig(UsableConfig config)
    {
        this.Config = config;
    }

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