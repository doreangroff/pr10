using System;
using System.Data;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySqlConnector;
using pr10.Entitites;

namespace pr10;

public partial class AutWin : Window
{
    private Database _db = new Database();
    private int attemptsCount = 0;
    private bool captchaNeed = false;
    public AutWin()
    {
        InitializeComponent();
        CaptchaTBlock.IsVisible = false;
        CaptchaTBox.IsVisible = false;
    }

    private void AuthBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        string login = LoginTBox.Text;
        string password = PasswordTBox.Text;
        
        DataTable table = new DataTable();
        MySqlDataAdapter adapter = new MySqlDataAdapter();
        string sql = "select * from users where login = @login and password = @password";
        MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
        command.Parameters.Add("@login", MySqlDbType.VarChar).Value = login;
        command.Parameters.Add("@password", MySqlDbType.VarChar).Value = password;
        adapter.SelectCommand = command;  
        adapter.Fill(table);
        if (table.Rows.Count > 0)
        {
            string username = table.Rows[0]["fio"].ToString();
            MainWindow mainWindow = new MainWindow(username, false);
            this.Hide();
            mainWindow.Show();
        }
        else
        {
            attemptsCount++;
            if (attemptsCount > 0)
            {
                captchaNeed = true;
                var box = MessageBoxManager.GetMessageBoxStandard("Ошибка","Неверный логин или пароль.", ButtonEnum.Ok);
                var result = box.ShowAsync();
            }
            else
            {
                captchaNeed = false;
                var box = MessageBoxManager.GetMessageBoxStandard("Ошибка","Неверный логин или пароль.", ButtonEnum.Ok);
            }

            if (captchaNeed)
            {
                CaptchaTBlock.Text = CreateCaptcha();
                CaptchaTBlock.TextDecorations = TextDecorations.Strikethrough;
                CaptchaTBlock.IsVisible = true;
                CaptchaTBox.IsVisible = true;
            }
            else
            {
                CaptchaTBlock.IsVisible = false;
                CaptchaTBox.IsVisible = false;
            }
        }
    }
    
    private string CreateCaptcha()
    {
        string allowChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        char[] chars = allowChar.ToCharArray();
        string captcha = "";

        Random random = new Random();
        for (int i = 0; i < 4; i++)
        {
            captcha += chars[random.Next(chars.Length)];
        }

        return captcha;
    }

    private void GuestBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        string username = "Гость";
        MainWindow mainWindow = new MainWindow(username, true);
        this.Hide();
        mainWindow.Show();
    }
}