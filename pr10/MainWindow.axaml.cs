using Avalonia.Controls;
using Avalonia.Interactivity;

namespace pr10;

public partial class MainWindow : Window
{
    private bool _isGuest;
    public MainWindow(string username, bool isGuest)
    {
        InitializeComponent();
        _isGuest = isGuest;
        Hello.Text += $", {username}";
    }

    private void ProductsBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        MainPanel.Children.Clear();
        GoodsPage productsPage = new GoodsPage(_isGuest);
        MainPanel.Children.Add(productsPage);
    }

    private void ExitBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        AutWin authWindow = new AutWin();
        this.Hide();
        authWindow.Show();
    }
}