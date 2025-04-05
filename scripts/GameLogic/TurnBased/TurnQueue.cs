using Godot;
using System;
using System.Collections.Generic;
using GameLogic.Entities;

namespace GameLogic.TurnBased
{
    public partial class TurnQueue : GodotObject
    {
        List<Character> _turnQueue;
        List<Character> _players;
        List<Character> _enemies;

        public TurnQueue(
            List<Character> players,
            List<Character> enemies)
        {
            this._turnQueue = new List<Character>(players);
            this._turnQueue.AddRange(enemies);

            this._players = new List<Character>(players);
            this._enemies = new List<Character>(enemies);
        }

        public Character GetCurrentTurn() {
            return this._turnQueue[0];
        }

        public List<Character> GetAlliesForCharacter(Character character) {
            bool isEnemy = this._enemies.Contains(character);
            if (isEnemy) {
                return new List<Character>(this._enemies);
            }

            return new List<Character>(this._players);
        }

        public List<Character> GetEnemiesForCharacter(Character character) {
            bool isEnemy = this._enemies.Contains(character);
            if (isEnemy) {
                return new List<Character>(this._players);
            }

            return new List<Character>(this._enemies);
        }
    }
}