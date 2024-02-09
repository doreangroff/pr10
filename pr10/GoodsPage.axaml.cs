﻿using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using MsBox.Avalonia;
using MsBox.Avalonia.Enums;
using MySqlConnector;
using pr10.Entitites;

namespace pr10;

public partial class GoodsPage : UserControl
{
    private Database _db = new Database();
    private ObservableCollection<Good> _goods = new ObservableCollection<Good>();
    private ObservableCollection<Manufacturer> _manufacturers = new ObservableCollection<Manufacturer>();
    private string _sql = """
                            select article_number, good_name, measurement_name, cost, max_discount, manufacturer_name, supplier_name, category_name, current_discount, quantity_in_stock, description, image from goods
                         join pr10.good_categories gc on gc.category_id = goods.category
                         join pr10.manufacturers m on m.manufacturer_id = goods.manufacturer
                         join pr10.suppliers s on s.supplier_id = goods.supplier
                         join pr10.measurements m2 on m2.measurement_id = goods.measurement
                         """;
    
    public GoodsPage()
    {
        InitializeComponent();
        ShowTable(_sql);
        FillManufacturers();
    }

    private void FillManufacturers()
    {
        _db.OpenConnection();
        string sql = "select manufacturer_name from manufacturers";
        MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var curMan = new Manufacturer()
            {
                ManufacturerName = reader.GetString("manufacturer_name")
            };
            _manufacturers.Add(curMan);

        }
        _db.CloseConnection();
        _manufacturers.Insert(0, new Manufacturer{ManufacturerName = "Все производители"});
        FilterCBox.ItemsSource = _goods;
    }

    private void ShowTable(string sql)
    {
        _db.OpenConnection();
        MySqlCommand command = new MySqlCommand(sql, _db.GetConnection());
        MySqlDataReader reader = command.ExecuteReader();
        while (reader.Read() && reader.HasRows)
        {
            var curGood = new Good()
            {
                ArticleNumber = reader.GetString("article_number"),
                GoodName = reader.GetString("good_name"),
                Measurment = reader.GetString("measurement_name"),
                Cost = reader.GetInt32("cost"),
                MaxDiscount = reader.GetInt32("max_discount"),
                Manufacturer = reader.GetString("manufacturer_name"),
                Supplier = reader.GetString("supplier_name"),
                Category = reader.GetString("category_name"),
                CurrentDiscount = reader.GetInt32("current_discount"),
                QuantityInStock = reader.GetInt32("quantity_in_stock"),
                Description = reader.GetString("description"),
                Image = reader["image"] as byte[]
            };
            _goods.Add(curGood);
        }
        _db.CloseConnection();
        LBoxProducts.ItemsSource = _goods;
    }

    private void AddBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        Panel.Children.Clear();
        AddGood add = new AddGood(null);
        Panel.Children.Add(add);
    }

    private async void DeleteBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        Good selectedGood = LBoxProducts.SelectedItem as Good;
        if (selectedGood != null)
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Предупреждение", "Вы увренеы что хотите удалить?",
                ButtonEnum.YesNo);
            var result = await box.ShowAsync();
            if (result == ButtonResult.Yes)
            {
                _db.OpenConnection();
                string sql = "delete from goods where article_number = " + selectedGood.ArticleNumber;
                MySqlCommand cooman = new MySqlCommand(sql, _db.GetConnection());
                cooman.ExecuteNonQuery();
                _goods.Remove(selectedGood);
                var success = MessageBoxManager.GetMessageBoxStandard("Успех", "Данные удалены", ButtonEnum.Ok);
                var result1 = success.ShowAsync();
            }
            else
            {
                var error = MessageBoxManager.GetMessageBoxStandard("Отмена", "Операция отменена", ButtonEnum.Ok);
                var result1 = error.ShowAsync();
            }
        }
        else
        {
            var box = MessageBoxManager.GetMessageBoxStandard("Ошибка", "Выберите товар для удаления!", ButtonEnum.Ok);
            var result = box.ShowAsync();
        }
    }

    private void SearchTBox_OnTextChanged(object? sender, TextChangedEventArgs e)
    {
        ObservableCollection<Good> search =
            new ObservableCollection<Good>(_goods.Where(x =>
                x.Manufacturer.Contains(SearchTBox.Text, StringComparison.OrdinalIgnoreCase) ||
                x.ArticleNumber.Contains(SearchTBox.Text, StringComparison.OrdinalIgnoreCase) ||
                x.GoodName.Contains(SearchTBox.Text, StringComparison.OrdinalIgnoreCase) ||
                x.Cost.ToString() == SearchTBox.Text ||
                x.MaxDiscount.ToString() == SearchTBox.Text ||
                x.Supplier.Contains(SearchTBox.Text, StringComparison.OrdinalIgnoreCase) ||
                x.Category.Contains(SearchTBox.Text, StringComparison.OrdinalIgnoreCase) ||
                x.CurrentDiscount.ToString() == SearchTBox.Text ||
                x.QuantityInStock.ToString() == SearchTBox.Text ||
                x.Description.Contains(SearchTBox.Text, StringComparison.OrdinalIgnoreCase)
                ));
        LBoxProducts.ItemsSource = search;
    }

    private void FilterCBox_OnSelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        var selectedMan = FilterCBox.SelectedItem as Manufacturer;
        if (selectedMan.ManufacturerName == "Все производители")
        {
            LBoxProducts.ItemsSource = _goods;
        }
        else
        {
            var filter = _goods.Where(x => x.Manufacturer == selectedMan.ManufacturerName).ToList();
            LBoxProducts.ItemsSource = filter;
        }
    }

    private void OrderByBtn_OnClick(object? sender, RoutedEventArgs e)
    {
        if (OrderByCheckBox.IsChecked == true)
        {
            ObservableCollection<Good> sort = new ObservableCollection<Good>(_goods.OrderBy(x => x.Cost).ToList());
            LBoxProducts.ItemsSource = sort;
        }
        else
        {
            ObservableCollection<Good> sort =
                new ObservableCollection<Good>(_goods.OrderByDescending(x => x.Cost).ToList());
            LBoxProducts.ItemsSource = sort;
        }
    }

    private void LBoxProducts_OnDoubleTapped(object? sender, TappedEventArgs e)
    {
        Good selectedGood = LBoxProducts.SelectedItem as Good;
        if (selectedGood != null)
        {
            Panel.Children.Clear();
            AddGood add = new AddGood(selectedGood);
            Panel.Children.Add(add);
        }
    }
}