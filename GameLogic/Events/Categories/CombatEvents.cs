namespace GameLogic.Events.Categories;

using GameLogic.Entities.Characters;

public abstract class CombatEvent : GameEvent<BuiltInEventCategory>
{
    // ✅ Overrides
    public string Name
    {
        get
        {
            string targets = String.Join(", ", this.Targets.Select(x => x.Name).ToList());
            return $"{GetType().Name}: Phase[{Phase}], Action[{Action}], User[{User}], CurrentTurn[{CurrentTurn}], Targets[{targets}]";
        }
    }

    public Character User { get; }
    public Character CurrentTurn { get; }
    public List<Character> Targets { get; } = [];
    public string Action { get; set; }
    public string Phase { get; }

    public CombatEvent(
        Character user,
        Character currentTurn,
        List<Character> targets,
        string action,
        string phase
    )
        : base(BuiltInEventCategory.Combat)
    {
        this.User = user;
        this.CurrentTurn = currentTurn;
        this.Targets = targets;
        this.Action = action;
        this.Phase = phase;
    }
}

public class CombatPhaseChangeEvent : CombatEvent
{
    public CombatPhaseChangeEvent(
        Character user,
        Character currentTurn,
        List<Character> targets,
        string action,
        string phase
    )
        : base(user, currentTurn, targets, action, phase) { }
}
