namespace GameLogic.Events
{
    public interface IEvent
    {
        public string Name => GetType().Name; // default implementation
        public DateTime Timestamp => DateTime.UtcNow;

        public void Log()
        {
            Console.WriteLine($"[EVENT] {Name} at {Timestamp}");
        }
    }
}

