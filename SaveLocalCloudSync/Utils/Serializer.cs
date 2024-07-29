using Newtonsoft.Json;
using System.IO;

namespace SaveLocalCloudSync.Utils;

public static class Serializer
{
    private readonly static string _path = "profiles.json";

    public static void SerializeToFile<T>(this T gamePadBinding)
    {
        string json = JsonConvert.SerializeObject(gamePadBinding);
        File.WriteAllText(_path, json);
    }

    public static T? DeserializeFromFile<T>()
    {
        try
        {
            if (!File.Exists(_path))
            {
                return default;
            }

            string json = File.ReadAllText(_path);
            return JsonConvert.DeserializeObject<T>(json);
        }
        catch
        {
            return default;
        }
    }
}
