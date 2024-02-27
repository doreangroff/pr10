using System;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySqlConnector;
using pr10.Entitites;

namespace pr10;

public partial class AddGood : UserControl
{
    public delegate void Closing();

    public event Closing OnClosing;
        
    private Database _db = new Database();
    private ObservableCollection<Manufacturer> _manufacturers = new ObservableCollection<Manufacturer>();
    private ObservableCollection<GoodCategory> _categories = new ObservableCollection<GoodCategory>();
    private ObservableCollection<Supplier> _suppliers = new ObservableCollection<Supplier>();
    private ObservableCollection<Measurment> _measurments = new ObservableCollection<Measurment>();
    private Good _good;
    private byte[] _imageBytes;

    private Random rand = new Random();
    public AddGood()
    {
        InitializeComponent();
        string art = GenerateArt();
        ArticleTBox.Text = art.ToString();
        FillCategories();
        FillSuppliers();
        FillMans();
        FillMeasurments();
    }

    private void FillMeasurments()
    {
        _db.OpenConnection();
        string sql = "select * from measurements";
        MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var curMes = new Measurment()
            {
                MeasurmentId = reader.GetInt32("measurement_id"),
                MeasurmentName = reader.GetString("measurement_name")
            };
            _measurments.Add(curMes);
            MesCmb.ItemsSource = _measurments;

        }
        _db.CloseConnection();
    }

    private void FillMans()
    {
        _db.OpenConnection();
        string sql = "select * from manufacturers";
        MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var curMan = new Manufacturer()
            {
                ManufacturerId = reader.GetInt32("manufacturer_id"),
                ManufacturerName = reader.GetString("manufacturer_name")
            };
            _manufacturers.Add(curMan);
            ManCmb.ItemsSource = _manufacturers;

        }
        _db.CloseConnection();
    }

    private void FillSuppliers()
    {
        _db.OpenConnection();
        string sql = "select * from suppliers";
        MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var curSus = new Supplier()
            {
                SipplierId = reader.GetInt32("supplier_id"),
                SupplierName = reader.GetString("supplier_name")
            };
            _suppliers.Add(curSus);
            SupplierCmb.ItemsSource = _suppliers;

        }
        _db.CloseConnection();
    }

    private void FillCategories()
    {
        _db.OpenConnection();
        string sql = "select * from good_categories";
        MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var curCat = new GoodCategory()
            {
                CategoryId = reader.GetInt32("category_id"),
                CategoryName = reader.GetString("category_name")
            };
            _categories.Add(curCat);
            CategoryCmb.ItemsSource = _categories;

        }
        _db.CloseConnection();
    }

    private async void ImageBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open Text File",
            AllowMultiple = false
        });

        if (files.Count >= 1)
        {
            await using var imageStream = await files[0].OpenReadAsync();
            
            _imageBytes = await ConvertStreamToBytesAsync(imageStream); 
        }
    }

    private  void BackBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        Panel.Children.Clear();
        GoodsPage goods = new GoodsPage(false);
        Panel.Children.Add(goods);
    }

    private void SaveBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        try
        {
            _db.OpenConnection();
            string sql = """
                            insert into goods (article_number, good_name, measurement, cost, max_discount, manufacturer, supplier, category, current_discount, quantity_in_stock, description, image)
                         values (@art, @good, @meas, @cost, @max_dis, @man, @sup, @cat, @cur_dis, @amount, @desc, @image)
                         """;
            MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
            command.Parameters.Add("@art", DbType.String);
            command.Parameters["@art"].Value = ArticleTBox.Text;
            command.Parameters.Add("@good", DbType.String);
            command.Parameters["@good"].Value = NameTBox.Text;
            command.Parameters.Add("@meas", DbType.Int32);
            command.Parameters["@meas"].Value = (MesCmb.SelectedItem as pr10.Entitites.Measurment).MeasurmentId;
            command.Parameters.Add("@cost", DbType.Int32);
            command.Parameters["@cost"].Value = CostTBox.Text;
            command.Parameters.Add("@max_dis", DbType.Int32);
            command.Parameters["@max_dis"].Value = MaxDiscTBox.Text;
            command.Parameters.Add("@man", DbType.Int32);
            command.Parameters["@man"].Value = (ManCmb.SelectedItem as pr10.Entitites.Manufacturer).ManufacturerId;
            command.Parameters.Add("@sup", DbType.Int32);
            command.Parameters["@sup"].Value = (SupplierCmb.SelectedItem as pr10.Entitites.Supplier).SipplierId;
            command.Parameters.Add("@cat", DbType.Int32);
            command.Parameters["@cat"].Value = (CategoryCmb.SelectedItem as pr10.Entitites.GoodCategory).CategoryId;
            command.Parameters.Add("@cur_dis", DbType.Int32);
            command.Parameters["@cur_dis"].Value = CurDiscTBox.Text;
            command.Parameters.Add("@amount", DbType.Int32);
            command.Parameters["@amount"].Value = MaxDiscTBox.Text;
            command.Parameters.Add("@desc", DbType.String);
            command.Parameters["@desc"].Value = DescriptionTBox.Text;
            command.Parameters.AddWithValue("@image", _imageBytes);
            command.ExecuteNonQuery();
            var box = MessageBoxManager.GetMessageBoxStandard("Успех", "Данные успешно добавлены", ButtonEnum.Ok,
                Icon.Success);
            var result = box.ShowAsync();
            _db.CloseConnection();
            
            OnClosing.Invoke();
            Panel.Children.Clear();
            GoodsPage goods = new GoodsPage(false);
            Panel.Children.Add(goods);
        }
        catch (Exception exception)
        {
            var error = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Ошибка " + exception, ButtonEnum.Ok, Icon.Error);
            var result = error.ShowAsync();
        }
    }

    private string GenerateArt()
    {
        char firstLet = (char)('A' + rand.Next(0, 26));
        int randNum = rand.Next(100, 999);
        char secondLet = (char)('A' + rand.Next(0, 26));
        int lastNum = rand.Next(0, 9);
        string art = $"{firstLet}{randNum}{secondLet}{lastNum}";
        return art;
    }
    
    private async Task<byte[]> ConvertStreamToBytesAsync(Stream stream)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            await stream.CopyToAsync(memoryStream);
            return memoryStream.ToArray();
        }
    }
}