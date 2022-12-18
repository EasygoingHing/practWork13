using Libmas;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfApp13
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        int[,] myArray;

        private void BtnDoATask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClassArray.FindToColums(myArray, tbResults);
            }
            catch (Exception)
            {
                MessageBox.Show("Таблица не создана");
            }
        }

        private void Create_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                uint columns = uint.Parse(tbCA.Text),
                        rows = uint.Parse(tbSA.Text),
                         max = uint.Parse(tbMaxValue.Text),
                         min = uint.Parse(tbMinValue.Text);

                Random rnd = new Random();

                myArray = new int[rows, columns];

                for (int i = 0; i < myArray.GetLength(0); i++)
                {
                    for (int j = 0; j < myArray.GetLength(1); j++)
                    {
                        myArray[i, j] = rnd.Next((int)min, (int)max + 1);
                    }
                }

                tbSA.Focus();
                tbResults.Clear();

                dataGrid.ItemsSource = VisualArray.ToDataTable(myArray).DefaultView;
            }
            catch (Exception)
            {
                MessageBox.Show("Введены не все данные или неверный формат данных");
            }
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.DefaultExt = ".txt";
            save.Filter = "Все файлы (*.*) | *.* |Текстовые файлы | *.txt";
            save.FilterIndex = 2;
            save.Title = "Сохранение таблицы";

            if (save.ShowDialog() == true)
            {
                StreamWriter file = new StreamWriter(save.FileName);

                file.WriteLine(myArray.GetLength(0));
                file.WriteLine(myArray.GetLength(1));

                for (int i = 0; i < myArray.GetLength(0); i++)
                {
                    for (int j = 0; j < myArray.GetLength(1); j++)
                    {
                        file.WriteLine(myArray[i, j]);
                    }
                }
                file.Close();
            }
        }

        private void ButtonInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Дана матрица размера M * N. Найти количество ее столбцов, элементы которых\r\nупорядочены по убыванию.\r\nВыполнил Крутолапов Валерий ИСП-31");
        }

        private void ButtonOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.DefaultExt = ".txt";
            open.Filter = "Все файлы (*.*) | *.* |Текстовые файлы | *.txt";
            open.FilterIndex = 2;
            open.Title = "Открытие таблицы";

            if (open.ShowDialog() == true)
            {
                StreamReader file = new StreamReader(open.FileName);

                int row = Convert.ToInt32(file.ReadLine());
                int column = Convert.ToInt32(file.ReadLine());

                myArray = new Int32[row, column];

                for (int i = 0; i < myArray.GetLength(0); i++)
                {
                    for (int j = 0; j < myArray.GetLength(1); j++)
                    {
                        myArray[i, j] = Convert.ToInt32(file.ReadLine());
                    }
                }

                file.Close();

                dataGrid.ItemsSource = VisualArray.ToDataTable(myArray).DefaultView;
            }
        }

        //Объявляем таймер
        DispatcherTimer timer;

        //Событие запуска окна
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //добавляем таймер
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
            timer.IsEnabled = true;
        }

        //Создание события
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (myArray != null)
            {
                textBlockCurrentCell.Text = $"Разм.тб: {myArray.GetLength(0)}x{myArray.GetLength(1)}";
            }
            DataGrid x = (DataGrid)this.FindName("dataGrid");
            
            if (x.SelectedIndex != -1)
            {
                textBlockSizeTab.Text = $"Текущая ячейка: {x.SelectedIndex + 1}";
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = null;
            myArray = null;
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
