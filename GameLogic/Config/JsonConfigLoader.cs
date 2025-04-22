namespace GameLogic.Config;

// Reusuable config loader utility
public static class JsonConfigLoader
{
    public static T LoadFromFile<T>(string path)
    {
        string json = File.ReadAllText(path);
        return System.Text.Json.JsonSerializer.Deserialize<T>(json)
            ?? throw new InvalidOperationException($"Failed to load {typeof(T).Name} from {path}");
    }

    public static List<T> LoadAllFromFolder<T>(string folderPath)
    {
        List<T> list = new List<T>();

        foreach (string filePath in Directory.GetFiles(folderPath, "*.json"))
        {
            try
            {
                T config = LoadFromFile<T>(filePath);
                list.Add(config);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Failed to load {filePath}: {ex.Message}");
            }
        }

        return list;
    }
}
