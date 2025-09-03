namespace GameLogic.Utils;

public interface IDeepCopyable<out T>
{
    T DeepCopy();
}

/// <summary>
/// Extends classes that implement IDeepCopyable<T> with methods to
/// deep copy the object and a list of objects.
/// </summary>
public static class DeepCopyExtensions
{

    public static T DeepCopy<T>(T obj)
        where T : IDeepCopyable<T>
    {
        return obj.DeepCopy();
    }

    public static List<T> DeepCopyList<T>(this IEnumerable<T> items)
        where T : IDeepCopyable<T>
    {
        return items.Select(i => i.DeepCopy()).ToList();
    }
}
