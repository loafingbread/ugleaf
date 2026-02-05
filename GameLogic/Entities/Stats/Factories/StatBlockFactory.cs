namespace GameLogic.Entities.Stats.Factories;

using GameLogic.Entities.Stats.StatBlock;

public static class StatBlockFactory
{
    public static StatBlock CreateStatBlockFromData(StatBlockData data)
    {
        return new StatBlock(data);
    }
}