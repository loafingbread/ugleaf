namespace GameLogic.Usables;

using GameLogic.Registry;
using GameLogic.Targeting;
using GameLogic.Usables.Effects;

public static class UsableFactory
{
    public static List<Usable> CreateUsablesFromData(List<UsableData> data)
    {
        return data.Select(CreateUsableFromData).ToList();
    }

    public static Usable CreateUsableFromData(UsableData data)
    {
        return new Usable(data);
    }
}
