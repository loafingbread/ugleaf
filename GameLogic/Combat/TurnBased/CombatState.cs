namespace GameLogic.Combat.TurnBased;

using GameLogic.Entities.Characters;
using GameLogic.Entities.Skills;
using GameLogic.Events;
using GameLogic.Targeting;

public enum EPhase
{
    CombatStart,
    TurnStart,
    PlayerTurn,
    AwaitPlayerSelectCommand,
    AwaitPlayerSelectTargets,
    EnemyTurn,
    EnemySelectCommand,
    EnemySelectTargets,
    ExecuteCommand,
    TurnEnd,
    CombatEnd,
}

public class CombatState
{
    public EPhase Phase { get; private set; }
    public TurnQueue Queue { get; private set; }
    public EventBus EventBus { get; } = new();
    public Skill? SelectedSkill { get; private set; }
    public List<Character> SelectedTargets { get; private set; } = new();

    public CombatState(List<Character> players, List<Character> enemies)
    {
        Phase = EPhase.CombatStart;
        Queue = new TurnQueue(players, enemies);

        foreach (Character c in players) c.Faction = EFaction.Player;
        foreach (Character c in enemies) c.Faction = EFaction.Enemy;
    }

    public void SetPhase(EPhase phase) => Phase = phase;

    public void SelectSkillAndTargets(Skill skill, List<Character> targets)
    {
        SelectedSkill = skill;
        SelectedTargets = targets;
    }

    public void ClearSelection()
    {
        SelectedSkill = null;
        SelectedTargets = new();
    }
}
