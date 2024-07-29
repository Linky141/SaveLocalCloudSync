namespace SaveLocalCloudSync.Models;
public class GameModel
{
    public string Name { get; set; }
    public string LocalPath { get; set; }
    public string CloudPath { get; set; }

    public override string? ToString()
    {
        return Name;
    }
}
