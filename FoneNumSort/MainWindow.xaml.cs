using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

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
        public MainWindow()
        {
            counter = 0;
            InitializeComponent();
        }

        private void BtnBase_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog baseData = new OpenFileDialog();
            baseData.ShowDialog();
            PathBaseFile.Content = "Путь до базового файла:\n" + baseData.FileName;
            pathData = baseData.FileName;
        }

        private void BtnNew_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog newData = new OpenFileDialog();
            newData.ShowDialog();
            PathNewFile.Content = "Путь до нового файла:\n" + newData.FileName;
            pathNew = newData.FileName;
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {

            List<string> BaseListTmp = new List<string>(System.IO.File.ReadAllLines(pathData, Encoding.Default));
            Chain_method BaseList = new Chain_method(100, 3, true);

            foreach (var VARIABLE in BaseListTmp)
            {
                BaseList.Add(VARIABLE);
            }

            List<string> Newlist = new List<string>(System.IO.File.ReadAllLines(pathNew, Encoding.Default));

            bool check_1 = false;

            int firstIndex = -1;
            int lastIndex = -1;

            // очистка от всего лишнего
            for (var i = 0; i < Newlist.Count; i++)
            {
                if (Newlist[i].IndexOf("Дата") >= 0 && check_1 == false)
                {
                    firstIndex = i;
                    check_1 = true;
                    break;
                }
            }
            for (var i = firstIndex; i < Newlist.Count; i++)
            {
                if (Newlist[i].IndexOf("ИТОГО:") >= 0)
                {
                    lastIndex = i;
                    break;
                }
            }
            Newlist.RemoveRange(lastIndex, (Newlist.Count - lastIndex));
            Newlist.RemoveRange(0, firstIndex + 1);

            firstIndex = -1;
            lastIndex = -1;
            check_1 = false;

            // удаление всего кроме номеров
            for (var i = 0; i < Newlist.Count; i++)
            {
                Newlist[i] = Newlist[i].Remove(0, 20);
                Newlist[i] = Newlist[i].Remove(Newlist[i].IndexOf(' '));
            }



            Newlist = Newlist.Distinct().ToList();
            //Newlist.RemoveAll(x => x[0] != '7');
            for (int i = 0; i < Newlist.Count; i++)
            {
                if (!Newlist[i].Contains('7'))
                {
                    Newlist.RemoveAt(i);
                    i = 0;
                }
            }

            LblCountAll.Content = "Количество уникальных номеров:  " + Newlist.Count;
            tmpOut = Newlist.Count.ToString();
            for (int i = 0; i < Newlist.Count; i++)
            {
                if (BaseList.Search(Newlist[i]).Found)
                {
                    Newlist.RemoveAt(i);
                    i = 0;
                }

            }

            //OutText.Text = string.Join("\n", Newlist.ToArray());

            LblCount.Content = "Количество новых номеров: " + Newlist.Count;

            tmpOut += " " + Newlist.Count.ToString() + '\n';
            OutText.Text += tmpOut;

            //LblDelta.Content = ;
            foreach (var VARIABLE in Newlist)
                BaseList.Add(VARIABLE);

            
            System.IO.File.WriteAllText(pathData, BaseList.Display());
            Newlist.Clear();
            //BaseList.Clear();
        }

    }
}
