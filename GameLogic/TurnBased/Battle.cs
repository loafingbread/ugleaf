using Godot;
using System;
using System.Collections.Generic;
using GameLogic.Entities;

namespace GameLogic.TurnBased
{
    public partial class Battle : GodotObject
    {
        BattleState State;
        public Battle(
            List<Character> players, List<Character> enemies)
        {
            this.State = new BattleState(players, enemies);
        }

        public void Play()
        {
            if (this.State.Phase == EPhase.BattleStart)
            {
                this.BattleStart();
            }
            else if (this.State.Phase == EPhase.TurnStart)
            {
                this.TurnStart();
            }
            else if (this.State.Phase == EPhase.PlayerTurn)
            {
                this.PlayerTurn();
            }
            else if (this.State.Phase == EPhase.EnemyTurn)
            {
                this.EnemyTurn();
            }
            else if (this.State.Phase == EPhase.EnemySelectCommand)
            {
                this.EnemySelectCommand();
            }
            else if (this.State.Phase == EPhase.EnemySelectTargets)
            {
                this.EnemySelectTargets();
            }
            else if (this.State.Phase == EPhase.ExecuteCommand)
            {
                this.ExecuteCommand();
            }
            else if (this.State.Phase == EPhase.TurnEnd)
            {
                this.TurnEnd();
            }

            this.BattleEnd();
        }

        void BattleStart()
        {
            this.State.SetPhase(EPhase.TurnStart);
        }

        void TurnStart()
        {
            Character currentTurn = this.State.Queue.GetCurrentTurn();

            if (this.State.Queue.IsCharacterAnEnemy(currentTurn))
            {
                this.State.SetPhase(EPhase.EnemyTurn);
                return;
            }

            this.State.SetPhase(EPhase.PlayerTurn);
        }

        void PlayerTurn()
        {
            this.State.SetPhase(EPhase.AwaitPlayerSelectCommand);
        }

        public void PlayerSelectCommand()
        {
            this.State.SetPhase(EPhase.AwaitPlayerSelectTargets);
        }

        public void PlayerSelectTargets()
        {
            this.State.SetPhase(EPhase.ExecuteCommand);
        }

        void EnemyTurn()
        {
            this.State.SetPhase(EPhase.EnemySelectCommand);
        }

        void EnemySelectCommand()
        {
            this.State.SetPhase(EPhase.EnemySelectTargets);
        }

        void EnemySelectTargets()
        {
            this.State.SetPhase(EPhase.ExecuteCommand);
        }

        void ExecuteCommand()
        {
            this.State.SetPhase(EPhase.TurnEnd);
        }

        void TurnEnd()
        {
            if (this.State.Queue.HaveMorePlayersAndEnemies())
            {
                this.State.Queue.NextTurn();
                this.State.SetPhase(EPhase.TurnStart);
                return;
            }

            this.State.SetPhase(EPhase.BattleEnd);
        }

        void BattleEnd()
        {

        }
    }
}
