namespace GameLogic.Targeting;

public static class TargetingFactory
{
    public static ITargeter CreateFromRecord(ITargeterRecord record)
    {
        return new Targeter(record.TargetQuantity, record.AllowedTargets, record.Count);
    }
}
