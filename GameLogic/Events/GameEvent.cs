namespace GameLogic.Events
{
    public abstract class GameEvent<TCategory> : IEvent
    {
        public TCategory Category { get; }

        protected GameEvent(TCategory category)
        {
            Category = category;
        }
    }
}