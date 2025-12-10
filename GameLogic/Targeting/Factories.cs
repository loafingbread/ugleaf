namespace GameLogic.Targeting;

public static class TargetingFactory
{
    public static ITargeter CreateFromRecord(ITargeterData record)
    {
        return new Targeter(record.TargetQuantity, record.AllowedTargets, record.Count);
    }
}
