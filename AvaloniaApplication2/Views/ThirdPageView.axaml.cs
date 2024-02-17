using Avalonia.Controls;
using Avalonia.Interactivity;
using AvaloniaApplication2.Model;
using AvaloniaApplication2.ViewModels;
using MsBox.Avalonia.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;


namespace AvaloniaApplication2.Views
{
    public partial class ThirdPageView : UserControl
    {
        MainWindow mainWindow = new MainWindow(); // для отображения окна выбора файла
        ThirdModel thirdModel = new ThirdModel(); // работа с excel файлом
        SecondModel secondModel = new SecondModel(); // работа с бд
        string[] string_array; // массив для направлений 
        // словарь подгруженных файлов (key:путь к файлу - value:номер строки, в которую подгружено) 
        Dictionary<string, int> file_path = new Dictionary<string, int>();
        int owner; // получение выбранного направления из comboBox


        public ThirdPageView()
        {
            InitializeComponent();
            string_array = new string[] { "ОНЛП", "1191", "соц. значимые" }; // заполнение combobox при открытии окна 
            WrapPanel? wrapPannel = this.FindControl<WrapPanel>("wrapPannel");
            Button? buttonOk = this.FindControl<Button>("buttonOk");
            Button? buttonRead = this.FindControl<Button>("buttonRead");
            TextBox? textBox = this.FindControl<TextBox>("number");
            buttonOk.Click += AddItemsBbutton_Click;
            buttonRead.Click += ReadFiles_Click;

        }

        private void AddItemsBbutton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            wrapPannel.Children.Clear(); // очистка перед отображением количества 
            int countInput = Convert.ToInt32(number.Text?.ToString()); // введенное количество 
            create_elements(countInput); // метод добавление элементов на экран 
            file_path.Clear(); // на случай, если пользователь указал неверное количество и уже подгрузил файлы
        }

        void create_elements(int count)
        {
            if (count > 6)
            {
                StaticClass.ShowMessageBox("Введеное число не должно превышать 6!", "Уведомление", ButtonEnum.Ok);
            }
            else
            {
                for (int i = 0; i < count; ++i)
                {
                    Button button = new Button()
                    {
                        Content = string.Format("Обзор", i),
                        Tag = i,
                        Width = 85,
                        Height = 30,
                        HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                        Margin = new Avalonia.Thickness(5, 0, 0, 0)
                    };

                    Button buttonClear = new Button()
                    {
                        Content = string.Format("Очистить", i),
                        Tag = i,
                        Width = 85,
                        Height = 30,
                        Margin = new Avalonia.Thickness(5, 0, 0, 0),
                        HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                        Background = new Avalonia.Media.SolidColorBrush(Avalonia.Media.Colors.DarkGray)
                    };

                    ComboBox comboBox = new ComboBox()
                    {
                        Name = string.Format("ComboBox{0}", i),
                        Width = 140,
                        Height = 27,
                        ItemsSource = string_array,
                        Margin = new Avalonia.Thickness(0, 0, 5, 0)

                    };
                    ListBox listbox = new ListBox()
                    {
                        Name = string.Format("listbox{0}", i),
                        Width = 450,
                        Height = 80,
                        Margin = new Avalonia.Thickness(2, 2, 2, 2)
                    };

                    wrapPannel.Children.Add(comboBox);
                    wrapPannel.Children.Add(listbox);
                    button.Click += buttonOpenFile_Click; // обработчик события для кнопки 
                    wrapPannel.Children.Add(button);
                    wrapPannel.Children.Add(buttonClear);
                    buttonClear.Click += button_Clear_Click; // обработчик события для кнопки 

                }
            }
        }
     
        void button_Clear_Click(object sender, RoutedEventArgs e)
        {
            int position = Convert.ToInt32(string.Format("{0}", (sender as Button).Tag)); // позиция кнопки и поля 
            var listBox = wrapPannel.Children.OfType<ListBox>().ElementAt(position); // доступ к элементу по позиции 
            listBox.Items.Clear(); // очистка
            foreach (var kvp in file_path.Where(pair => pair.Value == position).ToList())
            {
                file_path.Remove(kvp.Key);
            }

        }

        async void buttonOpenFile_Click(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Select an Excel file",
                AllowMultiple = true,
                Filters = new List<FileDialogFilter>
                {
                   new FileDialogFilter { Name = "Excel Files", Extensions = new List<string> { "xlsx", "xls" } }
                }
            };

           string[] result = await openFileDialog.ShowAsync(mainWindow);
           
           foreach (string filename in result)
           {
               int position = Convert.ToInt32(string.Format("{0}", (sender as Button).Tag)); // позиция кнопки и поля 
               var listBox = wrapPannel.Children.OfType<ListBox>().ElementAt(position); // доступ к элементу по позиции 

               if (file_path.ContainsKey(System.IO.Path.GetFullPath(filename)))
               {
                   StaticClass.ShowMessageBox("Файл с таким именем уже был добавлен!", "Предупреждение", ButtonEnum.Ok);
               }
               else if (file_path.Where(pair => pair.Value == position).Select(pair1 => pair1.Value).Count() >= 2)
               {
                    StaticClass.ShowMessageBox("Вы не можете добавить больше 2 файлов на одно направление!", "Предупреждение", ButtonEnum.Ok);
               }
               else
               {
                   file_path.Add(System.IO.Path.GetFullPath(filename), position); // вставка в словарь 
                   listBox.Items.Add(System.IO.Path.GetFileName(filename)); // добавление в список для отображения
               }
           }
            
        }

        private void ReadFiles_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (file_path.Any())
            {
                foreach (var files in file_path) // перебираем массив с названиями файлов 
                {
                    var comboBox = wrapPannel.Children.OfType<ComboBox>().ElementAt(files.Value); // доступ к элементу по позиции 
                    if (comboBox.SelectedIndex == -1)
                    {
                        StaticClass.ShowMessageBox("Вы не выбрали направление!", "Предупреждение", ButtonEnum.Ok);
                        return;
                    }
                    else
                    {
                        switch (comboBox.SelectedItem.ToString())
                        {
                            case "ОНЛП":
                                owner = 12;
                                break;
                            case "1191":
                                owner = 28;
                                break;
                            case "соц. значимые":
                                owner = 18;
                                break;
                        }

                        thirdModel.WorkWithExcel(files.Key, owner); // считывание excel файла
                    }
                }

                // При клике на кнопку вызываем событие во ViewModel и активируем кнопку
                if (DataContext is ThirdPageViewModel viewModel && StaticClass.datas.Count > 0)
                {
                    secondModel.WriteToDataBase(); // запись в БД
                    StaticClass.ShowMessageBox("Данные успешно записаны в базу данных MS SQL.", "Оповещение", ButtonEnum.OkCancel);
                    viewModel.OnButtonClicked();
                    buttonRead.IsEnabled = false;
                }
            }
            else
            {
                StaticClass.ShowMessageBox("Установите количество и выберите файлы!", "Предупреждение", ButtonEnum.Ok);
            }
        }
    }

}
