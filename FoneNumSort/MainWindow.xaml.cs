using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace FoneNumSort
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {
        private string pathData;
        private string pathNew;
        private int counter;
        private string tmpOut;
        private List<string> dirs;
        private int CountUniqueNums = 0;
        private int CountNewNums = 0;
        private List<string> Newlist;
        private bool check;
        public MainWindow()
        {
            counter = 0;
            InitializeComponent();
        }

        private void BtnBase_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog baseData = new FolderBrowserDialog();
            //OpenFileDialog baseData = new OpenFileDialog();
            baseData.ShowDialog();
            PathBaseFile.Content = "Путь до базового файла:\n" + baseData.SelectedPath;
            pathData = baseData.SelectedPath;

            dirs = new List<string>(Directory.GetFiles(pathData));


        }

        private void ClearFilies()
        {
            for (int j = 0; j < dirs.Count; j++)
            {
                List<string> ClearingList = new List<string>(File.ReadAllLines(dirs[j], Encoding.Default));
                if (ClearingList[0][0] == '7')
                {
                    return;
                }

                bool check_1 = false;

                int firstIndex = -1;
                int lastIndex = -1;

                // очистка от всего лишнего
                for (var i = 0; i < ClearingList.Count; i++)
                {
                    if (ClearingList[i].IndexOf("Дата") >= 0 && check_1 == false)
                    {
                        firstIndex = i;
                        check_1 = true;
                        break;
                    }
                }
                for (var i = firstIndex; i < ClearingList.Count; i++)
                {
                    if (ClearingList[i].IndexOf("ИТОГО:") >= 0)
                    {
                        lastIndex = i;
                        break;
                    }
                }
                ClearingList.RemoveRange(lastIndex, (ClearingList.Count - lastIndex));
                ClearingList.RemoveRange(0, firstIndex + 1);

                firstIndex = -1;
                lastIndex = -1;
                check_1 = false;

                // удаление всего кроме номеров
                for (var i = 0; i < ClearingList.Count; i++)
                {
                    ClearingList[i] = ClearingList[i].Remove(0, 20);
                    ClearingList[i] = ClearingList[i].Remove(ClearingList[i].IndexOf(' '));
                }

                ClearingList = ClearingList.Distinct().ToList();
                //Newlist.RemoveAll(x => x[0] != '7');
                for (int i = 0; i < ClearingList.Count; i++)
                {
                    if (!ClearingList[i].Contains('7'))
                    {
                        ClearingList.RemoveAt(i);
                        i = 0;
                    }
                }
                File.WriteAllLines(dirs[j], ClearingList);
            }
        }

        int ForNum(int j)
        {
            for (int k = 0; k < j; k++)
            {
                // создание таблицы из файла K
                List<string> BaseListTmp = new List<string>(File.ReadAllLines(dirs[k], Encoding.Default));
                Chain_method BaseList = new Chain_method(100, 3, true);

                foreach (var VARIABLE in BaseListTmp)
                    BaseList.Add(VARIABLE);
                /// Конец создания

                if (check)
                {
                    check = false;
                    Newlist.Clear();
                    Newlist = new List<string>(File.ReadAllLines(dirs[j], Encoding.Default));
                    CountUniqueNums = Newlist.Count;
                }

                for (int i = 0; i < Newlist.Count; i++)
                    if (BaseList.Search(Newlist[i]).Found)
                    {
                        Newlist.RemoveAt(i);
                        i = 0;
                    }
            }

            return CountUniqueNums;
        }
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            ClearFilies();

            CountUniqueNums = 0;
            CountNewNums = 0;
            Newlist = new List<string>();

            OutText.Text += "Имя файла ----- Количество номеров ----- Количество новых\n";
            for (int j = 0; j < dirs.Count; j++)
            {
                CountUniqueNums = 0;
                CountNewNums = 0;
                check = true;
                //====

                ForNum(j);

                //========Task.Run<int>(() => ForNum(j));
                CountNewNums = Newlist.Count;
                if (j == 0)
                {
                    Newlist.Clear();
                    Newlist = new List<string>(File.ReadAllLines(dirs[j], Encoding.Default));
                    CountUniqueNums = Newlist.Count;
                    CountNewNums = 0;
                }
                
                OutText.Text += dirs[j] + " " + CountUniqueNums + " " + CountNewNums + '\n';
                Newlist.Clear();
            }
            System.Windows.Forms.MessageBox.Show("Готово!");
        }
    }
}
