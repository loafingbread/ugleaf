namespace GameLogic.Targeting;

public static class TargeterConfigFactory
{
    public static ITargeter CreateFromRecord(ITargeterRecord record)
    {
        TargeterConfig config = new(record);
        return new Targeter(config);
    }
}
