using SaveLocalCloudSync.Enum;
using SaveLocalCloudSync.Models;
using SaveLocalCloudSync.Utils;
using SaveLocalCloudSync.Windows;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace SaveLocalCloudSync;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    ObservableCollection<GameModel> games;
    private GameModel _selectedGame;
    ConsoleManager _consoleManager;
    private string _version = "Beta 2.1";

    public MainWindow()
    {
        InitializeComponent();
        games = Serializer.DeserializeFromFile<ObservableCollection<GameModel>>();
        if (games == null)
            games = new();
        listboxGames.ItemsSource = games;
        _selectedGame = new();
        _consoleManager = new(listboxConsole);
        windowMainWindow.Title = "Game save local cloud sync manager (" + _version + ")";
        progressbarDownload.Minimum = 0;
        progressbarDownload.Maximum = 100;
        progressbarUpload.Minimum = 0;
        progressbarUpload.Maximum = 100;
    }

    private bool DeleteDirectoryContents(string directoryPath, Mode mode)
    {
        try
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                ManageDownloadUploadButtons(mode);
            });

            if (Directory.Exists(directoryPath))
            {
                DirectoryInfo di = new DirectoryInfo(directoryPath);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }

                foreach (DirectoryInfo subDir in di.GetDirectories())
                {
                    DeleteDirectoryContents(subDir.FullName, mode);
                    subDir.Delete();
                }
            }
        }
        catch (Exception ex)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _consoleManager.Add(ex.Message);
            });
            return false;
        }
        return true;
    }

    private bool CopyDirectory(string sourceDir, string destDir, bool firstIteration = true)
    {
        try
        {


            if (!Directory.Exists(sourceDir))
            {
                throw new DirectoryNotFoundException($"Folder źródłowy '{sourceDir}' nie istnieje.");
            }

            Directory.CreateDirectory(destDir);

            string[] files = Directory.GetFiles(sourceDir);
            int totalCount = files.Length + Directory.GetDirectories(sourceDir).Length;
            int copiedCount = 0;

            foreach (string filePath in files)
            {
                string fileName = Path.GetFileName(filePath);
                string destFilePath = Path.Combine(destDir, fileName);
                File.Copy(filePath, destFilePath, true);
                copiedCount++;

                double progress = (double)copiedCount / totalCount * 100;
                if (firstIteration)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                {
                    progressbarDownload.Value = progress;
                    progressbarUpload.Value = progress;
                });
                }
            }

            string[] subDirectories = Directory.GetDirectories(sourceDir);
            foreach (string subDirPath in subDirectories)
            {
                string subDirName = Path.GetFileName(subDirPath);
                string destSubDirPath = Path.Combine(destDir, subDirName);
                CopyDirectory(subDirPath, destSubDirPath, false);
                copiedCount++;

                double progress = (double)copiedCount / totalCount * 100;

                if (firstIteration)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        progressbarDownload.Value = progress;
                        progressbarUpload.Value = progress;
                    });
                }
            }

            if (firstIteration)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    ManageDownloadUploadButtons(Mode.Idle);
                });
            }
        }
        catch (Exception ex)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                _consoleManager.Add(ex.Message);
                ManageDownloadUploadButtons(Mode.Idle);
            });
            return false;
        }
        return true;
    }

    private void UpdateSelectedGame(GameModel model)
    {
        if (model == null && games.Count > 0)
        {
            textboxLabelGameName.Text = games[0].Name;
            textboxLabelGameLocalPath.Text = games[0].LocalPath;
            textboxLabelGameCloudPath.Text = games[0].CloudPath;
            _selectedGame = games[0];
        }
        else if (model == null)
        {
            textboxLabelGameName.Text = "-";
            textboxLabelGameLocalPath.Text = "-";
            textboxLabelGameCloudPath.Text = "-";
            _selectedGame = null;
        }
        else
        {
            textboxLabelGameName.Text = model.Name;
            textboxLabelGameLocalPath.Text = model.LocalPath;
            textboxLabelGameCloudPath.Text = model.CloudPath;
            _selectedGame = model;
        }
    }

    private void SaveGamesData()
    {
        games.SerializeToFile();
    }

    private async void buttonUpload_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedGame != null)
        {
            var deleted = await Task.Run(() => DeleteDirectoryContents(_selectedGame.CloudPath, Mode.Upload));
            var copied = await Task.Run(() => CopyDirectory(_selectedGame.LocalPath, _selectedGame.CloudPath));
            if (copied && deleted)
                _consoleManager.Add("uploaded: " + _selectedGame.Name);
        }
    }

    private async void buttonDownload_Click(object sender, RoutedEventArgs e)
    {
        if (_selectedGame != null)
        {
            var deleted = await Task.Run(() => DeleteDirectoryContents(_selectedGame.LocalPath, Mode.Download));
            var copied = await Task.Run(() => CopyDirectory(_selectedGame.CloudPath, _selectedGame.LocalPath));
            if (copied && deleted)
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

    void ManageDownloadUploadButtons(Mode mode)
    {
        if (mode == Mode.Download)
        {
            LockUnlockButtons(false);
            /*   buttonDownload.Visibility = Visibility.Hidden;*/
            progressbarDownload.Visibility = Visibility.Visible;
            progressbarDownload.Value = 0;
        }
        else if (mode == Mode.Upload)
        {
            LockUnlockButtons(false);
            /* buttonUpload.Visibility = Visibility.Hidden;*/
            progressbarUpload.Visibility = Visibility.Visible;
            progressbarUpload.Value = 0;
        }
        else if (mode == Mode.Idle)
        {
            LockUnlockButtons(true);
            /*   buttonDownload.Visibility = Visibility.Visible;
               buttonUpload.Visibility = Visibility.Visible;*/
            progressbarDownload.Visibility = Visibility.Hidden;
            progressbarUpload.Visibility = Visibility.Hidden;
        }
    }

    void LockUnlockButtons(bool state)
    {
        buttonUpload.IsEnabled = state;
        buttonDownload.IsEnabled = state;
        buttonAddNewGame.IsEnabled = state;
        buttonRemoveGame.IsEnabled = state;
        buttonEditGame.IsEnabled = state;
    }
}