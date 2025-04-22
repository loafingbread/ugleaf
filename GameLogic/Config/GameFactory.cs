namespace GameLogic.Config;

// Factory for creating IConfigurable<T> objects from configs
public static class GameFactory
{
    public static T CreateFromConfig<T, TConfig>(TConfig config)
        where T : IConfigurable<TConfig>, new()
    {
        T instance = new T();
        instance.ApplyConfig(config);
        return instance;
    }
}
