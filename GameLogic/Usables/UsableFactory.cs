namespace GameLogic.Usables;

public static class UsableFactory
{
    public static IUsable CreateFromRecord(IUsableRecord record)
    {
        UsableConfig config = new(record);

        return new Usable(config);
    }
}