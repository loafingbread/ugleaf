namespace GameLogic.Events
{
    public abstract class GameEvent<TCategory> : IEvent<TCategory>
        where TCategory : IEventCategory<EventCategoryConfig>
    {
        public TCategory Category { get; }
        string IEventBase.CategoryId => this.Category.Id;

        protected GameEvent(TCategory category)
        {
            Category = category;
        }
    }
}
