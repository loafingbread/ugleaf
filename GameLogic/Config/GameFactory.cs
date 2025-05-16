namespace GameLogic.Config;

// Factory for creating IConfigurable<T> objects from configs
public static class GameFactory
{
    public static T CreateFromConfig<T, TConfig>(TConfig config)
        where T : IConfigurable<TConfig>, new()
    {
        T instance = new();
        instance.ApplyConfig(config);
        return instance;
    }

    public static T CreateFromRecord<T, TRecord>(Func<TRecord, T> createRecord, TRecord record)
    {
        T instance = createRecord(record);
        return instance;
    }
}
