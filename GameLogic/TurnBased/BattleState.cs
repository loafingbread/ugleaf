using System.Collections.Generic;
using System.Transactions;
using GameLogic.Entities;

namespace GameLogic.TurnBased
{
    public enum EPhase
    {
        BattleStart,
        TurnStart,
        PlayerTurn,
        AwaitPlayerSelectCommand,
        AwaitPlayerSelectTargets,
        EnemyTurn,
        EnemySelectCommand,
        EnemySelectTargets,
        ExecuteCommand,
        TurnEnd,
        BattleEnd,
    }

    public class BattleState
    {
        public EPhase Phase { get; private set; }
        public TurnQueue Queue { get; private set; }

        public BattleState(List<Character> players, List<Character> enemies)
        {
            this.Phase = EPhase.BattleStart;
            this.Queue = new TurnQueue(players, enemies);
        }

        public void SetPhase(EPhase phase)
        {
            this.Phase = phase;
        }
    }
}
