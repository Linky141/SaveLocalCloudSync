using SaveLocalCloudSync.Models;
using SaveLocalCloudSync.Utils;
using SaveLocalCloudSync.Windows;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;

namespace SaveLocalCloudSync;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    ObservableCollection<GameModel> games;
    private GameModel _selectedGame;
    ConsoleManager _consoleManager;
    private string _version = "Alpha 1.0";

    public MainWindow()
    {
        InitializeComponent();
        games = Serializer.DeserializeFromFile<ObservableCollection<GameModel>>();
        if(games == null) 
            games = new();
        listboxGames.ItemsSource = games;
        _selectedGame = new();
        _consoleManager = new(listboxConsole);
        windowMainWindow.Title = "Game save local cloud sync manager (" + _version + ")";
    }












    static void DeleteDirectoryContents(string directoryPath)
    {
        try
        {
            if (Directory.Exists(directoryPath))
            {
                DirectoryInfo di = new DirectoryInfo(directoryPath);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }

                foreach (DirectoryInfo subDir in di.GetDirectories())
                {
                    subDir.Delete(true);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    static void CopyDirectory(string sourceDir, string destDir)
    {
        try
        {
            if (!Directory.Exists(sourceDir))
            {
                throw new DirectoryNotFoundException($"Folder źródłowy '{sourceDir}' nie istnieje.");
            }

            Directory.CreateDirectory(destDir);

            foreach (string filePath in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(filePath);
                string destFilePath = Path.Combine(destDir, fileName);
                File.Copy(filePath, destFilePath, true);
            }

            foreach (string subDirPath in Directory.GetDirectories(sourceDir))
            {
                string subDirName = Path.GetFileName(subDirPath);
                string destSubDirPath = Path.Combine(destDir, subDirName);
                CopyDirectory(subDirPath, destSubDirPath);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }



    private void UpdateSelectedGame(GameModel model)
    {
        if (model == null && games.Count > 0)
        {
            labelGame.Content = games[0].Name;
            labelLocal.Content = games[0].LocalPath;
            labelCloud.Content = games[0].CloudPath;
            _selectedGame = games[0];
        }
        else if (model == null)
        {
            labelGame.Content = "-";
            labelLocal.Content = "-";
            labelCloud.Content = "-";
            _selectedGame = null;
        }
        else
        {
            labelGame.Content = model.Name;
            labelLocal.Content = model.LocalPath;
            labelCloud.Content = model.CloudPath;
            _selectedGame = model;
        }
    }

    private void SaveGamesData()
    {
        games.SerializeToFile();
    }

    private void buttonUpload_Click(object sender, RoutedEventArgs e)
    {
        if(_selectedGame != null) {
            DeleteDirectoryContents(_selectedGame.CloudPath);
            CopyDirectory(_selectedGame.LocalPath, _selectedGame.CloudPath);
            _consoleManager.Add("uploaded: "+_selectedGame.Name);
        }
    }

    private void buttonDownload_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedGame != null)
        {
            DeleteDirectoryContents(_selectedGame.LocalPath);
            CopyDirectory(_selectedGame.CloudPath, _selectedGame.LocalPath);
            _consoleManager.Add("downloaded: " + _selectedGame.Name);
        }
    }

    private void buttonAddNewGame_Click(object sender, RoutedEventArgs e)
    {
        NewGameWindow newGameWindow = new NewGameWindow();
        newGameWindow.ShowDialog();
        if (newGameWindow.Saving())
        {
            var model = newGameWindow.GameModel();
            games.Add(model);

            SaveGamesData();
        }
    }

    private void buttonEditGame_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedGame != null)
        {
            NewGameWindow newGameWindow = new NewGameWindow(_selectedGame);
            newGameWindow.ShowDialog();
            if (newGameWindow.Saving())
            {
                var model = newGameWindow.GameModel();
                games[games.IndexOf(_selectedGame)] = model;

                SaveGamesData();
            }
        }
    }

    private void buttonRemoveGame_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedGame != null)
        {
                games.Remove(_selectedGame);

                SaveGamesData();
        }
    }

    private void listboxGames_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        UpdateSelectedGame((GameModel)listboxGames.SelectedItem);
    }
}