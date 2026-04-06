namespace GameLogic.Actions.Usables.Usable;

using GameLogic.Entities;

public class Usable : IUsable
{
    public UsableTemplate Template { get; private set; }
    public UsableState State { get; private set; }

    public Usable(UsableTemplate template, UsableState state)
    {
        this.Template = template;
        this.State = state;
    }

    public Usable DeepCopy()
    {
        return new Usable(this.Template.DeepCopy(), this.State.DeepCopy());
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
