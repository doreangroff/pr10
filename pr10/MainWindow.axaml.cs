using Avalonia.Controls;
using Avalonia.Interactivity;

namespace pr10;

public partial class MainWindow : Window
{
    public MainWindow(string username)
    {
        InitializeComponent();
        Hello.Text += $", {username}";
    }

    private void ProductsBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        MainPanel.Children.Clear();
        GoodsPage productsPage = new GoodsPage();
        MainPanel.Children.Add(productsPage);
    }

    private void ExitBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        AutWin authWindow = new AutWin();
        this.Hide();
        authWindow.Show();
    }
}