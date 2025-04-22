namespace GameLogic.Combat.TurnBased;

using GameLogic.Entities;

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

    public CombatState(List<Character> players, List<Character> enemies)
    {
        this.Phase = EPhase.CombatStart;
        this.Queue = new TurnQueue(players, enemies);
    }

    public void SetPhase(EPhase phase)
    {
        this.Phase = phase;
    }
}
