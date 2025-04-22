using GameLogic.Entities;

namespace GameLogic.Combat.TurnBased
{
    public class TurnQueue
    {
        List<Character> _turnQueue;
        List<Character> _players;
        List<Character> _enemies;

        public TurnQueue(List<Character> players, List<Character> enemies)
        {
            this._turnQueue = [.. players, .. enemies];

            this._players = [.. players];
            this._enemies = [.. enemies];
        }

        public Character GetCurrentTurn()
        {
            return this._turnQueue[0];
        }

        public bool IsCharacterAnEnemy(Character character)
        {
            return this._enemies.Contains(character);
        }

        public List<Character> GetAlliesForCharacter(Character character)
        {
            bool isEnemy = this._enemies.Contains(character);
            if (isEnemy)
            {
                return [.. this._enemies];
            }

            return [.. this._players];
        }

        public List<Character> GetEnemiesForCharacter(Character character)
        {
            bool isEnemy = this._enemies.Contains(character);
            if (isEnemy)
            {
                return [.. this._players];
            }

            return [.. this._enemies];
        }

        public bool HaveMorePlayersAndEnemies()
        {
            return this._players.Count > 0 && this._enemies.Count > 0;
        }

        public void NextTurn()
        {
            Character prevTurn = this._turnQueue[0];
            this._turnQueue.RemoveAt(0);
            this._turnQueue.Append(prevTurn);
        }

        public void RemoveCharacter(Character character)
        {
            this._turnQueue.Remove(character);
            this._enemies.Remove(character);
            this._enemies.Remove(character);
        }
    }
}
