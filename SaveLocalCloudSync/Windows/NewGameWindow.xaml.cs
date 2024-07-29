using Microsoft.Win32;
using SaveLocalCloudSync.Models;
using System.IO;
using System.Windows;

namespace SaveLocalCloudSync.Windows;

/// <summary>
/// Interaction logic for NewGameWindow.xaml
/// </summary>
public partial class NewGameWindow : Window
{
    private GameModel _game;
    private bool _save;
    private bool _editMode;
    private string _singleFileName;

    public NewGameWindow()
    {
        InitializeComponent();
        _editMode = false;
        _game = new();
        init();

    }

    public NewGameWindow(GameModel game)
    {
        InitializeComponent();
        _editMode = true;
        _game = game;
        init();
    }

    private void init()
    {
        _save = false;

        if(_editMode)
        {
            textboxName.Text = _game.Name;
            textboxLocalData.Text = _game.LocalPath;
            textboxCloudData.Text = _game.CloudPath;
        }
    }

    private void ControlsToModel()
    {
        _game = new();
        _game.Name = textboxName.Text;
        _game.LocalPath = textboxLocalData.Text;
        _game.CloudPath = textboxCloudData.Text;
    }

    private void buttonSave_Click(object sender, RoutedEventArgs e)
    {
        _save = true;
        ControlsToModel();
        this.Close();
    }

    private void buttonCancel_Click(object sender, RoutedEventArgs e)
    {
        _save = false;
        this.Close();
    }

    public bool Saving() { return _save; }
    public GameModel GameModel() { return _game; }
}
