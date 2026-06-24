namespace GameLogic.Combat.TurnBased;

using GameLogic.Entities.Characters;
using GameLogic.Entities.Stats;
using GameLogic.Usables;
using GameLogic.Usables.Effects;

public class Combat
{
    public CombatState State { get; }

    public Combat(List<Character> players, List<Character> enemies)
    {
        State = new CombatState(players, enemies);
    }

    public void Play()
    {
        if (State.Phase == EPhase.CombatStart)
            CombatStart();
        else if (State.Phase == EPhase.TurnStart)
            TurnStart();
        else if (State.Phase == EPhase.PlayerTurn)
            PlayerTurn();
        else if (State.Phase == EPhase.EnemyTurn)
            EnemyTurn();
        else if (State.Phase == EPhase.EnemySelectCommand)
            EnemySelectCommand();
        else if (State.Phase == EPhase.EnemySelectTargets)
            EnemySelectTargets();
        else if (State.Phase == EPhase.ExecuteCommand)
            ExecuteCommand();
        else if (State.Phase == EPhase.TurnEnd)
            TurnEnd();
    }

    void CombatStart() => State.SetPhase(EPhase.TurnStart);

    void TurnStart()
    {
        Character current = State.Queue.GetCurrentTurn();
        State.SetPhase(State.Queue.IsCharacterAnEnemy(current) ? EPhase.EnemyTurn : EPhase.PlayerTurn);
    }

    void PlayerTurn() => State.SetPhase(EPhase.AwaitPlayerSelectCommand);

    public void PlayerSelectCommand(GameLogic.Entities.Skills.Skill skill, List<Character> targets)
    {
        State.SelectSkillAndTargets(skill, targets);
        State.SetPhase(EPhase.ExecuteCommand);
    }

    void EnemyTurn() => State.SetPhase(EPhase.EnemySelectCommand);

    void EnemySelectCommand() => State.SetPhase(EPhase.EnemySelectTargets);

    void EnemySelectTargets() => State.SetPhase(EPhase.ExecuteCommand);

    void ExecuteCommand()
    {
        Character user = State.Queue.GetCurrentTurn();

        if (State.SelectedSkill != null && State.SelectedTargets.Count > 0)
        {
            foreach (UsableTemplate usable in State.SelectedSkill.Usables)
            {
                foreach (Character target in State.SelectedTargets)
                {
                    UsableResult usableResult = usable.Use(user, target);
                    foreach (IEffectResult effectResult in usableResult.Results)
                        effectResult.Apply();
                }
            }
        }

        State.ClearSelection();
        State.SetPhase(EPhase.TurnEnd);
    }

    void TurnEnd()
    {
        RemoveDefeatedCharacters();

        if (!State.Queue.HaveMorePlayersAndEnemies())
        {
            State.SetPhase(EPhase.CombatEnd);
            return;
        }

        State.Queue.NextTurn();
        State.SetPhase(EPhase.TurnStart);
    }

    void RemoveDefeatedCharacters()
    {
        List<Character> all = [.. State.Queue.Players, .. State.Queue.Enemies];
        foreach (Character c in all)
        {
            ResourceStat? hp = c.Stats.GetStat("resource_stat_health", StatType.Resource) as ResourceStat;
            if (hp != null && hp.CurrentValue <= 0)
                State.Queue.RemoveCharacter(c);
        }
    }

    public bool IsCombatOver() => State.Phase == EPhase.CombatEnd;
}
