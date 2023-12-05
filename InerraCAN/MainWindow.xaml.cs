
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
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
using System.Windows.Ink;
using System.Windows.Interop;
using System.Buffers.Binary;
using System.Diagnostics;
using System.Xml;

using System.Diagnostics.Eventing.Reader;
using Microsoft.VisualBasic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Threading;
using System.IO.IsolatedStorage;
using System.Data;
using System.IO;
using System.Windows.Controls.Primitives;

using System.Xml.Linq;

using OxyPlot;
using OxyPlot.Series;
using System.Globalization;
using System.Reflection;
using InerraCAN;
using System.Collections;
using System.Net;
using InerraCAN.Properties;

namespace InterraCAN
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {



        public MainWindow()
        {

            InitializeComponent();
            this.Show();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var MyIni = new IniFiles("Settings.ini");
            //WindowSettings.stat
            WindowSettings.Height = Convert.ToDouble(MyIni.ReadINI("Window", "Height"));
            WindowSettings.Width = Convert.ToDouble(MyIni.ReadINI("Window", "Width"));
            WindowSettings.Top = Convert.ToDouble(MyIni.ReadINI("Window", "Top"));
            WindowSettings.Left = Convert.ToDouble(MyIni.ReadINI("Window", "Left"));

            //WindowSettings.WindowState = TryParse(MyIni.ReadINI("Window", "WindowState"));
            //WindowSettings.Visibility = Visibility.Visible;
        }

        //Dictionary<string, Dictionary<int, List<string>>> messages = new Dictionary<string, Dictionary<int, List<string>>>();
        public Dictionary<string, List<List<string>>> _messages;
        Dictionary<int, string> _comboBoxValues = new Dictionary<int, string>();
        List<string> _uniqId;
        List<string> _distinctData;
        List<string> _files0 = new List<string>(); List<string> _files4 = new List<string>();
        List<string> _files1 = new List<string>(); List<string> _files5 = new List<string>();
        List<string> _files2 = new List<string>(); List<string> _files6 = new List<string>();
        List<string> _files3 = new List<string>(); List<string> _files7 = new List<string>();
        List<string> _dataTime = new List<string>();
        List<string> _timings = new List<string>();
        List<int> _greenIndex = new List<int>();
        List<int> _redIndex = new List<int>();
        Dictionary<int, string> _commits = new Dictionary<int, string>();
        //int _byteSelected;
        string _idForCBox;
        string _itemByte0Selected;
        string _itemByte1Selected;
        string _itemByte2Selected;
        string _itemByte3Selected;
        string _itemByte4Selected;
        string _itemByte5Selected;
        string _itemByte6Selected;
        string _itemByte7Selected;
        bool _key = new bool();
        //int _itemsCount;
        List<string> _listForCBox = new List<string>();
        List<string> _list1ForCBox = new List<string>();
        List<string> _list2ForCBox = new List<string>();
        List<string> _list3ForCBox = new List<string>();
        List<string> _list4ForCBox = new List<string>();
        List<string> _list5ForCBox = new List<string>();
        List<string> _list6ForCBox = new List<string>();
        List<string> _list7ForCBox = new List<string>();
        ListBox _markedUniqId = new ListBox();
        OpenFileDialog _currentFile = new OpenFileDialog();
        string _selectedID;
        string _files;
        int _speedCAN;
        public PlotModel ModelByte0 { get; private set; }
        public PlotModel ModelByte01LE { get; private set; }
        public PlotModel ModelByte1 { get; private set; }
        public PlotModel ModelByte23LE { get; private set; }
        public PlotModel ModelByte2 { get; private set; }
        public PlotModel ModelByte45LE { get; private set; }
        public PlotModel ModelByte3 { get; private set; }
        public PlotModel ModelByte67LE { get; private set; }
        public PlotModel ModelByte4 { get; private set; }
        public PlotModel ModelByte10BE { get; private set; }
        public PlotModel ModelByte5 { get; private set; }
        public PlotModel ModelByte32BE { get; private set; }
        public PlotModel ModelByte6 { get; private set; }
        public PlotModel ModelByte54BE { get; private set; }
        public PlotModel ModelByte7 { get; private set; }
        public PlotModel ModelByte76BE { get; private set; }



        public static void DoEvents()
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background,
                                                  new Action(delegate { }));
        }
        public void Btn_ACE(object sender, RoutedEventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            if (_countACE != true)
            {
                _currentFile.FileName = "";
                _files = null;
            }
            else
            {
                _countACE = false;
            }
            if (_currentFile.FileName == "")
            {

                ofd.Filter = "Text files (*.log)|*.log|All files (*.*)|*.*";
                ofd.ShowDialog();
                _currentFile = ofd;
            }







            if (_currentFile.FileName != "")
            {

                TB_List.Visibility = Visibility.Hidden;
                PB_Load.Value = 0;
                Label_ProgressBar_Status.Content = "Обработка файла... (1 из 2)";
                TB_List.Clear();
                string filename = _currentFile.FileName;
                string files = System.IO.File.ReadAllText(filename);
                if (_files != null)
                {
                    int index = files.IndexOf("  ");
                    files = files.Remove(0, index);
                    _files = _files + files;
                }
                else
                {
                    _files = files;
                }
                List<string> words = new List<string>();
                words = _files.Split('\n').ToList();
                var oneWord = words[0];
                words.RemoveRange(0, 2);
                _speedCAN = Convert.ToInt32(string.Concat(oneWord.Where(Char.IsDigit)));
                //Label_Speed_CAN.Content = string.Concat(oneWord.Where(Char.IsDigit));
                Label_Speed_CAN.Content = Convert.ToString(_speedCAN);
                List<string> dataSheets = new List<string>();
                _dataTime.Clear();
                PB_Load.Visibility = Visibility.Visible;
                PB_Load.Value = PB_Load.Value + 20;
                DoEvents();


                int range;
                //убираем тайминг, он не нужен для обработки и построения графика
                dataSheets = words.ToList();

                PB_Load.Value = PB_Load.Value + 20;
                DoEvents();
                for (int i = 0; i < words.Count; i++)
                {
                    if (dataSheets[i].Contains("Pause"))
                    {
                        dataSheets.RemoveAt(i);
                        words.RemoveAt(i);
                        i--;
                    }
                    else
                    {
                        dataSheets[i] = Regex.Replace(dataSheets[i], @"\s+", " ");
                        range = dataSheets[i].IndexOf(' ', 0) + 1;
                        while (range == 1)
                        {
                            dataSheets[i] = dataSheets[i].Remove(0, 1);
                            range = dataSheets[i].IndexOf(' ', 0) + 1;
                        }
                        //dataTime.Add(dataSheets[i].Remove(range));
                        _dataTime.Add(dataSheets[i]);
                        dataSheets[i] = dataSheets[i].Remove(0, range);
                    }


                    //Thread.Sleep(1);
                }
                PB_Load.Value = PB_Load.Value + 40;
                DoEvents();



                //Оставляем нужные данные для гарфика, убирая между ID и сообщением значение
                int foundSpace1;
                int foundSpace2;



                for (int i = 0; i < dataSheets.Count; i++)
                {

                    foundSpace1 = dataSheets[i].IndexOf(' ', 0) + 1;
                    foundSpace2 = dataSheets[i].IndexOf(' ', foundSpace1);
                    if (foundSpace2 == -1 || foundSpace1 == 0)
                    {
                        dataSheets.Remove(dataSheets[i]);
                        continue;
                    }
                    dataSheets[i] = dataSheets[i].Remove(foundSpace1, foundSpace2 - foundSpace1);


                    //Thread.Sleep(1);
                }

                PB_Load.Value = PB_Load.Value + 20;
                DoEvents();
                Thread.Sleep(100);

                Label_Msg_In.Content = dataSheets.Count;
                List<string> distinctData = new List<string>();
                distinctData = dataSheets;
                _distinctData = distinctData;
                //создаем словарь
                Dictionary<string, List<List<string>>> messages = new Dictionary<string, List<List<string>>>();
                //список для уникальных ID
                List<string> uniqId = new List<string>();
                for (int i = 0; i < distinctData.Count; i++)
                {
                    uniqId.Add(Regex.Replace(distinctData[i], @" \S*", ""));
                }
                uniqId = uniqId.Distinct().ToList();
                PB_Load.Value = PB_Load.Value + 15;
                DoEvents();
                Thread.Sleep(100);

                //удаление неизменяемых элементов, если этого хочет пользователь.
                List<string> containData = new List<string>();
                if (CheckBox_Filter.IsChecked == true)
                {
                    for (int i = 0; i < uniqId.Count; i++)
                    {
                        containData = distinctData.FindAll(d => d.Contains(uniqId[i]));
                        containData = containData.Distinct().ToList();
                        if (containData.Count <= 1)
                        {
                            _dataTime.RemoveAll(d => d.Contains(uniqId[i]));
                            distinctData.RemoveAll(d => d.Contains(uniqId[i]));
                            uniqId.RemoveAt(i);
                            i = i - 1;
                        }
                    }
                    Label_Msq_Unique.Content = distinctData.Count;


                }


                PB_Load.Value = PB_Load.Value + 35;
                DoEvents();
                Thread.Sleep(100);

                //заполняем обработанные данные в TextBox
                TB_List.Text = string.Join("\n", distinctData.ToArray());
                TB_List.Visibility = Visibility.Visible;
                List<int> sortUniqId = new List<int>();
                for (int i = 0; i < uniqId.Count; i++)
                {
                    sortUniqId.Add(Convert.ToInt32(uniqId[i].Remove(0, 2), 16));
                    //Convert.ToInt32(massOneByte[i], 16)
                }
                PB_Load.Value = PB_Load.Value + 5;


                sortUniqId = sortUniqId.OrderBy(s => s).ToList();
                if (uniqId[0].Length == 5)
                {
                    uniqId.Clear();
                    for (int i = 0; i < sortUniqId.Count; i++)
                    {
                        uniqId.Add(Convert.ToString(sortUniqId[i], 16));
                        if (uniqId[i].Length != 3)
                        {
                            uniqId[i] = "0" + uniqId[i].ToUpper();
                            uniqId[i] = "0x" + uniqId[i].ToUpper();
                        }
                        else
                        {
                            uniqId[i] = "0x" + uniqId[i].ToUpper();
                        }

                    }
                }
                else
                {
                    uniqId.Clear();
                    for (int i = 0; i < sortUniqId.Count; i++)
                    {
                        uniqId.Add(Convert.ToString(sortUniqId[i], 16));
                        if (uniqId[i].Length != 8)
                        {
                            uniqId[i] = "0" + uniqId[i].ToUpper();
                            uniqId[i] = "0x" + uniqId[i].ToUpper();
                        }
                        else
                        {
                            uniqId[i] = "0x" + uniqId[i].ToUpper();
                        }

                    }
                }


                PB_Load.Value = PB_Load.Value + 10;
                DoEvents();
                Thread.Sleep(100);

                LB_Uniq.ItemsSource = uniqId;

                //лист сообщений
                //List<List<string>> listBytes = new List<List<string>>();
                //лист для одного сообщения
                List<string> listMsg = new List<string>();
                int counter = 0;
                PB_Load.Value = 0;
                Label_ProgressBar_Status.Content = "Заполнение списков... (2 из 2)";
                PB_Load.Maximum = uniqId.Count;
                PB_Load.Value = PB_Load.Value + 1;
                DoEvents();
                for (int i = 0; i < uniqId.Count; i++)
                {
                    List<List<string>> listBytes = new List<List<string>>();

                    containData = distinctData.FindAll(d => d.Contains(uniqId[i]));
                    if (counter < i)
                    {
                        PB_Load.Value = PB_Load.Value + 1;
                        DoEvents();
                        counter++;
                    }
                    for (int j = 0; j < containData.Count; j++)
                    {
                        listMsg = containData[j].Split(' ').ToList();
                        listMsg.RemoveAll(l => l.Contains(" "));
                        listMsg.RemoveAll(l => l.Equals(string.Empty));
                        listMsg.RemoveRange(0, 1);

                        if (listMsg.Count < 8)
                        {
                            for (int x = listMsg.Count; x < 8; x++)
                            {
                                listMsg.Add("00");
                            }
                        }
                        listBytes.Add(listMsg);
                    }

                    messages.Add(uniqId[i], listBytes);
                    //listBytes.Clear();
                }

                PB_Load.Value = PB_Load.Value + 15;
                DoEvents();
                Thread.Sleep(100);
                Label_ProgressBar_Status.Content = "Обработка выполнена.";
                PB_Load.Visibility = Visibility.Hidden;
                _messages = messages;
                _uniqId = uniqId;
                uniqId = null;
                TabControl_Analize.IsEnabled = true;
                Tab_Charts.IsEnabled = true;
                Tab_Charts.IsSelected = true;
                TabItemOneByte.IsSelected = true;
                LB_Uniq.SelectedIndex = 0;
                //для точки остановки
                List<string> uniqId1 = new List<string>();
                List<string> uniqId2 = new List<string>();
                Btn_PRM_CLick(sender, e);
            }

            //else MessageBox.Show("Файл не был выбран.");

        }

        private void Lb_Uniq_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_commits.ContainsKey(LB_Uniq.SelectedIndex) == true)
            {
                MenuItemAddCommit.Visibility = Visibility.Collapsed;
                MenuItemEditCommit.Visibility = Visibility.Visible;
            }
            else
            {
                MenuItemAddCommit.Visibility = Visibility.Visible;
                MenuItemEditCommit.Visibility = Visibility.Collapsed;
            }
            if (_markedUniqId.Items.Count != 0)
            {
                LB_Uniq = _markedUniqId;
            }
            _selectedID = (string)LB_Uniq.SelectedItem;
            //LB_Uniq.ItemsSource = _uniqId;
            //LB_Uniq.SelectedItem = _selectedID;
            List<string> timing = new List<string>();
            List<string> massOneByte = new List<string>();
            List<string> massOneByte1 = new List<string>();
            List<string> massOneByte2 = new List<string>();
            List<string> massOneByte3 = new List<string>();
            List<string> massOneByte4 = new List<string>();
            List<string> massOneByte5 = new List<string>();
            List<string> massOneByte6 = new List<string>();
            List<string> massOneByte7 = new List<string>();
            List<float> massByte0 = new List<float>(); List<int> y0 = new List<int>();
            List<float> massByte1 = new List<float>();
            List<float> massByte2 = new List<float>();
            List<float> massByte3 = new List<float>();
            List<float> massByte4 = new List<float>();
            List<float> massByte5 = new List<float>();
            List<float> massByte6 = new List<float>();
            List<float> massByte7 = new List<float>();
            List<float> massByte01LE = new List<float>();
            List<float> massByte23LE = new List<float>();
            List<float> massByte45LE = new List<float>();
            List<float> massByte67LE = new List<float>();
            List<float> massByte10BE = new List<float>();
            List<float> massByte32BE = new List<float>();
            List<float> massByte54BE = new List<float>();
            List<float> massByte76BE = new List<float>();
            List<string> listBoxData = new List<string>();
            List<string> replaseData0 = new List<string>();
            List<string> distinctData = _distinctData;
            if (_selectedID != null)
            {
                distinctData = _distinctData.FindAll(d => d.Contains(_selectedID));
            }
            string dataPlace;
            //if (LB_Uniq.SelectedItem != null)
            //{
            string identy = string.Empty;
            if (_selectedID != null)
            {

                int range;
                timing.Clear();
                timing = _dataTime.FindAll(t => t.Contains(_selectedID));
                if (CB_FilterOneByte.SelectedIndex == 0)
                {
                    //CB_FilterOneByte.Items.Clear();
                    LabelOneByte.Content = ("без фильтра");

                }
                if (CB_FilterOneByte.SelectedItem == null)
                {
                    LabelOneByte.Content = " ";

                }
                if (CB_FilterOneByte.Items.Count == 0 &&
                    CB_FilterByte1.Items.Count == 0 &&
                    CB_FilterByte2.Items.Count == 0 &&
                    CB_FilterByte3.Items.Count == 0 &&
                    CB_FilterByte4.Items.Count == 0 &&
                    CB_FilterByte5.Items.Count == 0 &&
                    CB_FilterByte6.Items.Count == 0 &&
                    CB_FilterByte7.Items.Count == 0 ||
                    (CB_FilterOneByte.SelectedIndex == -1 || CB_FilterOneByte.SelectedIndex == 0) &&
                    (CB_FilterByte1.SelectedIndex == -1 || CB_FilterByte1.SelectedIndex == 0) &&
                    (CB_FilterByte2.SelectedIndex == -1 || CB_FilterByte2.SelectedIndex == 0) &&
                    (CB_FilterByte3.SelectedIndex == -1 || CB_FilterByte3.SelectedIndex == 0) &&
                    (CB_FilterByte4.SelectedIndex == -1 || CB_FilterByte4.SelectedIndex == 0) &&
                    (CB_FilterByte5.SelectedIndex == -1 || CB_FilterByte5.SelectedIndex == 0) &&
                    (CB_FilterByte6.SelectedIndex == -1 || CB_FilterByte6.SelectedIndex == 0) &&
                    (CB_FilterByte7.SelectedIndex == -1 || CB_FilterByte7.SelectedIndex == 0))




                //CB_FilterOneByte.Items.Count == 0 || CB_FilterOneByte.SelectedIndex == -1 || CB_FilterOneByte.SelectedIndex == 0 ||
                //CB_FilterByte1.Items.Count == 0 || CB_FilterByte1.SelectedIndex == -1 || CB_FilterByte1.SelectedIndex == 0 ||
                //CB_FilterByte2.Items.Count == 0 || CB_FilterByte2.SelectedIndex == -1 || CB_FilterByte2.SelectedIndex == 0 ||
                //CB_FilterByte3.Items.Count == 0 || CB_FilterByte3.SelectedIndex == -1 || CB_FilterByte3.SelectedIndex == 0 ||
                //CB_FilterByte4.Items.Count == 0 || CB_FilterByte4.SelectedIndex == -1 || CB_FilterByte4.SelectedIndex == 0 ||
                //CB_FilterByte5.Items.Count == 0 || CB_FilterByte5.SelectedIndex == -1 || CB_FilterByte5.SelectedIndex == 0 ||
                //CB_FilterByte6.Items.Count == 0 || CB_FilterByte6.SelectedIndex == -1 || CB_FilterByte6.SelectedIndex == 0 ||
                //CB_FilterByte7.Items.Count == 0 || CB_FilterByte7.SelectedIndex == -1 || CB_FilterByte7.SelectedIndex == 0 )
                {
                    for (int i = 0; i < timing.Count; i++)
                    {
                        range = timing[i].IndexOf(" ", 0);
                        timing[i] = timing[i].Remove(range);
                    }
                    var lastitem = timing.Last();
                    int maxLenght = timing.Find(t => t.Contains(lastitem)).Length;


                    for (int i = 0; i < timing.Count; i++)
                    {
                        while (timing[i].Length < maxLenght)
                        {
                            timing[i] = "0" + timing[i];
                        }
                        dataPlace = Regex.Replace(distinctData[i], "\\w{3,}\\s+", "");
                        listBoxData.Add(timing[i] + " " + dataPlace);
                    }
                    for (int i = 0; i < _messages[_selectedID].Count; i++)
                    {
                        y0.Add(i);
                        massOneByte.Add(Convert.ToString(_messages[_selectedID][i][0]));
                        massOneByte1.Add(Convert.ToString(_messages[_selectedID][i][1]));
                        massOneByte2.Add(Convert.ToString(_messages[_selectedID][i][2]));
                        massOneByte3.Add(Convert.ToString(_messages[_selectedID][i][3]));
                        massOneByte4.Add(Convert.ToString(_messages[_selectedID][i][4]));
                        massOneByte5.Add(Convert.ToString(_messages[_selectedID][i][5]));
                        massOneByte6.Add(Convert.ToString(_messages[_selectedID][i][6]));
                        massOneByte7.Add(Convert.ToString(_messages[_selectedID][i][7]));

                        massByte0.Add(Convert.FromHexString(_messages[_selectedID][i][0]).ToList()[0]);
                        massByte1.Add(Convert.FromHexString(_messages[_selectedID][i][1]).ToList()[0]);
                        massByte2.Add(Convert.FromHexString(_messages[_selectedID][i][2]).ToList()[0]);
                        massByte3.Add(Convert.FromHexString(_messages[_selectedID][i][3]).ToList()[0]);
                        massByte4.Add(Convert.FromHexString(_messages[_selectedID][i][4]).ToList()[0]);
                        massByte5.Add(Convert.FromHexString(_messages[_selectedID][i][5]).ToList()[0]);
                        massByte6.Add(Convert.FromHexString(_messages[_selectedID][i][6]).ToList()[0]);
                        massByte7.Add(Convert.FromHexString(_messages[_selectedID][i][7]).ToList()[0]);
                        massByte01LE.Add(Convert.ToInt32(_messages[_selectedID][i][0] + _messages[_selectedID][i][1], 16));
                        massByte23LE.Add(Convert.ToInt32(_messages[_selectedID][i][2] + _messages[_selectedID][i][3], 16));
                        massByte45LE.Add(Convert.ToInt32(_messages[_selectedID][i][4] + _messages[_selectedID][i][5], 16));
                        massByte67LE.Add(Convert.ToInt32(_messages[_selectedID][i][6] + _messages[_selectedID][i][7], 16));
                        massByte10BE.Add(Convert.ToInt32(_messages[_selectedID][i][1] + _messages[_selectedID][i][0], 16));
                        massByte32BE.Add(Convert.ToInt32(_messages[_selectedID][i][3] + _messages[_selectedID][i][2], 16));
                        massByte54BE.Add(Convert.ToInt32(_messages[_selectedID][i][5] + _messages[_selectedID][i][4], 16));
                        massByte76BE.Add(Convert.ToInt32(_messages[_selectedID][i][7] + _messages[_selectedID][i][6], 16));

                    }
                    //for (int i = 0; i < _messages[_selectedID].Count; i++)
                    //{
                    //    massByte01LE.Add(Convert.ToInt32(_messages[_selectedID][i][0] + _messages[_selectedID][i][1], 16));
                    //    massByte23LE.Add(Convert.ToInt32(_messages[_selectedID][i][2] + _messages[_selectedID][i][3], 16));
                    //    massByte45LE.Add(Convert.ToInt32(_messages[_selectedID][i][4] + _messages[_selectedID][i][5], 16));
                    //    massByte67LE.Add(Convert.ToInt32(_messages[_selectedID][i][6] + _messages[_selectedID][i][7], 16));
                    //    massByte10BE.Add(Convert.ToInt32(_messages[_selectedID][i][1] + _messages[_selectedID][i][0], 16));
                    //    massByte32BE.Add(Convert.ToInt32(_messages[_selectedID][i][3] + _messages[_selectedID][i][2], 16));
                    //    massByte54BE.Add(Convert.ToInt32(_messages[_selectedID][i][5] + _messages[_selectedID][i][4], 16));
                    //    massByte76BE.Add(Convert.ToInt32(_messages[_selectedID][i][7] + _messages[_selectedID][i][6], 16));
                    //}
                    //(_messages[_selectedID][i][0].Contains(CB_FilterOneByte.SelectedItem))
                }
                else if ((CB_FilterOneByte.Items.Count != 0 && CB_FilterOneByte.SelectedIndex > 0) ||
                         (CB_FilterByte1.Items.Count != 0 && CB_FilterByte1.SelectedIndex > 0) ||
                         (CB_FilterByte2.Items.Count != 0 && CB_FilterByte2.SelectedIndex > 0) ||
                         (CB_FilterByte3.Items.Count != 0 && CB_FilterByte3.SelectedIndex > 0) ||
                         (CB_FilterByte4.Items.Count != 0 && CB_FilterByte4.SelectedIndex > 0) ||
                         (CB_FilterByte5.Items.Count != 0 && CB_FilterByte5.SelectedIndex > 0) ||
                         (CB_FilterByte6.Items.Count != 0 && CB_FilterByte6.SelectedIndex > 0) ||
                         (CB_FilterByte7.Items.Count != 0 && CB_FilterByte7.SelectedIndex > 0))
                {

                    int count;
                    List<int> keys = _comboBoxValues.Keys.ToList();
                    //for (int i = 0; i < _messages[_selectedID].Count; i++)
                    //{
                    //    count = 0;
                    //    for (int k = 0; k < _comboBoxValues.Count; k++)
                    //    {
                    //        if (_messages[_selectedID][i][keys[k]].Contains(_comboBoxValues[k]))
                    //        {
                    //            count++;
                    //        }
                    //        if (count == _comboBoxValues.Count)
                    //        {
                    //            massByte0.Add(Convert.FromHexString(_messages[_selectedID][i][0]).ToList()[0]);
                    //            massByte1.Add(Convert.FromHexString(_messages[_selectedID][i][1]).ToList()[0]);
                    //            massByte2.Add(Convert.FromHexString(_messages[_selectedID][i][2]).ToList()[0]);
                    //            massByte3.Add(Convert.FromHexString(_messages[_selectedID][i][3]).ToList()[0]);
                    //            massByte4.Add(Convert.FromHexString(_messages[_selectedID][i][4]).ToList()[0]);
                    //            massByte5.Add(Convert.FromHexString(_messages[_selectedID][i][5]).ToList()[0]);
                    //            massByte6.Add(Convert.FromHexString(_messages[_selectedID][i][6]).ToList()[0]);
                    //            massByte7.Add(Convert.FromHexString(_messages[_selectedID][i][7]).ToList()[0]);
                    //        }
                    //    }
                    //}
                    for (int i = 0; i < _messages[_selectedID].Count; i++)
                    {

                        massOneByte.Add(Convert.ToString(_messages[_selectedID][i][0]));
                        massOneByte1.Add(Convert.ToString(_messages[_selectedID][i][1]));
                        massOneByte2.Add(Convert.ToString(_messages[_selectedID][i][2]));
                        massOneByte3.Add(Convert.ToString(_messages[_selectedID][i][3]));
                        massOneByte4.Add(Convert.ToString(_messages[_selectedID][i][4]));
                        massOneByte5.Add(Convert.ToString(_messages[_selectedID][i][5]));
                        massOneByte6.Add(Convert.ToString(_messages[_selectedID][i][6]));
                        massOneByte7.Add(Convert.ToString(_messages[_selectedID][i][7]));


                        //if (CB_FilterOneByte.SelectedItem != null)
                        //{

                        //    if (TabItemOneByte.IsSelected == true && _messages[_selectedID][i][0].Contains(_itemByte0Selected) == true)
                        //    {

                        //        massByte0.Add(Convert.FromHexString(_messages[_selectedID][i][0]).ToList()[0]);
                        //        massByte1.Add(Convert.FromHexString(_messages[_selectedID][i][1]).ToList()[0]);
                        //        massByte2.Add(Convert.FromHexString(_messages[_selectedID][i][2]).ToList()[0]);
                        //        massByte3.Add(Convert.FromHexString(_messages[_selectedID][i][3]).ToList()[0]);
                        //        massByte4.Add(Convert.FromHexString(_messages[_selectedID][i][4]).ToList()[0]);
                        //        massByte5.Add(Convert.FromHexString(_messages[_selectedID][i][5]).ToList()[0]);
                        //        massByte6.Add(Convert.FromHexString(_messages[_selectedID][i][6]).ToList()[0]);
                        //        massByte7.Add(Convert.FromHexString(_messages[_selectedID][i][7]).ToList()[0]);

                        //    }
                        count = 0;
                        for (int k = 0; k < _comboBoxValues.Count; k++)
                        {
                            if (_messages[_selectedID][i][keys[k]].Contains(_comboBoxValues[keys[k]]))
                            {
                                count++;
                            }
                            if (count == _comboBoxValues.Count)
                            {
                                massByte0.Add(Convert.FromHexString(_messages[_selectedID][i][0]).ToList()[0]);
                                massByte1.Add(Convert.FromHexString(_messages[_selectedID][i][1]).ToList()[0]);
                                massByte2.Add(Convert.FromHexString(_messages[_selectedID][i][2]).ToList()[0]);
                                massByte3.Add(Convert.FromHexString(_messages[_selectedID][i][3]).ToList()[0]);
                                massByte4.Add(Convert.FromHexString(_messages[_selectedID][i][4]).ToList()[0]);
                                massByte5.Add(Convert.FromHexString(_messages[_selectedID][i][5]).ToList()[0]);
                                massByte6.Add(Convert.FromHexString(_messages[_selectedID][i][6]).ToList()[0]);
                                massByte7.Add(Convert.FromHexString(_messages[_selectedID][i][7]).ToList()[0]);
                            }
                        }

                        massByte01LE.Add(Convert.ToInt32(_messages[_selectedID][i][0] + _messages[_selectedID][i][1], 16));
                        massByte23LE.Add(Convert.ToInt32(_messages[_selectedID][i][2] + _messages[_selectedID][i][3], 16));
                        massByte45LE.Add(Convert.ToInt32(_messages[_selectedID][i][4] + _messages[_selectedID][i][5], 16));
                        massByte67LE.Add(Convert.ToInt32(_messages[_selectedID][i][6] + _messages[_selectedID][i][7], 16));
                        massByte10BE.Add(Convert.ToInt32(_messages[_selectedID][i][1] + _messages[_selectedID][i][0], 16));
                        massByte32BE.Add(Convert.ToInt32(_messages[_selectedID][i][3] + _messages[_selectedID][i][2], 16));
                        massByte54BE.Add(Convert.ToInt32(_messages[_selectedID][i][5] + _messages[_selectedID][i][4], 16));
                        massByte76BE.Add(Convert.ToInt32(_messages[_selectedID][i][7] + _messages[_selectedID][i][6], 16));
                    }
                    for (int i = 0; i < distinctData.Count; i++)
                    {

                        dataPlace = Regex.Replace(distinctData[i], "\\w{3,}\\s+", "");
                        dataPlace = Regex.Replace(dataPlace, "\\s\\w*", "");
                        replaseData0.Add(dataPlace);

                    }
                    //replaseData0 = replaseData0.FindAll(d => d.Contains(_selectedID));
                    for (int i = 0; i < timing.Count; i++)
                    {
                        range = timing[i].IndexOf(" ", 0);
                        timing[i] = timing[i].Remove(range);
                    }
                    var lastitem = timing.Last();
                    int maxLenght = timing.Find(t => t.Contains(lastitem)).Length;
                    for (int i = 0; i < timing.Count; i++)
                    {
                        if (_comboBoxValues.Count != 0)
                        {
                            if (i == -1)
                            {
                                i = 0;
                            }

                            //if (TabItemOneByte.IsSelected == true && _messages[_selectedID][i][0].Contains(_itemByte0Selected) == true)
                            //{
                            //int lastIndex 
                            count = 0;
                            for (int k = 0; k < _comboBoxValues.Count; k++)
                            {
                                if (replaseData0[i].Contains(_comboBoxValues[keys[k]]))
                                {
                                    count++;
                                }
                                if (count == _comboBoxValues.Count)
                                {
                                    while (timing[i].Length < maxLenght)
                                    {
                                        timing[i] = "0" + timing[i];
                                    }
                                    listBoxData.Add(timing[i] + " " + Regex.Replace(distinctData[i], "\\w{3,}\\s+", ""));
                                }
                            }

                            //if (replaseData0[i].Contains(_comboBoxValues[keys[0]]) == true)
                            //    {
                            //        while (timing[i].Length < maxLenght)
                            //        {
                            //            timing[i] = "0" + timing[i];
                            //        }
                            //    listBoxData.Add(timing[i] + " " +Regex.Replace(distinctData[i], "\\w{3,}\\s+", ""));
                            //    }

                            //}

                            //else 
                            ////if(TabItemOneByte.IsSelected == true && _messages[_selectedID][i][0].Contains(_itemByte0Selected) == false)
                            //{
                            //    timing.RemoveAt(i);
                            //    replaseData0.RemoveAt(i);
                            //    distinctData.RemoveAt(i);
                            //    i = i -1;
                            //}

                            //else if (timing[i].Contains(_itemByte0Selected) == false)
                            //{
                            //    timing.RemoveAt(i);
                            //    i = i - 1;
                            //    if (i == -1)
                            //    {
                            //        i = 0;
                            //    }
                            //}
                        }
                    }
                }
                if (_itemByte0Selected != null)
                {
                    //CB_FilterOneByte.Items.Clear();
                    LabelOneByte.Content = _itemByte0Selected;

                }
                else
                {
                    LabelOneByte.Content = " ";
                    _listForCBox.Clear();
                }
                if (_idForCBox != _selectedID)
                {
                    //CB_FilterOneByte.Items.Clear();
                    LabelOneByte.Content = " ";
                    _listForCBox.Clear();
                }
                //_byteSelected = CB_FilterOneByte.SelectedIndex;
                //if (_listForCBox.Count == 0)
                //{
                LabelOneByte.Content = CB_FilterOneByte.SelectedItem;
                _files0.Clear();
                _files1.Clear();
                _files2.Clear();
                _files3.Clear();
                _files4.Clear();
                _files5.Clear();
                _files6.Clear();
                _files7.Clear();
                List<int> converter = new List<int>();
                List<int> converter1 = new List<int>();
                List<int> converter2 = new List<int>();
                List<int> converter3 = new List<int>();
                List<int> converter4 = new List<int>();
                List<int> converter5 = new List<int>();
                List<int> converter6 = new List<int>();
                List<int> converter7 = new List<int>();

                List<string> strings = new List<string>();

                massOneByte = massOneByte.Distinct().ToList();
                massOneByte1 = massOneByte1.Distinct().ToList();
                massOneByte2 = massOneByte2.Distinct().ToList();
                massOneByte3 = massOneByte3.Distinct().ToList();
                massOneByte4 = massOneByte4.Distinct().ToList();
                massOneByte5 = massOneByte5.Distinct().ToList();
                massOneByte6 = massOneByte6.Distinct().ToList();
                massOneByte7 = massOneByte7.Distinct().ToList();
                _files0 = SortingCBox(massOneByte);
                _files1 = SortingCBox(massOneByte1);
                _files2 = SortingCBox(massOneByte2);
                _files3 = SortingCBox(massOneByte3);
                _files4 = SortingCBox(massOneByte4);
                _files5 = SortingCBox(massOneByte5);
                _files6 = SortingCBox(massOneByte6);
                _files7 = SortingCBox(massOneByte7);

                //for (int i = 0; i < massOneByte.Count; i++)
                //{
                //    converter.Add(Convert.ToInt32(massOneByte[i], 16));
                //}
                //converter = converter.OrderBy(c => c).ToList();
                //for (int i = 0; i < converter.Count; i++)
                //{
                //    strings.Add(Convert.ToString(converter[i], 16));
                //    string x = strings[i].ToString();
                //    if (x.Length == 1)
                //    {
                //        x = ("0" + x);
                //    }

                //    _files0.Add(x.ToUpper());
                //}
                //CB_FilterOneByte.Items.Clear();
                //CB_FilterOneByte.Items.Add("без фильтра");
                //_listForCBox.Add("без фильтра");
                //for (int i = 0; i < _files0.Count; i++)
                //{
                //    CB_FilterOneByte.Items.Add(_files0[i]);
                //    _listForCBox.Add(_files0[i]);
                //}
                _idForCBox = _selectedID;
                //}

                //else
                //{
                //    CB_FilterOneByte.Items.Clear();
                //    for (int i = 0; i < _listForCBox.Count; i++)
                //    {
                //        CB_FilterOneByte.Items.Add(_listForCBox[i]);
                //    }
                //    _idForCBox = _selectedID;
                //}
                CB_FilterOneByte.ItemsSource = _files0;
                CB_FilterByte1.ItemsSource = _files1;
                CB_FilterByte2.ItemsSource = _files2;
                CB_FilterByte3.ItemsSource = _files3;
                CB_FilterByte4.ItemsSource = _files4;
                CB_FilterByte5.ItemsSource = _files5;
                CB_FilterByte6.ItemsSource = _files6;
                CB_FilterByte7.ItemsSource = _files7;
                //////////

                //for (int i = 0; i < massByte0.Count; i++)
                //{

                //    y0.Add(i);
                //    //LineGraphByte0.Plot(massByte0, massByte0.Select(v => Math.Sin(v + i / 10.0)).ToArray());

                //}
                //LineGraphByte0.Plot(y0, massByte0);
                #region chart0
                plotByte0.Visibility = Visibility.Collapsed;
                var lineSeriesByte0 = new LineSeries();
                for (int i = 0; i < massByte0.Count; i++)
                {
                    lineSeriesByte0.Points.Add(new DataPoint(i, massByte0[i]));
                }
                lineSeriesByte0.Color = OxyColors.Blue;
                lineSeriesByte0.StrokeThickness = 0.5;
                this.ModelByte0 = new PlotModel { Title = "0" };
                this.ModelByte0.Series.Add(lineSeriesByte0);
                plotByte0.Visibility = Visibility.Visible;
                //plotByte0.Model.Series[0].TrackerKey.
                //if (plotByte0.Model.Series[0].)
                //{

                //}
                plotByte0.Model = ModelByte0;

                ModelByte0.MouseDown += (s, e) =>
                {
                    if (e.IsShiftDown == true)
                    {
                        if (e.HitTestResult != null)
                        {
                            string item = e.HitTestResult.Item.ToString();
                            int index = item.IndexOf(" ");
                            int x = Convert.ToInt32(item.Remove(index));
                            try
                            {

                                Tab_Msg.IsSelected = true;
                                LB_Messages.SelectedIndex = Convert.ToInt32(x);
                                LB_Messages.ScrollIntoView(LB_Messages.Items[x]);
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }

                    }
                };

                plotByte0.Model.TrackerChanged += (s, e) =>
                {

                    if (e.HitResult != null)
                    {
                        e.HitResult.Text = "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                        e.HitResult.Item = e.HitResult.Item + "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                    }

                };
                #endregion
                #region chart1
                plotByte1.Visibility = Visibility.Collapsed;
                var lineSeriesByte1 = new LineSeries();
                for (int i = 0; i < massByte1.Count; i++)
                {
                    lineSeriesByte1.Points.Add(new DataPoint(i, massByte1[i]));
                }
                lineSeriesByte1.Color = OxyColors.Blue;
                lineSeriesByte1.StrokeThickness = 0.5;
                this.ModelByte1 = new PlotModel { Title = "1" };
                this.ModelByte1.Series.Add(lineSeriesByte1);
                plotByte1.Visibility = Visibility.Visible;

                plotByte1.Model = ModelByte1;
                ModelByte1.MouseDown += (s, e) =>
                {

                    if (e.IsShiftDown == true)
                    {
                        if (e.HitTestResult != null)
                        {
                            string item = e.HitTestResult.Item.ToString();
                            int index = item.IndexOf(" ");
                            int x = Convert.ToInt32(item.Remove(index));
                            try
                            {

                                Tab_Msg.IsSelected = true;
                                LB_Messages.SelectedIndex = Convert.ToInt32(x);
                                LB_Messages.ScrollIntoView(LB_Messages.Items[x]);
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }

                    }
                };

                plotByte1.Model.TrackerChanged += (s, e) =>
                {

                    if (e.HitResult != null)
                    {
                        e.HitResult.Text = "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                        e.HitResult.Item = e.HitResult.Item + "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                    }

                };
                #endregion
                #region chart2
                plotByte2.Visibility = Visibility.Collapsed;
                var lineSeriesByte2 = new LineSeries();
                for (int i = 0; i < massByte2.Count; i++)
                {
                    lineSeriesByte2.Points.Add(new DataPoint(i, massByte2[i]));
                }
                lineSeriesByte2.Color = OxyColors.Blue;
                lineSeriesByte2.StrokeThickness = 0.5;
                this.ModelByte2 = new PlotModel { Title = "2" };
                this.ModelByte2.Series.Add(lineSeriesByte2);
                plotByte2.Visibility = Visibility.Visible;

                plotByte2.Model = ModelByte2;
                ModelByte2.MouseDown += (s, e) =>
                {

                    if (e.IsShiftDown == true)
                    {
                        if (e.HitTestResult != null)
                        {
                            string item = e.HitTestResult.Item.ToString();
                            int index = item.IndexOf(" ");
                            int x = Convert.ToInt32(item.Remove(index));
                            try
                            {

                                Tab_Msg.IsSelected = true;
                                LB_Messages.SelectedIndex = Convert.ToInt32(x);
                                LB_Messages.ScrollIntoView(LB_Messages.Items[x]);
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }

                    }
                };

                plotByte2.Model.TrackerChanged += (s, e) =>
                {

                    if (e.HitResult != null)
                    {
                        e.HitResult.Text = "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                        e.HitResult.Item = e.HitResult.Item + "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                    }

                };
                #endregion
                #region chart3
                plotByte3.Visibility = Visibility.Collapsed;
                var lineSeriesByte3 = new LineSeries();
                for (int i = 0; i < massByte3.Count; i++)
                {
                    lineSeriesByte3.Points.Add(new DataPoint(i, massByte3[i]));
                }
                lineSeriesByte3.Color = OxyColors.Blue;
                lineSeriesByte3.StrokeThickness = 0.5;
                this.ModelByte3 = new PlotModel { Title = "3" };
                this.ModelByte3.Series.Add(lineSeriesByte3);
                plotByte3.Visibility = Visibility.Visible;

                plotByte3.Model = ModelByte3;
                ModelByte3.MouseDown += (s, e) =>
                {

                    if (e.IsShiftDown == true)
                    {
                        if (e.HitTestResult != null)
                        {
                            string item = e.HitTestResult.Item.ToString();
                            int index = item.IndexOf(" ");
                            int x = Convert.ToInt32(item.Remove(index));
                            try
                            {

                                Tab_Msg.IsSelected = true;
                                LB_Messages.SelectedIndex = Convert.ToInt32(x);
                                LB_Messages.ScrollIntoView(LB_Messages.Items[x]);
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }

                    }
                };

                plotByte3.Model.TrackerChanged += (s, e) =>
                {

                    if (e.HitResult != null)
                    {
                        e.HitResult.Text = "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                        e.HitResult.Item = e.HitResult.Item + "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                    }

                };
                #endregion
                #region chart4
                plotByte4.Visibility = Visibility.Collapsed;
                var lineSeriesByte4 = new LineSeries();
                for (int i = 0; i < massByte4.Count; i++)
                {
                    lineSeriesByte4.Points.Add(new DataPoint(i, massByte4[i]));
                }
                lineSeriesByte4.Color = OxyColors.Blue;
                lineSeriesByte4.StrokeThickness = 0.5;
                this.ModelByte4 = new PlotModel { Title = "4" };
                this.ModelByte4.Series.Add(lineSeriesByte4);
                plotByte4.Visibility = Visibility.Visible;

                plotByte4.Model = ModelByte4;
                ModelByte4.MouseDown += (s, e) =>
                {

                    if (e.IsShiftDown == true)
                    {
                        if (e.HitTestResult != null)
                        {
                            string item = e.HitTestResult.Item.ToString();
                            int index = item.IndexOf(" ");
                            int x = Convert.ToInt32(item.Remove(index));
                            try
                            {

                                Tab_Msg.IsSelected = true;
                                LB_Messages.SelectedIndex = Convert.ToInt32(x);
                                LB_Messages.ScrollIntoView(LB_Messages.Items[x]);
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }

                    }
                };

                plotByte4.Model.TrackerChanged += (s, e) =>
                {

                    if (e.HitResult != null)
                    {
                        e.HitResult.Text = "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                        e.HitResult.Item = e.HitResult.Item + "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                    }

                };
                #endregion
                #region chart5
                plotByte5.Visibility = Visibility.Collapsed;
                var lineSeriesByte5 = new LineSeries();
                for (int i = 0; i < massByte5.Count; i++)
                {
                    lineSeriesByte5.Points.Add(new DataPoint(i, massByte5[i]));
                }
                lineSeriesByte5.Color = OxyColors.Blue;
                lineSeriesByte5.StrokeThickness = 0.5;
                this.ModelByte5 = new PlotModel { Title = "5" };
                this.ModelByte5.Series.Add(lineSeriesByte5);
                plotByte5.Visibility = Visibility.Visible;

                plotByte5.Model = ModelByte5;
                ModelByte5.MouseDown += (s, e) =>
                {

                    if (e.IsShiftDown == true)
                    {
                        if (e.HitTestResult != null)
                        {
                            string item = e.HitTestResult.Item.ToString();
                            int index = item.IndexOf(" ");
                            int x = Convert.ToInt32(item.Remove(index));
                            try
                            {

                                Tab_Msg.IsSelected = true;
                                LB_Messages.SelectedIndex = Convert.ToInt32(x);
                                LB_Messages.ScrollIntoView(LB_Messages.Items[x]);
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }

                    }
                };

                plotByte5.Model.TrackerChanged += (s, e) =>
                {

                    if (e.HitResult != null)
                    {
                        e.HitResult.Text = "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                        e.HitResult.Item = e.HitResult.Item + "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                    }

                };
                #endregion
                #region chart6
                plotByte6.Visibility = Visibility.Collapsed;
                var lineSeriesByte6 = new LineSeries();
                for (int i = 0; i < massByte6.Count; i++)
                {
                    lineSeriesByte6.Points.Add(new DataPoint(i, massByte6[i]));
                }
                lineSeriesByte6.Color = OxyColors.Blue;
                lineSeriesByte6.StrokeThickness = 0.5;
                this.ModelByte6 = new PlotModel { Title = "6" };
                this.ModelByte6.Series.Add(lineSeriesByte6);
                plotByte6.Visibility = Visibility.Visible;

                plotByte6.Model = ModelByte6;
                ModelByte6.MouseDown += (s, e) =>
                {

                    if (e.IsShiftDown == true)
                    {
                        if (e.HitTestResult != null)
                        {
                            string item = e.HitTestResult.Item.ToString();
                            int index = item.IndexOf(" ");
                            int x = Convert.ToInt32(item.Remove(index));
                            try
                            {

                                Tab_Msg.IsSelected = true;
                                LB_Messages.SelectedIndex = Convert.ToInt32(x);
                                LB_Messages.ScrollIntoView(LB_Messages.Items[x]);
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }

                    }
                };

                plotByte6.Model.TrackerChanged += (s, e) =>
                {

                    if (e.HitResult != null)
                    {
                        e.HitResult.Text = "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                        e.HitResult.Item = e.HitResult.Item + "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                    }

                };
                #endregion
                #region chart7
                plotByte7.Visibility = Visibility.Collapsed;
                var lineSeriesByte7 = new LineSeries();
                for (int i = 0; i < massByte7.Count; i++)
                {
                    lineSeriesByte7.Points.Add(new DataPoint(i, massByte7[i]));
                }
                lineSeriesByte7.Color = OxyColors.Blue;
                lineSeriesByte7.StrokeThickness = 0.5;
                this.ModelByte7 = new PlotModel { Title = "7" };
                this.ModelByte7.Series.Add(lineSeriesByte7);
                plotByte7.Visibility = Visibility.Visible;

                plotByte7.Model = ModelByte7;
                ModelByte7.MouseDown += (s, e) =>
                {

                    if (e.IsShiftDown == true)
                    {
                        if (e.HitTestResult != null)
                        {
                            string item = e.HitTestResult.Item.ToString();
                            int index = item.IndexOf(" ");
                            int x = Convert.ToInt32(item.Remove(index));
                            try
                            {

                                Tab_Msg.IsSelected = true;
                                LB_Messages.SelectedIndex = Convert.ToInt32(x);
                                LB_Messages.ScrollIntoView(LB_Messages.Items[x]);
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }

                    }
                };

                plotByte7.Model.TrackerChanged += (s, e) =>
                {

                    if (e.HitResult != null)
                    {
                        e.HitResult.Text = "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                        e.HitResult.Item = e.HitResult.Item + "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                    }

                };
                #endregion
                #region chart01LE
                plotByte01LE.Visibility = Visibility.Collapsed;
                var lineSeriesByte01LE = new LineSeries();
                for (int i = 0; i < massByte01LE.Count; i++)
                {
                    lineSeriesByte01LE.Points.Add(new DataPoint(i, massByte01LE[i]));
                }
                lineSeriesByte01LE.Color = OxyColors.Blue;
                lineSeriesByte01LE.StrokeThickness = 0.5;
                this.ModelByte01LE = new PlotModel { Title = "0+1" };
                this.ModelByte01LE.Series.Add(lineSeriesByte01LE);
                plotByte01LE.Visibility = Visibility.Visible;

                plotByte01LE.Model = ModelByte01LE;
                ModelByte01LE.MouseDown += (s, e) =>
                {

                    if (e.IsShiftDown == true)
                    {
                        if (e.HitTestResult != null)
                        {
                            string item = e.HitTestResult.Item.ToString();
                            int index = item.IndexOf(" ");
                            int x = Convert.ToInt32(item.Remove(index));
                            try
                            {

                                Tab_Msg.IsSelected = true;
                                LB_Messages.SelectedIndex = Convert.ToInt32(x);
                                LB_Messages.ScrollIntoView(LB_Messages.Items[x]);
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }

                    }
                };

                plotByte01LE.Model.TrackerChanged += (s, e) =>
                {

                    if (e.HitResult != null)
                    {
                        e.HitResult.Text = "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                        e.HitResult.Item = e.HitResult.Item + "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                    }

                };
                #endregion
                #region chart23LE
                plotByte23LE.Visibility = Visibility.Collapsed;
                var lineSeriesByte23LE = new LineSeries();
                for (int i = 0; i < massByte23LE.Count; i++)
                {
                    lineSeriesByte23LE.Points.Add(new DataPoint(i, massByte23LE[i]));
                }
                lineSeriesByte23LE.Color = OxyColors.Blue;
                lineSeriesByte23LE.StrokeThickness = 0.5;
                this.ModelByte23LE = new PlotModel { Title = "2+3" };
                this.ModelByte23LE.Series.Add(lineSeriesByte23LE);
                plotByte23LE.Visibility = Visibility.Visible;

                plotByte23LE.Model = ModelByte23LE;
                ModelByte23LE.MouseDown += (s, e) =>
                {

                    if (e.IsShiftDown == true)
                    {
                        if (e.HitTestResult != null)
                        {
                            string item = e.HitTestResult.Item.ToString();
                            int index = item.IndexOf(" ");
                            int x = Convert.ToInt32(item.Remove(index));
                            try
                            {

                                Tab_Msg.IsSelected = true;
                                LB_Messages.SelectedIndex = Convert.ToInt32(x);
                                LB_Messages.ScrollIntoView(LB_Messages.Items[x]);
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }

                    }
                };

                plotByte23LE.Model.TrackerChanged += (s, e) =>
                {

                    if (e.HitResult != null)
                    {
                        e.HitResult.Text = "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                        e.HitResult.Item = e.HitResult.Item + "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                    }

                };
                #endregion
                #region chart45LE
                plotByte45LE.Visibility = Visibility.Collapsed;
                var lineSeriesByte45LE = new LineSeries();
                for (int i = 0; i < massByte45LE.Count; i++)
                {
                    lineSeriesByte45LE.Points.Add(new DataPoint(i, massByte45LE[i]));
                }
                lineSeriesByte45LE.Color = OxyColors.Blue;
                lineSeriesByte45LE.StrokeThickness = 0.5;
                this.ModelByte45LE = new PlotModel { Title = "4+5" };
                this.ModelByte45LE.Series.Add(lineSeriesByte45LE);
                plotByte45LE.Visibility = Visibility.Visible;

                plotByte45LE.Model = ModelByte45LE;
                ModelByte45LE.MouseDown += (s, e) =>
                {

                    if (e.IsShiftDown == true)
                    {
                        if (e.HitTestResult != null)
                        {
                            string item = e.HitTestResult.Item.ToString();
                            int index = item.IndexOf(" ");
                            int x = Convert.ToInt32(item.Remove(index));
                            try
                            {

                                Tab_Msg.IsSelected = true;
                                LB_Messages.SelectedIndex = Convert.ToInt32(x);
                                LB_Messages.ScrollIntoView(LB_Messages.Items[x]);
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }

                    }
                };

                plotByte45LE.Model.TrackerChanged += (s, e) =>
                {

                    if (e.HitResult != null)
                    {
                        e.HitResult.Text = "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                        e.HitResult.Item = e.HitResult.Item + "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                    }

                };
                #endregion
                #region chart67LE
                plotByte67LE.Visibility = Visibility.Collapsed;
                var lineSeriesByte67LE = new LineSeries();
                for (int i = 0; i < massByte67LE.Count; i++)
                {
                    lineSeriesByte67LE.Points.Add(new DataPoint(i, massByte67LE[i]));
                }
                lineSeriesByte67LE.Color = OxyColors.Blue;
                lineSeriesByte67LE.StrokeThickness = 0.5;
                this.ModelByte67LE = new PlotModel { Title = "6+7" };
                this.ModelByte67LE.Series.Add(lineSeriesByte67LE);
                plotByte67LE.Visibility = Visibility.Visible;

                plotByte67LE.Model = ModelByte67LE;
                ModelByte67LE.MouseDown += (s, e) =>
                {

                    if (e.IsShiftDown == true)
                    {
                        if (e.HitTestResult != null)
                        {
                            string item = e.HitTestResult.Item.ToString();
                            int index = item.IndexOf(" ");
                            int x = Convert.ToInt32(item.Remove(index));
                            try
                            {

                                Tab_Msg.IsSelected = true;
                                LB_Messages.SelectedIndex = Convert.ToInt32(x);
                                LB_Messages.ScrollIntoView(LB_Messages.Items[x]);
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }

                    }
                };

                plotByte67LE.Model.TrackerChanged += (s, e) =>
                {

                    if (e.HitResult != null)
                    {
                        e.HitResult.Text = "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                        e.HitResult.Item = e.HitResult.Item + "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                    }

                };
                #endregion
                #region chart10BE
                plotByte10BE.Visibility = Visibility.Collapsed;
                var lineSeriesByte10BE = new LineSeries();
                for (int i = 0; i < massByte10BE.Count; i++)
                {
                    lineSeriesByte10BE.Points.Add(new DataPoint(i, massByte10BE[i]));
                }
                lineSeriesByte10BE.Color = OxyColors.Blue;
                lineSeriesByte10BE.StrokeThickness = 0.5;
                this.ModelByte10BE = new PlotModel { Title = "1+0" };
                this.ModelByte10BE.Series.Add(lineSeriesByte10BE);
                plotByte10BE.Visibility = Visibility.Visible;

                plotByte10BE.Model = ModelByte10BE;
                ModelByte10BE.MouseDown += (s, e) =>
                {

                    if (e.IsShiftDown == true)
                    {
                        if (e.HitTestResult != null)
                        {
                            string item = e.HitTestResult.Item.ToString();
                            int index = item.IndexOf(" ");
                            int x = Convert.ToInt32(item.Remove(index));
                            try
                            {

                                Tab_Msg.IsSelected = true;
                                LB_Messages.SelectedIndex = Convert.ToInt32(x);
                                LB_Messages.ScrollIntoView(LB_Messages.Items[x]);
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }

                    }
                };

                plotByte10BE.Model.TrackerChanged += (s, e) =>
                {

                    if (e.HitResult != null)
                    {
                        e.HitResult.Text = "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                        e.HitResult.Item = e.HitResult.Item + "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                    }

                };
                #endregion
                #region chart32BE
                plotByte32BE.Visibility = Visibility.Collapsed;
                var lineSeriesByte32BE = new LineSeries();
                for (int i = 0; i < massByte32BE.Count; i++)
                {
                    lineSeriesByte32BE.Points.Add(new DataPoint(i, massByte32BE[i]));
                }
                lineSeriesByte32BE.Color = OxyColors.Blue;
                lineSeriesByte32BE.StrokeThickness = 0.5;
                this.ModelByte32BE = new PlotModel { Title = "3+2" };
                this.ModelByte32BE.Series.Add(lineSeriesByte32BE);
                plotByte32BE.Visibility = Visibility.Visible;

                plotByte32BE.Model = ModelByte32BE;
                ModelByte32BE.MouseDown += (s, e) =>
                {

                    if (e.IsShiftDown == true)
                    {
                        if (e.HitTestResult != null)
                        {
                            string item = e.HitTestResult.Item.ToString();
                            int index = item.IndexOf(" ");
                            int x = Convert.ToInt32(item.Remove(index));
                            try
                            {

                                Tab_Msg.IsSelected = true;
                                LB_Messages.SelectedIndex = Convert.ToInt32(x);
                                LB_Messages.ScrollIntoView(LB_Messages.Items[x]);
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }

                    }
                };

                plotByte32BE.Model.TrackerChanged += (s, e) =>
                {

                    if (e.HitResult != null)
                    {
                        e.HitResult.Text = "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                        e.HitResult.Item = e.HitResult.Item + "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                    }

                };
                #endregion
                #region chart54BE
                plotByte54BE.Visibility = Visibility.Collapsed;
                var lineSeriesByte54BE = new LineSeries();
                for (int i = 0; i < massByte54BE.Count; i++)
                {
                    lineSeriesByte54BE.Points.Add(new DataPoint(i, massByte54BE[i]));
                }
                lineSeriesByte54BE.Color = OxyColors.Blue;
                lineSeriesByte54BE.StrokeThickness = 0.5;
                this.ModelByte54BE = new PlotModel { Title = "5+4" };
                this.ModelByte54BE.Series.Add(lineSeriesByte54BE);
                plotByte54BE.Visibility = Visibility.Visible;

                plotByte54BE.Model = ModelByte54BE;
                ModelByte54BE.MouseDown += (s, e) =>
                {

                    if (e.IsShiftDown == true)
                    {
                        if (e.HitTestResult != null)
                        {
                            string item = e.HitTestResult.Item.ToString();
                            int index = item.IndexOf(" ");
                            int x = Convert.ToInt32(item.Remove(index));
                            try
                            {

                                Tab_Msg.IsSelected = true;
                                LB_Messages.SelectedIndex = Convert.ToInt32(x);
                                LB_Messages.ScrollIntoView(LB_Messages.Items[x]);
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }

                    }
                };

                plotByte54BE.Model.TrackerChanged += (s, e) =>
                {

                    if (e.HitResult != null)
                    {
                        e.HitResult.Text = "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                        e.HitResult.Item = e.HitResult.Item + "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                    }

                };
                #endregion
                #region chart76BE
                plotByte76BE.Visibility = Visibility.Collapsed;
                var lineSeriesByte76BE = new LineSeries();
                for (int i = 0; i < massByte76BE.Count; i++)
                {
                    lineSeriesByte76BE.Points.Add(new DataPoint(i, massByte76BE[i]));
                }
                lineSeriesByte76BE.Color = OxyColors.Blue;
                lineSeriesByte76BE.StrokeThickness = 0.5;
                this.ModelByte76BE = new PlotModel { Title = "7+6" };
                this.ModelByte76BE.Series.Add(lineSeriesByte76BE);
                plotByte76BE.Visibility = Visibility.Visible;

                plotByte76BE.Model = ModelByte76BE;
                ModelByte76BE.MouseDown += (s, e) =>
                {

                    if (e.IsShiftDown == true)
                    {
                        if (e.HitTestResult != null)
                        {
                            string item = e.HitTestResult.Item.ToString();
                            int index = item.IndexOf(" ");
                            int x = Convert.ToInt32(item.Remove(index));
                            try
                            {

                                Tab_Msg.IsSelected = true;
                                LB_Messages.SelectedIndex = Convert.ToInt32(x);
                                LB_Messages.ScrollIntoView(LB_Messages.Items[x]);
                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        }

                    }
                };

                ////////

                plotByte76BE.Model.TrackerChanged += (s, e) =>
                {

                    if (e.HitResult != null)
                    {
                        e.HitResult.Text = "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                        e.HitResult.Item = e.HitResult.Item + "Время: " + _timings[Convert.ToInt32(e.HitResult.DataPoint.X)];
                    }

                };
                #endregion
                /*LineGraphByte0.PlotY(massByte0);*/





            }


            //ModelByte0.MouseMove += (s, e) =>
            //{

            //    OxyMouseDownEventArgs oxy = (OxyMouseDownEventArgs)e;
            //    string item = oxy.HitTestResult.Item.ToString(); ;
            //    int index = item.IndexOf(" ");
            //    int x = Convert.ToInt32(item.Remove(index));
            //    try
            //    {

            //        Tab_Msg.IsSelected = true;
            //        LB_Messages.SelectedIndex = Convert.ToInt32(x);
            //        LB_Messages.ScrollIntoView(LB_Messages.Items[Convert.ToInt32(x)]);
            //    }
            //    catch (Exception)
            //    {

            //        throw;
            //    }

            //};

            //заполнение ListBox
            if (_selectedID != null)
            {
                int findID;

                List<string> replaseData1 = new List<string>();


                LB_Messages.ItemsSource = string.Empty;

                LB_Messages.ItemsSource = listBoxData;
                TB_PRM_Commits.Clear();
                for (int i = 0; i < PRM_Commits_List.Count; i++)
                {
                    if (PRM_Commits_List[i].Contains(_selectedID))
                    {
                        TB_PRM_Commits.Text = PRM_Commits_List[i];

                    }
                    //else
                    //{
                    //    TB_PRM_Commits.Clear();
                    //}
                }
                //_commits.Add(_uniqId.FindIndex(u => u.Contains(IdAdress)), words[i]);
                _timings.Clear();
                _timings = timing;
                if (_markedUniqId.Items.Count == 0)
                {
                    _markedUniqId = LB_Uniq;
                }
                if (_markedUniqId.Items.Count != 0)
                {
                    _markedUniqId.SelectedIndex = LB_Uniq.SelectedIndex;
                    LB_Uniq = _markedUniqId;
                }
                List<string> strings1 = new List<string>();
            }
            BtnReadFile_Click(sender, e);
            BtnReadFile.IsEnabled = true;
        }





        #region методы графиков при выборе точки

        private void TabControl_Analize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (Tab_Msg.IsSelected == true)
            {
                //plotByte0.IsMouseOver = false;
                //plotByte0.IsMouseCaptureWithin = false;
                //plotByte0.IsMouseCaptured = false;
                //plotByte0.IsMouseDirectlyOver = false;
                plotByte0.TrackerDefinitions.Clear();
            }

            if (TabItemOneByte.IsSelected == true && LB_Uniq.SelectedItem != null)
            {

                //TabItemOneByte.Refresh();

                //if (LabelOneByte.Content != null)
                //{
                //CB_FilterOneByte.SelectedItem = LabelOneByte.Content.ToString();
                _comboBoxValues.Clear();
                if (CB_FilterOneByte.SelectedIndex > 0)
                {
                    _comboBoxValues.Add(0, (string)CB_FilterOneByte.SelectedItem);
                }
                if (CB_FilterByte1.SelectedIndex > 0)
                {
                    _comboBoxValues.Add(1, (string)CB_FilterByte1.SelectedItem);
                }
                if (CB_FilterByte2.SelectedIndex > 0)
                {
                    _comboBoxValues.Add(2, (string)CB_FilterByte2.SelectedItem);
                }
                if (CB_FilterByte3.SelectedIndex > 0)
                {
                    _comboBoxValues.Add(3, (string)CB_FilterByte3.SelectedItem);
                }
                if (CB_FilterByte4.SelectedIndex > 0)
                {
                    _comboBoxValues.Add(4, (string)CB_FilterByte4.SelectedItem);
                }
                if (CB_FilterByte5.SelectedIndex > 0)
                {
                    _comboBoxValues.Add(5, (string)CB_FilterByte5.SelectedItem);
                }
                if (CB_FilterByte6.SelectedIndex > 0)
                {
                    _comboBoxValues.Add(6, (string)CB_FilterByte6.SelectedItem);
                }
                if (CB_FilterByte7.SelectedIndex > 0)
                {
                    _comboBoxValues.Add(7, (string)CB_FilterByte7.SelectedItem);
                }
                string msgId = (string)LB_Uniq.SelectedItem;
                LB_Uniq.SelectedIndex = -1;

                LB_Uniq.SelectedItem = msgId;
                //if (CB_FilterOneByte.SelectedItem !=null)
                //{
                //    _itemByte0Selected = CB_FilterOneByte.SelectedItem.ToString();
                //}
                //}
                //LabelOneByte.Visibility = Visibility.Visible;

            }
            if (Tab2xLE.IsSelected == true && LB_Uniq.SelectedItem != null)
            {
                //Tab2xLE.Refresh();
                string msgId = (string)LB_Uniq.SelectedItem;
                LB_Uniq.SelectedIndex = -1;
                LB_Uniq.SelectedItem = msgId;
                if (LabelOneByte.Content != null)
                {
                    CB_FilterOneByte.SelectedItem = LabelOneByte.Content.ToString();
                    if (CB_FilterOneByte.SelectedItem != null)
                    {
                        _itemByte0Selected = CB_FilterOneByte.SelectedItem.ToString();
                    }
                }
                LabelOneByte.Visibility = Visibility.Hidden;

            }
            if (Tab2xBE.IsSelected == true && LB_Uniq.SelectedItem != null)
            {
                //Tab2xBE.Refresh();
                string msgId = (string)LB_Uniq.SelectedItem;
                LB_Uniq.SelectedIndex = -1;
                LB_Uniq.SelectedItem = msgId;
                if (LabelOneByte.Content != null)
                {
                    CB_FilterOneByte.SelectedItem = LabelOneByte.Content.ToString();
                    if (CB_FilterOneByte.SelectedItem != null)
                    {
                        _itemByte0Selected = CB_FilterOneByte.SelectedItem.ToString();
                    }
                }
                LabelOneByte.Visibility = Visibility.Hidden;
            }
        }

        private void CB_FilterOneByte_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //e.Handled = true;
            //CB_FilterOneByte.IsDropDownOpen = false;
            ////CB_FilterOneByte.
            //if (CB_FilterOneByte.SelectedItem != _itemByte0Selected)
            //{
            //    if (TabItemOneByte.IsSelected == true)
            //    {
            //        plotByte0.Visibility = Visibility.Hidden;
            //        plotByte1.Visibility = Visibility.Hidden;
            //        plotByte2.Visibility = Visibility.Hidden;
            //        plotByte3.Visibility = Visibility.Hidden;
            //        plotByte4.Visibility = Visibility.Hidden;
            //        plotByte5.Visibility = Visibility.Hidden;
            //        plotByte6.Visibility = Visibility.Hidden;
            //        plotByte7.Visibility = Visibility.Hidden;
            //        if (CB_FilterOneByte.SelectedItem != null)
            //        {
            //            _itemByte0Selected = (string)CB_FilterOneByte.SelectedItem;

            //        }
            //        //TabItemOneByte.Refresh();
            //        string msgId = (string)LB_Uniq.SelectedItem;
            //        LB_Uniq.SelectedIndex = -1;
            //        plotByte0.Visibility = Visibility.Visible;
            //        plotByte1.Visibility = Visibility.Visible;
            //        plotByte2.Visibility = Visibility.Visible;
            //        plotByte3.Visibility = Visibility.Visible;
            //        plotByte4.Visibility = Visibility.Visible;
            //        plotByte5.Visibility = Visibility.Visible;
            //        plotByte6.Visibility = Visibility.Visible;
            //        plotByte7.Visibility = Visibility.Visible;
            //        LB_Uniq.SelectedItem = msgId;
            //    }
            //}
            //else
            //{


            //    //CB_FilterOneByte.SelectionChanged -= CB_FilterOneByte_SelectionChanged;
            //    //LB_Uniq.SelectionChanged -= Lb_Uniq_SelectionChanged;
            //}
        }

        //private void LineByte0_MouseMove(object sender, MouseEventArgs e)
        //{

        //    var hotSpotData = LineByte0.PeFunction.GetHotSpot();
        //    var x = hotSpotData.Data2;

        //    if (x >=0 || _timings.Count > 0)
        //    {
        //        while (x > _timings.Count - 1)
        //        {
        //            x--;
        //        }
        //        try
        //        {
        //            Label_Timing.Content = "Время: " + _timings[x];
        //        }
        //        catch (Exception)
        //        {

        //            throw;
        //        }

        //    }
        //    else
        //    {
        //        Label_Timing.Content = string.Empty;
        //    }
        //}


        private void MenuItemGreen_Click(object sender, RoutedEventArgs e)
        {

            int index = LB_Uniq.SelectedIndex;
            if (_markedUniqId.Items.Count != 0)
            {
                LB_Uniq = _markedUniqId;
            }
            ListBoxItem lbi = (ListBoxItem)_markedUniqId.ItemContainerGenerator.ContainerFromIndex(index);
            //lbi.Background = Brushes.Green;
            //lbi.Foreground = Brushes.White;
            lbi.Foreground = Brushes.Green;
            _greenIndex.Add(index);

            //lbi.SetValue();
            LB_Uniq = _markedUniqId;
        }

        private void MenuItemRed_Click(object sender, RoutedEventArgs e)
        {
            int index = LB_Uniq.SelectedIndex;
            if (_markedUniqId.Items.Count != 0)
            {
                LB_Uniq = _markedUniqId;
            }
            ListBoxItem lbi = (ListBoxItem)_markedUniqId.ItemContainerGenerator.ContainerFromIndex(index);
            lbi.Foreground = Brushes.Red;
            _redIndex.Add(index);
            LB_Uniq = _markedUniqId;
        }

        private void MenuItemClear_Click(object sender, RoutedEventArgs e)
        {
            int index = LB_Uniq.SelectedIndex;
            if (_markedUniqId.Items.Count != 0)
            {
                LB_Uniq = _markedUniqId;
            }
            ListBoxItem lbi = (ListBoxItem)_markedUniqId.ItemContainerGenerator.ContainerFromIndex(index);
            lbi.Foreground = Brushes.Black;
            if (_greenIndex.Contains(index))
            {
                _greenIndex.Remove(index);
            }
            if (_redIndex.Contains(index))
            {
                _redIndex.Remove(index);
            }
            LB_Uniq = _markedUniqId;
        }

        private void MenuItemHideItems_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < LB_Uniq.Items.Count; i++)
            {
                if (_markedUniqId.Items.Count != 0)
                {
                    LB_Uniq = _markedUniqId;
                }
                ListBoxItem lbi = (ListBoxItem)_markedUniqId.ItemContainerGenerator.ContainerFromIndex(i);

                if (lbi == null)
                {
                    LB_Uniq.UpdateLayout();
                    LB_Uniq.ScrollIntoView(LB_Uniq.Items[i]);
                    lbi = (ListBoxItem)_markedUniqId.ItemContainerGenerator.ContainerFromIndex(i);
                }



                if (lbi.Foreground == Brushes.Red)
                {
                    lbi.Visibility = Visibility.Collapsed;
                }
                LB_Uniq = _markedUniqId;
            }
            LB_Uniq.ScrollIntoView(LB_Uniq.Items[0]);
        }

        private void MenuItemShowAllItems_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < LB_Uniq.Items.Count; i++)
            {
                if (_markedUniqId.Items.Count != 0)
                {
                    LB_Uniq = _markedUniqId;
                }
                ListBoxItem lbi = (ListBoxItem)_markedUniqId.ItemContainerGenerator.ContainerFromIndex(i);
                if (lbi == null)
                {
                    LB_Uniq.UpdateLayout();
                    LB_Uniq.ScrollIntoView(LB_Uniq.Items[i]);
                    lbi = (ListBoxItem)_markedUniqId.ItemContainerGenerator.ContainerFromIndex(i);
                }
                if (lbi.Visibility == Visibility.Collapsed)
                {
                    lbi.Visibility = Visibility.Visible;
                }
                LB_Uniq = _markedUniqId;
            }
            LB_Uniq.ScrollIntoView(LB_Uniq.Items[0]);
        }

        private void MenuItemShowGreenItems_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < LB_Uniq.Items.Count; i++)
            {

                ListBoxItem lbi = (ListBoxItem)_markedUniqId.ItemContainerGenerator.ContainerFromIndex(i);
                if (lbi == null)
                {
                    LB_Uniq.UpdateLayout();
                    LB_Uniq.ScrollIntoView(LB_Uniq.Items[i]);
                    lbi = (ListBoxItem)_markedUniqId.ItemContainerGenerator.ContainerFromIndex(i);
                }
                if (lbi.Foreground != Brushes.Green)
                {
                    lbi.Visibility = Visibility.Collapsed;
                }
                LB_Uniq = _markedUniqId;

            }
            LB_Uniq.ScrollIntoView(LB_Uniq.Items[0]);
        }

        private void BtnSaveToFile_Click(object sender, RoutedEventArgs e)
        {
            //string logFileName = _currentFile.SafeFileName;
            //logFileName = logFileName.Remove(logFileName.Length - 4);
            //string textFileName = logFileName + "_commits.txt";
            string docPath = _currentFile.FileName + "_commits.txt";

            List<string> textLines = new List<string>();
            try
            {
                if (File.Exists(docPath) == true)
                {
                    File.Delete(docPath);
                }

                for (int i = 0; i < LB_Uniq.Items.Count; i++) // перебираем данные    
                {
                    ListBoxItem lbi = (ListBoxItem)_markedUniqId.ItemContainerGenerator.ContainerFromIndex(i);
                    if (lbi == null)
                    {
                        LB_Uniq.UpdateLayout();
                        LB_Uniq.ScrollIntoView(LB_Uniq.Items[i]);
                        lbi = (ListBoxItem)_markedUniqId.ItemContainerGenerator.ContainerFromIndex(i);
                    }
                    if (lbi.Foreground == Brushes.Green || lbi.Foreground == Brushes.Red || lbi.Visibility == Visibility.Collapsed || _commits.ContainsKey(i))
                    {
                        if (_commits.ContainsKey(i) == true)
                        {
                            textLines.Add(LB_Uniq.Items[i] + "|" + lbi.Foreground.ToString() + "|" + lbi.Visibility + "|" + _commits[i]);
                        }
                        else
                        {
                            textLines.Add(LB_Uniq.Items[i] + "|" + lbi.Foreground.ToString() + "|" + lbi.Visibility);
                        }
                    }


                }
                //File.AppendAllLines(System.IO.Path.Combine(docPath, textFileName), textLines);
                File.AppendAllLines(System.IO.Path.Combine(docPath), textLines);
                if (textLines.Count == 0)
                {
                    //File.Delete(docPath + "\\" + textFileName);
                    File.Delete(docPath);
                    MessageBox.Show("Нечего сохранять.");
                }

            }
            catch (Exception)
            {

                throw;
            }
            LB_Uniq.ScrollIntoView(LB_Uniq.Items[0]);
        }

        private void BtnReadFile_Click(object sender, RoutedEventArgs e)
        {
            //StreamWriter sw = new StreamWriter(path);
            string files;
            try
            {
                files = System.IO.File.ReadAllText(_currentFile.FileName + "_commits.txt");
            }
            catch (Exception)
            {

                return;
            }

            if (files != null && files != "")
            {
                List<string> words = new List<string>();
                words = files.Split('\n').ToList();
                words.RemoveAt(words.Count - 1);
                for (int i = 0; i < words.Count; i++)
                {

                    int index = words[i].IndexOf("\r");
                    words[i] = words[i].Remove(index);

                }
                for (int i = 0; i < LB_Uniq.Items.Count; i++)
                {
                    if (words.Count != 0)
                    {
                        int index = words[0].IndexOf("|");
                        string word = words[0].Remove(index);
                        if (LB_Uniq.Items[i].ToString() == word)
                        {
                            List<string> strings = new List<string>();
                            strings = words[0].Split("|").ToList();
                            ListBoxItem lbi = (ListBoxItem)LB_Uniq.ItemContainerGenerator.ContainerFromIndex(i);
                            if (lbi == null)
                            {
                                LB_Uniq.UpdateLayout();
                                LB_Uniq.ScrollIntoView(LB_Uniq.Items[i]);
                                lbi = (ListBoxItem)LB_Uniq.ItemContainerGenerator.ContainerFromIndex(i);
                            }
                            if (strings[1] == "#FF008000")
                            {
                                lbi.Foreground = Brushes.Green;
                            }
                            else if (strings[1] == "#FFFF0000")
                            {
                                lbi.Foreground = Brushes.Red;
                            }
                            if (strings[2] == "Visible")
                            {
                                lbi.Visibility = Visibility.Visible;
                            }
                            else if (strings[2] == "Collapsed")
                            {
                                lbi.Visibility = Visibility.Collapsed;
                            }
                            if (strings.Count == 4)
                            {
                                _commits.Add(i, strings[3]);
                            }
                            words.RemoveAt(0);

                            i = 0;
                        }
                    }
                    //int index = LB_Uniq.Items.
                    //ListBoxItem lbi = (ListBoxItem)LB_Uniq.ItemContainerGenerator.ContainerFromIndex(i);

                }
            }
            else
            {
                if (BtnReadFile.IsEnabled == true)
                {
                    OpenFileDialog ofd = new OpenFileDialog();
                    ofd.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
                    ofd.ShowDialog();
                    string filename = ofd.FileName;
                    if (ofd.FileName != "")
                    {
                        _commits.Clear();
                        files = System.IO.File.ReadAllText(filename);
                        List<string> words = new List<string>();
                        words = files.Split('\n').ToList();
                        words.RemoveAt(words.Count - 1);
                        for (int i = 0; i < words.Count; i++)
                        {

                            int index = words[i].IndexOf("\r");
                            words[i] = words[i].Remove(index);

                        }
                        for (int i = 0; i < LB_Uniq.Items.Count; i++)
                        {
                            if (words.Count != 0)
                            {
                                int index = words[0].IndexOf("|");
                                string word = words[0].Remove(index);
                                if (LB_Uniq.Items[i].ToString() == word)
                                {
                                    List<string> strings = new List<string>();
                                    strings = words[0].Split("|").ToList();
                                    ListBoxItem lbi = (ListBoxItem)LB_Uniq.ItemContainerGenerator.ContainerFromIndex(i);
                                    if (lbi == null)
                                    {
                                        LB_Uniq.UpdateLayout();
                                        LB_Uniq.ScrollIntoView(LB_Uniq.Items[i]);
                                        lbi = (ListBoxItem)LB_Uniq.ItemContainerGenerator.ContainerFromIndex(i);
                                    }
                                    if (strings[1] == "#FF008000")
                                    {
                                        lbi.Foreground = Brushes.Green;
                                    }
                                    else if (strings[1] == "#FFFF0000")
                                    {
                                        lbi.Foreground = Brushes.Red;
                                    }
                                    if (strings[2] == "Visible")
                                    {
                                        lbi.Visibility = Visibility.Visible;
                                    }
                                    else if (strings[2] == "Collapsed")
                                    {
                                        lbi.Visibility = Visibility.Collapsed;
                                    }
                                    if (strings.Count == 4)
                                    {
                                        _commits.Add(i, strings[3]);
                                    }
                                    words.RemoveAt(0);

                                    i = 0;
                                }
                            }
                            //int index = LB_Uniq.Items.
                            //ListBoxItem lbi = (ListBoxItem)LB_Uniq.ItemContainerGenerator.ContainerFromIndex(i);

                        }

                    }
                }
            }
        }

        private void LB_Uniq_Scroll(object sender, System.Windows.Controls.Primitives.ScrollEventArgs e)
        {
            if (_markedUniqId.Items.Count != 0)
            {
                //LB_Uniq.ItemContainerGenerator.

                for (int i = 0; i < _greenIndex.Count; i++)
                {
                    ListBoxItem lbi = (ListBoxItem)LB_Uniq.ItemContainerGenerator.ContainerFromIndex(_greenIndex[i]);
                    if (lbi != null)
                    {
                        lbi.Foreground = Brushes.Green;
                    }
                }
                for (int i = 0; i < _redIndex.Count; i++)
                {
                    ListBoxItem lbi = (ListBoxItem)LB_Uniq.ItemContainerGenerator.ContainerFromIndex(_redIndex[i]);
                    if (lbi != null)
                    {
                        lbi.Foreground = Brushes.Red;
                    }
                }
                LB_Uniq = _markedUniqId;
                //LB_Uniq.ScrollIntoView(LB_Uniq.Items[0]);
                //LB_Uniq.Refresh();
                //LB_Uniq.UpdateLayout();
            }
        }

        private void LB_Uniq_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (_markedUniqId.Items.Count != 0)
            {
                //LB_Uniq.ItemContainerGenerator.

                for (int i = 0; i < _greenIndex.Count; i++)
                {
                    ListBoxItem lbi = (ListBoxItem)LB_Uniq.ItemContainerGenerator.ContainerFromIndex(_greenIndex[i]);
                    if (lbi != null)
                    {
                        lbi.Foreground = Brushes.Green;
                    }
                }
                for (int i = 0; i < _redIndex.Count; i++)
                {
                    ListBoxItem lbi = (ListBoxItem)LB_Uniq.ItemContainerGenerator.ContainerFromIndex(_redIndex[i]);
                    if (lbi != null)
                    {
                        lbi.Foreground = Brushes.Red;
                    }
                }
                LB_Uniq = _markedUniqId;

                //LB_Uniq.ScrollIntoView(LB_Uniq.Items[0]);
                //LB_Uniq.Refresh();
                //LB_Uniq.UpdateLayout();
            }
        }

        private void MenuItemEditCommit_Click(object sender, RoutedEventArgs e)
        {
            myPopup.IsOpen = true;
            TB_Commit.Clear();
            TB_Commit.Text = _commits[LB_Uniq.SelectedIndex];
            //string userCommit = Microsoft.VisualBasic.Interaction.InputBox("Комментарий", "", _commits[LB_Uniq.SelectedIndex]);
            //_commits.Remove(LB_Uniq.SelectedIndex);
            //_commits.Add(LB_Uniq.SelectedIndex, userCommit);
        }

        private void MenuItemAddCommit_Click(object sender, RoutedEventArgs e)
        {
            myPopup.IsOpen = true;
            TB_Commit.Clear();
            //string userCommit = Microsoft.VisualBasic.Interaction.InputBox("Комментарий","");

            //_commits.Add(LB_Uniq.SelectedIndex, userCommit);
            //int y = 0;
            //MenuItemAddCommit.Visibility = Visibility.Collapsed;
            //MenuItemEditCommit.Visibility = Visibility.Visible;
        }
        bool _countACE;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (_currentFile != null)
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Text files (*.log)|*.log|All files (*.*)|*.*";
                ofd.ShowDialog();
                _currentFile = ofd;
                _countACE = true;
                Btn_ACE(sender, e);

            }
        }

        private void BtnSaveCommit(object sender, RoutedEventArgs e)
        {

            if (TB_Commit.Text != null && TB_Commit.Text != string.Empty)
            {
                if (_commits.ContainsKey(LB_Uniq.SelectedIndex))
                {
                    _commits.Remove(LB_Uniq.SelectedIndex);
                }
                _commits.Add(LB_Uniq.SelectedIndex, TB_Commit.Text);
                MenuItemAddCommit.Visibility = Visibility.Collapsed;
                MenuItemEditCommit.Visibility = Visibility.Visible;
                myPopup.IsOpen = false;
            }
            else
            {
                MessageBox.Show("Нечего сохранять");
            }
        }

        private void BtnCommitCancel(object sender, RoutedEventArgs e)
        {
            myPopup.IsOpen = false;
        }



        #endregion
        public List<string> SortingCBox(List<string> strings)
        {
            List<int> converter = new List<int>();
            for (int i = 0; i < strings.Count; i++)
            {
                if (strings[i] == "без фильтра")
                {
                    strings.RemoveAt(i);
                }
                converter.Add(Convert.ToInt32(strings[i], 16));
            }
            converter = converter.OrderBy(c => c).ToList();
            strings.Clear();
            strings.Add("без фильтра");
            for (int i = 0; i < converter.Count; i++)
            {
                //strings.Add(Convert.ToString(converter[i], 16));
                string x = Convert.ToString(converter[i], 16);
                if (x.Length == 1)
                {
                    x = ("0" + x);
                }
                //x = x.ToUpper();
                strings.Add(x.ToUpper());
            }
            return strings;
        }
        //public static RoutedCommand ShiftAndLeftClick = new RoutedCommand();
        CommandBinding ShiftAndLeftClick = new CommandBinding();
        List<string> PRM_Commits_List = new List<string>();
        Dictionary<string, List<string>> _dictForCommitsPMR = new Dictionary<string, List<string>>();

        private void Btn_PRM_CLick(object sender, RoutedEventArgs e)
        {
            _dictForCommitsPMR.Clear();
            var MyIni = new IniFiles("Settings.ini");
            var ReadFile = MyIni.ReadINI("InterraCAN", "ReadFile");
            //if (ReadFile == "")
            //{

            //}
            //var readCFG = System.IO.File.ReadAllText("PRM.cfg");

            //OpenFileDialog ofd = new OpenFileDialog();
            //if (ofd.FileName == "")
            //{

            //    ofd.Filter = "Text files (*.txt)|prm*.txt|All files (*.*)|*.*";
            //    ofd.ShowDialog();
            //    string filename = ofd.FileName;
            //string files = System.IO.File.ReadAllText(filename, Encoding.UTF8);
            //string files = System.IO.File.ReadAllText($"..\\prm_xxxxxxxx1.txt");

            //string files = System.IO.File.ReadAllText(readCFG + ".txt");
            string files;
            try
            {
                files = System.IO.File.ReadAllText(ReadFile);
            }
            catch (Exception)
            {
                MessageBox.Show("Файл " + ReadFile + " не был найден.");
                return;
            }
            //string files = System.IO.File.ReadAllText("C:\\Users\\technic\\Downloads\\prm_xxxxxxxx1.txt");
            //\r\n\r\n\r\n
            List<string> words = files.Split("Адрес").ToList();
            words.RemoveAt(0);
            List<string> uniqId = new List<string>();
            List<string> regex1 = new List<string>();
            List<string> regex2 = new List<string>();

            for (int i = 0; i < words.Count; i++)
            {
                //words[i] = Encoding.Default.GetString(words[i]);
                ////words[i] = 
                if (words[i] == "")
                {
                    words.RemoveAt(i);
                    i--;
                }
                else
                {

                    //if (CheckBox_Filter_PGN.IsChecked == true)
                    //{
                    //    for (int j = 0; j < _uniqId.Count; j++)
                    //    {
                    //        //string regex1 = _uniqId[i]
                    //        string regexStart = Regex.Replace(_uniqId[j], @"0x..", "");
                    //        regex1.Add(Regex.Replace(_uniqId[j], @"......\b", ""));

                    //        string regexEnd = Regex.Replace(regexStart, @"..\b", "");
                    //        regex2.Add(Regex.Replace(_uniqId[j], @"........", ""));
                    //        uniqId.Add(regexEnd);
                    //    }
                    //}
                    //else
                    //{
                    uniqId = _uniqId;
                    //}
                    int firstIndex = words[i].IndexOf("0");
                    int lastIndex = words[i].IndexOf("\r", firstIndex);

                    string IdAdress = words[i].Substring(firstIndex, lastIndex - firstIndex);
                    //if (CheckBox_Filter_PGN.IsChecked == true)
                    //{

                    //    //string regex1 = _uniqId[i]
                    //    string regex = Regex.Replace(IdAdress, @"0x..", "");
                    //    IdAdress = Regex.Replace(regex, @"..\b", "");


                    //}
                    if (uniqId.Find(u => u.Contains(IdAdress)) != null)
                    {
                        List<string> PRM_bytes = new List<string>();
                        PRM_bytes = words[i].Split("\r\n\r\n").ToList();
                        PRM_bytes.RemoveAt(0);

                        //PRM_Commits_List.Add(words[i]);
                        if (_dictForCommitsPMR.ContainsKey(IdAdress))
                        {
                            while (_dictForCommitsPMR.ContainsKey(IdAdress))
                            {
                                IdAdress = IdAdress + "|";
                            }
                            _dictForCommitsPMR.Add(IdAdress, PRM_bytes);
                            //_dictForCommitsPMR[IdAdress].Add(PRM_bytes[j]);

                            //_dictForCommitsPMR[IdAdress].Add(PRM_bytes);
                        }
                        else
                        {
                            _dictForCommitsPMR.Add(IdAdress, PRM_bytes);
                        }

                        //_commits.Add(_uniqId.FindIndex(u => u.Contains(IdAdress)), words[i]);
                        for (int j = 0; j < _dictForCommitsPMR[IdAdress].Count; j++)
                        {
                            if (_dictForCommitsPMR[IdAdress][j] == "\r\n" || _dictForCommitsPMR[IdAdress][j] == "")
                            {
                                _dictForCommitsPMR[IdAdress].RemoveAt(j);
                                j--;
                                break;
                            }
                            if (_dictForCommitsPMR[IdAdress][j].Remove(5) == " байт")
                            {
                                continue;
                            }
                            string message = _dictForCommitsPMR[IdAdress][j].Remove(0, _dictForCommitsPMR[IdAdress][j].IndexOf("\r\n"));
                            //firstIndex = _dictForCommitsPMR[IdAdress][j].IndexOf(",")+1;
                            //lastIndex = _dictForCommitsPMR[IdAdress][j].IndexOf("\r\n", firstIndex);
                            firstIndex = message.IndexOf(",") + 1;
                            lastIndex = message.IndexOf("\r\n", firstIndex);
                            string stroke = message.Substring(firstIndex, lastIndex - firstIndex);
                            string stringByte;
                            string stringBit = null;
                            if (stroke.IndexOf(",") != -1)
                            {
                                //indexer subIndex1 = 
                                stringByte = stroke.Remove(stroke.IndexOf(","));
                                stringByte = string.Concat(stringByte.Where(Char.IsDigit));
                                stringBit = stroke.Remove(0, stroke.IndexOf(","));
                                stringBit = string.Concat(stringBit.Where(Char.IsDigit));
                            }
                            else
                            {
                                stringByte = string.Concat(stroke.Where(Char.IsDigit));
                            }
                            int y = 0;
                            //ПЕРЕПИСАТЬ ЛОГИКУ
                            //�
                            //ЧТОБЫ БАЙТЕ ПРОВЕРЯЛИСЬ ВМЕСТЕ
                            if (stringBit == null)
                            {
                                if (stringByte.Length > 1)
                                {

                                    _dictForCommitsPMR[IdAdress][j] = _dictForCommitsPMR[IdAdress][j].Replace("�����", "байты");
                                    int byte1 = Convert.ToInt32(stringByte.Substring(0, 1)) - 1;
                                    int byte2 = Convert.ToInt32(stringByte.Substring(1, 1)) - 1;
                                    var listForDict = _dictForCommitsPMR[IdAdress][j].Split("\r\n");
                                    listForDict[1] = listForDict[1].Remove(0, listForDict[1].IndexOf(",") + 1);
                                    _dictForCommitsPMR[IdAdress][j] = listForDict[1] + ";" + listForDict[0] + ";" + listForDict[2] + ";" + listForDict[3] + ";";

                                    List<string> massByte = new List<string>();
                                    //if (CheckBox_Filter_PGN.IsChecked == true)
                                    //{
                                    //    for (int c = 0; c < _messages[regex1[i] + IdAdress + regex2[i]].Count; c++)
                                    //    {
                                    //        string strokeBytes = string.Empty;
                                    //        int count = byte1;
                                    //        for (int l = 0; l < byte2 - byte1 + 1; l++)
                                    //        {
                                    //            strokeBytes = strokeBytes + _messages[regex1[i] + IdAdress + regex2[i]][c][count];
                                    //            count++;
                                    //        }
                                    //        //massByte.Add(_messages[IdAdress][c][byte1]+ _messages[IdAdress][c][byte2]);
                                    //        massByte.Add(strokeBytes);
                                    //    }
                                    //}
                                    //else
                                    //{
                                    int indexAdress = _dictForCommitsPMR.ElementAt(0).Key.Length;
                                    string IdPath = IdAdress.Remove(0, indexAdress);
                                    IdAdress = IdAdress.Remove(indexAdress);
                                    for (int c = 0; c < _messages[IdAdress].Count; c++)
                                    {
                                        string strokeBytes = string.Empty;
                                        int count = byte1;
                                        for (int l = 0; l < byte2 - byte1 + 1; l++)
                                        {
                                            strokeBytes = strokeBytes + _messages[IdAdress][c][count];
                                            count++;
                                        }
                                        //massByte.Add(_messages[IdAdress][c][byte1]+ _messages[IdAdress][c][byte2]);
                                        massByte.Add(strokeBytes);
                                    }
                                    //}
                                    List<string> distinct = massByte.Distinct().ToList();
                                    if (distinct.Count <= 1)
                                    {
                                        if (CheckBox_Filter_PRM.IsChecked == true)
                                        {
                                            if (distinct[0] == "0000")
                                            {
                                                if (IdPath != "")
                                                {
                                                    IdAdress = IdAdress + IdPath;
                                                    _dictForCommitsPMR[IdAdress].RemoveAt(j);
                                                    j--;
                                                }
                                                else
                                                {
                                                    _dictForCommitsPMR[IdAdress].RemoveAt(j);
                                                    j--;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (IdPath != "")
                                            {
                                                IdAdress = IdAdress + IdPath;
                                                _dictForCommitsPMR[IdAdress].RemoveAt(j);
                                                j--;
                                            }
                                            else
                                            {
                                                _dictForCommitsPMR[IdAdress].RemoveAt(j);
                                                j--;
                                            }
                                        }
                                    }


                                    //int count = 0;
                                    //int lenght = stringByte.Length;
                                    //for (int x = 0; x < stringByte.Length; x++)
                                    //{
                                    //    int oneByte = Convert.ToInt32(stringByte.Substring(0, 1));
                                    //    //int distinct = Convert.ToInt32(_messages[IdAdress][oneByte].Distinct());
                                    //    List<string> massByte = new List<string>();
                                    //    for (int c = 0; c < _messages[IdAdress].Count; c++)
                                    //    {
                                    //        massByte.Add(_messages[IdAdress][c][oneByte - 1]);
                                    //    }
                                    //    List<string> distinct = massByte.Distinct().ToList();
                                    //    if (distinct.Count > 1)
                                    //    {
                                    //        stringByte.Remove(0, 1);
                                    //        count++;
                                    //    }
                                    //    else
                                    //    {
                                    //        if (lenght == x && count != lenght)
                                    //        {
                                    //                _dictForCommitsPMR[IdAdress].RemoveAt(j);
                                    //                j--;

                                    //                break;

                                    //        }

                                    //    }
                                    //}
                                }
                                else
                                {
                                    _dictForCommitsPMR[IdAdress][j] = _dictForCommitsPMR[IdAdress][j].Replace("����", "байт");
                                    var listForDict = _dictForCommitsPMR[IdAdress][j].Split("\r\n");
                                    listForDict[1] = listForDict[1].Remove(0, listForDict[1].IndexOf(",") + 1);
                                    _dictForCommitsPMR[IdAdress][j] = listForDict[1] + ";" + listForDict[0] + ";" + listForDict[2] + ";" + listForDict[3] + ";";
                                    int oneByte = Convert.ToInt32(stringByte);
                                    int indexAdress = _dictForCommitsPMR.ElementAt(0).Key.Length;
                                    string IdPath = IdAdress.Remove(0, indexAdress);
                                    IdAdress = IdAdress.Remove(indexAdress);
                                    List<string> massByte = new List<string>();
                                    //if (CheckBox_Filter_PGN.IsChecked == true)
                                    //{
                                    //    for (int c = 0; c < _messages[regex1[i] + IdAdress + regex2[i]].Count; c++)
                                    //    {
                                    //        massByte.Add(_messages[regex1[i] + IdAdress + regex2[i]][c][oneByte - 1]);
                                    //    }
                                    //}
                                    //else
                                    //{
                                    for (int c = 0; c < _messages[IdAdress].Count; c++)
                                    {
                                        massByte.Add(_messages[IdAdress][c][oneByte - 1]);
                                    }
                                    //}
                                    List<string> distinct = massByte.Distinct().ToList();
                                    if (distinct.Count > 1)
                                    {
                                        stringByte.Remove(0, 1);
                                    }
                                    else
                                    {
                                        if (CheckBox_Filter_PRM.IsChecked == true)
                                        {
                                            if (distinct[0] == "00")
                                            {
                                                if (IdPath != "")
                                                {
                                                    IdAdress = IdAdress + IdPath;
                                                    _dictForCommitsPMR[IdAdress].RemoveAt(j);
                                                    j--;
                                                }
                                                else
                                                {
                                                    _dictForCommitsPMR[IdAdress].RemoveAt(j);
                                                    j--;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (IdPath != "")
                                            {
                                                IdAdress = IdAdress + IdPath;
                                                _dictForCommitsPMR[IdAdress].RemoveAt(j);
                                                j--;
                                            }
                                            else
                                            {
                                                _dictForCommitsPMR[IdAdress].RemoveAt(j);
                                                j--;
                                            }
                                        }


                                    }
                                }
                            }
                            else
                            {
                                if (stringBit.Length > 1)
                                {
                                    int indexstroke = _dictForCommitsPMR[IdAdress][j].IndexOf(", ") + 6;
                                    string halfstroke1 = _dictForCommitsPMR[IdAdress][j].Substring(0, indexstroke);
                                    halfstroke1 = halfstroke1.Replace("����", "байт");
                                    string halfstroke2 = _dictForCommitsPMR[IdAdress][j].Remove(0, indexstroke);
                                    halfstroke2 = halfstroke2.Replace("����", "биты");
                                    _dictForCommitsPMR[IdAdress][j] = halfstroke1 + halfstroke2;
                                    var listForDict = _dictForCommitsPMR[IdAdress][j].Split("\r\n");
                                    listForDict[1] = listForDict[1].Remove(0, listForDict[1].IndexOf(",") + 1);
                                    _dictForCommitsPMR[IdAdress][j] = listForDict[1] + ";" + listForDict[0] + ";" + listForDict[2] + ";" + listForDict[3] + ";";
                                    int minBit = Convert.ToInt32(stringBit.Remove(1)) - 1;
                                    int maxBit = Convert.ToInt32(stringBit.Remove(0, 1)) - 1;
                                    int oneByte = Convert.ToInt32(stringByte) - 1;
                                    int indexAdress = _dictForCommitsPMR.ElementAt(0).Key.Length;
                                    string IdPath = IdAdress.Remove(0, indexAdress);
                                    IdAdress = IdAdress.Remove(indexAdress);
                                    List<string> listBits = new List<string>();
                                    //BitArray bits = new BitArray(_messages[IdAdress][0][oneByte]);
                                    //if (CheckBox_Filter_PGN.IsChecked == true)
                                    //{
                                    //    for (int c = 0; c < _messages[regex1[i] + IdAdress + regex2[i]].Count; c++)
                                    //    {
                                    //        var bits = Convert.ToInt32(_messages[regex1[i] + IdAdress + regex2[i]][c][oneByte], 16);
                                    //        string stringBits = Convert.ToString(bits, 2);
                                    //        while (stringBits.Length != 8)
                                    //        {
                                    //            stringBits = "0" + stringBits;
                                    //        }
                                    //        //переворачиваем строку для правильной проверки битов
                                    //        StringBuilder sb = new StringBuilder();
                                    //        for (int h = stringBits.Length - 1; h >= 0; h--)
                                    //        {
                                    //            sb.Append(stringBits[h]);
                                    //        }
                                    //        string reverseBits = sb.ToString();
                                    //        listBits.Add(reverseBits.Substring(minBit, (maxBit - minBit) + 1));
                                    //    }
                                    //}
                                    //else
                                    //{
                                    for (int c = 0; c < _messages[IdAdress].Count; c++)
                                    {
                                        var bits = Convert.ToInt32(_messages[IdAdress][c][oneByte], 16);
                                        string stringBits = Convert.ToString(bits, 2);
                                        while (stringBits.Length != 8)
                                        {
                                            stringBits = "0" + stringBits;
                                        }
                                        //переворачиваем строку для правильной проверки битов
                                        StringBuilder sb = new StringBuilder();
                                        for (int h = stringBits.Length - 1; h >= 0; h--)
                                        {
                                            sb.Append(stringBits[h]);
                                        }
                                        string reverseBits = sb.ToString();
                                        listBits.Add(reverseBits.Substring(minBit, (maxBit - minBit) + 1));
                                    }
                                    //}
                                    List<string> distinct = listBits.Distinct().ToList();
                                    if (distinct.Count <= 1)
                                    {
                                        if (CheckBox_Filter_PRM.IsChecked == true)
                                        {
                                            string zeroBits = null;
                                            for (int d = 0; d < (maxBit - minBit) + 1; d++)
                                            {
                                                zeroBits = zeroBits + "0";
                                            }
                                            if (distinct[0] == zeroBits)
                                            {
                                                if (IdPath != "")
                                                {
                                                    IdAdress = IdAdress + IdPath;
                                                    _dictForCommitsPMR[IdAdress].RemoveAt(j);
                                                    j--;
                                                }
                                                else
                                                {
                                                    _dictForCommitsPMR[IdAdress].RemoveAt(j);
                                                    j--;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (IdPath != "")
                                            {
                                                IdAdress = IdAdress + IdPath;
                                                _dictForCommitsPMR[IdAdress].RemoveAt(j);
                                                j--;
                                            }
                                            else
                                            {
                                                _dictForCommitsPMR[IdAdress].RemoveAt(j);
                                                j--;
                                            }
                                        }



                                    }

                                    //�

                                }
                            }
                            //string.Concat(oneWord.Where(Char.IsDigit));


                        }
                        string commitPRM = string.Empty;
                        //= IdAdress + "\r\n";
                        for (int j = 0; j < _dictForCommitsPMR[IdAdress].Count; j++)
                        {
                            commitPRM = commitPRM + IdAdress + _dictForCommitsPMR[IdAdress][j] + "\r\n" + "\r\n";
                        }
                        if (_dictForCommitsPMR[IdAdress].Count > 0)
                        {
                            PRM_Commits_List.Add(commitPRM);
                        }

                    }

                }
            }
            Dictionary<string, List<string>> dictPGN = new Dictionary<string, List<string>>();
            if (CheckBox_Filter_PGN.IsChecked == true)
            {
                int dictcount = _dictForCommitsPMR.Keys.Count;
                for (int i = 0; i < _dictForCommitsPMR.Keys.Count; i++)
                {
                    var key = _dictForCommitsPMR.ElementAt(i).Key;

                    int indexAdress = _dictForCommitsPMR.ElementAt(0).Key.Length;
                    string IdPath = key.Remove(0, indexAdress);
                    key = key.Remove(indexAdress);

                    string regex = Regex.Replace(key, @"0x..", "");
                    if (key.Length > _dictForCommitsPMR.ElementAt(0).Key.Length)
                    {
                        int countPath = key.Length - _dictForCommitsPMR.ElementAt(0).Key.Length;
                        string regexPath = string.Empty;
                        for (int x = 0; x < countPath; x++)
                        {
                            regexPath = regexPath + ".";
                        }
                        key = Regex.Replace(regex, @".." + regexPath + "\b", "");
                    }
                    key = Regex.Replace(regex, @"..\b", "");
                    List<string> dictList = _dictForCommitsPMR[_dictForCommitsPMR.ElementAt(i).Key];
                    //if (IdPath != "")
                    //{
                    //    key = key + IdPath;
                    //}
                    //if (dictPGN.ContainsKey(key))
                    //{
                    //    //for (int j = 0; j < dictList.Count; j++)
                    //    //{
                    //    //    dictPGN[key].Add(dictList[j]);
                    //    //}
                    //    dictPGN.Add(key + "|", dictList);
                    //}
                    //else
                    //{
                    dictPGN.Add(key + _dictForCommitsPMR.ElementAt(i).Key, dictList);
                    //_dictForCommitsPMR.Remove(_dictForCommitsPMR.ElementAt(i).Key);
                    //}

                    //if (_dictForCommitsPMR.ContainsKey(key))
                    //{
                    //    List<string> dictList = _dictForCommitsPMR[_dictForCommitsPMR.ElementAt(i).Key];
                    //    for (int j = 0; j < dictList.Count; j++)
                    //    {
                    //        _dictForCommitsPMR[key].Add(dictList[j]);
                    //    }
                    //    //_dictForCommitsPMR[IdAdress].Add(PRM_bytes);
                    //}
                    //else
                    //{
                    //    List<string> dictList = _dictForCommitsPMR[_dictForCommitsPMR.ElementAt(i).Key];
                    //    _dictForCommitsPMR.Add(key, dictList);
                    //    _dictForCommitsPMR.Remove(_dictForCommitsPMR.ElementAt(i).Key);

                    //}



                    //    //string regex1 = _uniqId[i]
                    //    string regex = Regex.Replace(IdAdress, @"0x..", "");
                    //    IdAdress = Regex.Replace(regex, @"..\b", "");
                }
                _dictForCommitsPMR = dictPGN;
            }

            //TB_PRM_Commits.Text = TB_PRM_Commits.Text + words[i];
            //if (_dictForCommitsPMR.Count != 0)
            //{
            //    for (int i = 0; i < _dictForCommitsPMR.Count; i++)
            //    {
            //        for (int j = 0; j < _dictForCommitsPMR[].Count; j++)
            //        {
            //            //int firstIndex = _dictForCommitsPMR
            //        }
            //    }
            //}
            Tab_Charts.IsSelected = true;
            Tab_Msg.IsSelected = true;
            //string fileName = _currentFile.SafeFileName + "_J1939.txt";
            //$"..\\prm_xxxxxxxx1.txt"
            //string path = "C:\\Users\\technic\\Downloads\\" + _currentFile.SafeFileName + "_J1939.txt";
            string path = _currentFile.FileName + "_J1939.txt";
            string pathCMD = _currentFile.FileName + "_cmd.txt";
            string pathObj = _currentFile.FileName + "_obj.txt";
            if (File.Exists(path) == true)
            {
                File.Delete(path);
            }
            if (File.Exists(pathCMD) == true)
            {
                File.Delete(pathCMD);
            }
            if (File.Exists(pathObj) == true)
            {
                File.Delete(pathObj);
            }
            if (_dictForCommitsPMR.Count == 0)
            {
                return;
            }
            int check = 0;
            for (int i = 0; i < _dictForCommitsPMR.Keys.Count; i++)
            {
                if (_dictForCommitsPMR[_dictForCommitsPMR.ElementAt(i).Key].Count != 0)
                {
                    check++;
                }
                if (check < 0)
                {
                    break;
                }
            }
            StreamWriter sw = new StreamWriter(path);
            StreamWriter swCMD = new StreamWriter(pathCMD);
            StreamWriter obj = new StreamWriter(pathObj);
            List<string> listCAN8 = new List<string>();
            List<string> listCAN16 = new List<string>();
            List<string> listCAN32 = new List<string>();
            List<string> listCommands = new List<string>();
            List<string> listTags = new List<string>();
            int count8 = 0;
            int count16 = 0;
            int count32 = 0;
            var MaxCAN8Values = Convert.ToInt32(MyIni.ReadINI("InterraCAN", "MaxCAN8Values")); int CAN8ValuesCount = 0;
            var MaxCAN16Values = Convert.ToInt32(MyIni.ReadINI("InterraCAN", "MaxCAN16Values")); int CAN16ValuesCount = 0;
            var MaxCAN32Values = Convert.ToInt32(MyIni.ReadINI("InterraCAN", "MaxCAN32Values")); int CAN32ValuesCount = 0;
            //CANREGIME 3,250000,2000,0
            //ACTIVECAN 0
            swCMD.WriteLine("CANREGIME 3," + Convert.ToString(_speedCAN * 1000) + ",2000,0");
            swCMD.WriteLine("ACTIVECAN 0");
            int count0 = 0;
            int count1 = 0;

            for (int i = 0; i < _dictForCommitsPMR.Keys.Count; i++)
            {
                int indexAdress = _dictForCommitsPMR.ElementAt(0).Key.Length;
                //var key = _dictForCommitsPMR.Take(i).Select(d => d.Key).First();
                string key = _dictForCommitsPMR.ElementAt(i).Key;
                //string IdPath = key.Remove(0, indexAdress);
                //key = key.Remove(indexAdress);
                _dictForCommitsPMR[key] = _dictForCommitsPMR[key].Distinct().ToList();
                if (_dictForCommitsPMR[key].Count != 0)
                {
                    
                    for (int j = 0; j < _dictForCommitsPMR[key].Count; j++)
                    {
                        string c;
                        string offset;
                        string f = "";

                        int firstIndex = _dictForCommitsPMR[key][j].IndexOf(";");
                        if (firstIndex == -1)
                        {
                            _dictForCommitsPMR[key].RemoveAt(j);
                            break;
                        }
                        string IdPath = key.Remove(0, indexAdress);
                        key = key.Remove(indexAdress);
                        if (CheckBox_Filter_PGN.IsChecked == true)
                        {
                            if (IdPath != "")
                            {
                                sw.WriteLine(key.Remove(4) + " " + _dictForCommitsPMR[key + IdPath][j]);
                            }
                            else
                            {
                                sw.WriteLine(key.Remove(4) + " " + _dictForCommitsPMR[key][j]);
                            }

                        }
                        else
                        {
                            if (IdPath != "")
                            {
                                sw.WriteLine(key + " " + _dictForCommitsPMR[key + IdPath][j]);
                            }
                            else
                            {
                                sw.WriteLine(key + " " + _dictForCommitsPMR[key][j]);
                            }
                        }
                        string chars;
                        if (IdPath != "")
                        {
                            chars = _dictForCommitsPMR[key + IdPath][j].Remove(firstIndex);
                        }
                        else
                        {
                            chars = _dictForCommitsPMR[key][j].Remove(firstIndex);
                        }
                        //берем коэффициент и смещение
                        var splitStroke = _dictForCommitsPMR[key][j].Split(";");
                        c = splitStroke[2].Remove(splitStroke[2].IndexOf(","));
                        var indexC = c.IndexOf(" ");
                        if (indexC == -1)
                        {
                            indexC = c.IndexOf("/");
                        }
                        c = c.Remove(indexC);
                        offset = splitStroke[2].Remove(0, splitStroke[2].IndexOf(",") + 1);
                        offset = offset.Remove(0, 1);
                        offset = offset.Remove(offset.IndexOf(" "));
                        if (c != "1")
                        {
                            if (offset != "0")
                            {
                                if (offset.Contains("-"))
                                {
                                    f = "*const" + c + offset.Remove(1) + "const" + offset.Remove(0, 1);
                                }
                                else
                                {
                                    f = "*const" + c + "+" + "const" + offset;
                                }
                            }
                            else
                            {
                                f = "*const" + c;
                            }
                        }
                        else
                        {
                            if (offset != "0")
                            {
                                if (offset.Contains("-"))
                                {
                                    f = offset.Remove(1) + "const" + offset.Remove(0, 1);
                                }
                                else
                                {
                                    f = "+" + "const" + offset;
                                }
                            }
                        }

                        chars = string.Concat(chars.Where(Char.IsDigit));
                        int CANbit;
                        if (chars.Length >= 3)
                        {
                            chars = chars.Substring(0, chars.Length - 2);
                        }
                        if (chars.Length == 2)
                        {
                            int minByte = Convert.ToInt32(chars.Substring(0, 1));
                            int maxByte = Convert.ToInt32(chars.Substring(1, 1));
                            CANbit = (maxByte - minByte + 1) * 8;
                            if (CANbit < 24 && CAN16ValuesCount < MaxCAN16Values)
                            {
                                string CANCommand16;
                                if (CheckBox_Filter_PGN.IsChecked == true)
                                {
                                    //if (listCAN16.Contains(key.Remove(4)))
                                    //{
                                    if (IdPath != "")
                                    {
                                        //var ReadFile = MyIni.ReadINI("InterraCAN", "ReadFile");

                                        //CANCommand16 = "CAN" + "16" + "BITR" + Convert.ToString(listCAN16.IndexOf(key.Remove(4)));
                                        CANCommand16 = "CAN" + "16" + "BITR" + Convert.ToString(count16);
                                        sw.WriteLine(CANCommand16 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 6), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                        //swCMD.WriteLine(CANCommand16 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 6), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                        //sw.WriteLine("mainpackbit " + MyIni.ReadINI("CANCommands", CANCommand16) + ",1");

                                        if (count16 < 5)
                                        {
                                            //sw.WriteLine("can_r" + Convert.ToString(count16 + 18) + " " + _dictForCommitsPMR[key + IdPath][j].Remove(0, _dictForCommitsPMR[key + IdPath][j].IndexOf(";") + 1));
                                            obj.WriteLine("can_r" + Convert.ToString(count16 + 18) + f);
                                        }
                                        else if (count16 > 4 && count16 + 27 < 44)
                                        {
                                            //sw.WriteLine("can16bitr" + Convert.ToString(count16 + 27) + " " + _dictForCommitsPMR[key + IdPath][j].Remove(0, _dictForCommitsPMR[key + IdPath][j].IndexOf(";") + 1));
                                            obj.WriteLine("can16bitr" + Convert.ToString(count16 + 27) + f);
                                        }

                                        count16++;
                                        listTags.Add(CANCommand16 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 6), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                        listCommands.Add("mainpackbit " + MyIni.ReadINI("CANCommands", CANCommand16) + ",1");

                                    }
                                    else
                                    {
                                        CANCommand16 = "CAN" + "16" + "BITR" + Convert.ToString(count16);
                                        sw.WriteLine(CANCommand16 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 6), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                        //swCMD.WriteLine(CANCommand16 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 6), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                        //sw.WriteLine("mainpackbit " + MyIni.ReadINI("CANCommands", CANCommand16) + ",1");
                                        if (count16 < 5)
                                        {
                                            //sw.WriteLine("can_r" + Convert.ToString(count16 + 18) + " " + _dictForCommitsPMR[key][j].Remove(0, _dictForCommitsPMR[key][j].IndexOf(";") + 1));
                                            obj.WriteLine("can_r" + Convert.ToString(count16 + 18) + f);
                                        }
                                        else if (count16 > 4 && count16 + 27 < 44)
                                        {
                                            //sw.WriteLine("can16bitr" + Convert.ToString(count16 + 27) + " " + _dictForCommitsPMR[key][j].Remove(0, _dictForCommitsPMR[key][j].IndexOf(";") + 1));
                                            obj.WriteLine("can16bitr" + Convert.ToString(count16 + 27) + f);
                                        }
                                        count16++;
                                        listTags.Add(CANCommand16 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 6), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                        listCommands.Add("mainpackbit " + MyIni.ReadINI("CANCommands", CANCommand16) + ",1");

                                    }

                                    //}
                                    //else
                                    //{
                                    //    CANCommand16 = "CAN" + "16" + "BITR" + Convert.ToString(count16);
                                    //    sw.WriteLine(CANCommand16 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 6), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                    //    swCMD.WriteLine(CANCommand16 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 6), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                    //    sw.WriteLine("mainpackbit " + MyIni.ReadINI("CANCommands", CANCommand16) + ",1");
                                    //    count16++;
                                    //    listCAN16.Add(key.Remove(4));
                                    //}

                                }
                                else
                                {
                                    CANCommand16 = "CAN" + "16" + "BITR" + Convert.ToString(count16);
                                    sw.WriteLine(CANCommand16 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 2), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                    //swCMD.WriteLine(CANCommand16 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 2), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                    //sw.WriteLine("mainpackbit " + MyIni.ReadINI("CANCommands", CANCommand16) + ",1");
                                    if (count16 < 5)
                                    {
                                        //sw.WriteLine("can_r" + Convert.ToString(count16 + 18) + " " + _dictForCommitsPMR[key][j].Remove(0, _dictForCommitsPMR[key][j].IndexOf(";") + 1));
                                        obj.WriteLine("can_r" + Convert.ToString(count16 + 18) + f);
                                    }
                                    else if (count16 > 4 && count16 + 27 < 44)
                                    {
                                        //sw.WriteLine("can16bitr" + Convert.ToString(count16 + 27) + " " + _dictForCommitsPMR[key][j].Remove(0, _dictForCommitsPMR[key][j].IndexOf(";") + 1));
                                        obj.WriteLine("can16bitr" + Convert.ToString(count16 + 27) + f);
                                    }
                                    count16++;
                                    listTags.Add(CANCommand16 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 2), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                    listCommands.Add("mainpackbit " + MyIni.ReadINI("CANCommands", CANCommand16) + ",1");
                                }
                                CAN16ValuesCount++;
                            }
                            else if (CANbit > 24 && CAN32ValuesCount < MaxCAN32Values)
                            {
                                string CANCommand32;
                                if (CheckBox_Filter_PGN.IsChecked == true)
                                {
                                    //if (listCAN32.Contains(key.Remove(4)))
                                    //{
                                    if (IdPath != "")
                                    {
                                        CANCommand32 = "CAN" + "32" + "BITR" + Convert.ToString(count32);
                                        sw.WriteLine(CANCommand32 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 6), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                        //swCMD.WriteLine(CANCommand32 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 6), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                        //sw.WriteLine("mainpackbit " + MyIni.ReadINI("CANCommands", CANCommand32) + ",1");
                                        if (count32 < 5)
                                        {
                                            //sw.WriteLine("can_r" + Convert.ToString(count32 + 23) + " " + _dictForCommitsPMR[key + IdPath][j].Remove(0, _dictForCommitsPMR[key + IdPath][j].IndexOf(";") + 1));
                                            obj.WriteLine("can_r" + Convert.ToString(count32 + 23) + f);
                                        }
                                        else if (count32 > 4 && count32 + 91 < 106)
                                        {
                                            //sw.WriteLine("can32bitr" + Convert.ToString(count32 + 91) + " " + _dictForCommitsPMR[key + IdPath][j].Remove(0, _dictForCommitsPMR[key + IdPath][j].IndexOf(";") + 1));
                                            obj.WriteLine("can32bitr" + Convert.ToString(count32 + 91) + f);
                                        }
                                        count32++;
                                        listTags.Add(CANCommand32 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 6), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                        listCommands.Add("mainpackbit " + MyIni.ReadINI("CANCommands", CANCommand32) + ",1");

                                    }
                                    else
                                    {
                                        CANCommand32 = "CAN" + "32" + "BITR" + Convert.ToString(count32);
                                        sw.WriteLine(CANCommand32 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 6), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                        //swCMD.WriteLine(CANCommand32 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 6), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                        //sw.WriteLine("mainpackbit " + MyIni.ReadINI("CANCommands", CANCommand32) + ",1");
                                        if (count32 < 5)
                                        {
                                            //sw.WriteLine("can_r" + Convert.ToString(count32 + 23) + " " + _dictForCommitsPMR[key][j].Remove(0, _dictForCommitsPMR[key][j].IndexOf(";") + 1));
                                            obj.WriteLine("can_r" + Convert.ToString(count32 + 23) + f);
                                        }
                                        else if (count32 > 4 && count32 + 91 < 106)
                                        {
                                            //sw.WriteLine("can32bitr" + Convert.ToString(count32 + 91) + " " + _dictForCommitsPMR[key][j].Remove(0, _dictForCommitsPMR[key][j].IndexOf(";") + 1));
                                            obj.WriteLine("can32bitr" + Convert.ToString(count32 + 91) + f);
                                        }
                                        count32++;
                                        listTags.Add(CANCommand32 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 6), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                        listCommands.Add("mainpackbit " + MyIni.ReadINI("CANCommands", CANCommand32) + ",1");

                                    }

                                    //}
                                    //else
                                    //{
                                    //    CANCommand32 = "CAN" + "32" + "BITR" + Convert.ToString(count32);
                                    //    sw.WriteLine(CANCommand32 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 6), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                    //    swCMD.WriteLine(CANCommand32 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 6), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                    //    sw.WriteLine("mainpackbit " + MyIni.ReadINI("CANCommands", CANCommand32) + ",1");
                                    //    count32++;
                                    //    listCAN32.Add(key.Remove(4));
                                    //}

                                }
                                else
                                {
                                    CANCommand32 = "CAN" + "32" + "BITR" + Convert.ToString(count32);
                                    sw.WriteLine(CANCommand32 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 2), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                    //swCMD.WriteLine(CANCommand32 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 2), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                    //sw.WriteLine("mainpackbit " + MyIni.ReadINI("CANCommands", CANCommand32) + ",1");
                                    if (count32 < 5)
                                    {
                                        //sw.WriteLine("can_r" + Convert.ToString(count32 + 23) + " " + _dictForCommitsPMR[key][j].Remove(0, _dictForCommitsPMR[key][j].IndexOf(";") + 1));
                                        obj.WriteLine("can_r" + Convert.ToString(count32 + 23) + f);
                                    }
                                    else if (count32 > 4 && count32 + 91 < 106)
                                    {
                                        //sw.WriteLine("can32bitr" + Convert.ToString(count32 + 91) + " " + _dictForCommitsPMR[key][j].Remove(0, _dictForCommitsPMR[key][j].IndexOf(";") + 1));
                                        obj.WriteLine("can32bitr" + Convert.ToString(count32 + 91) + f);
                                    }
                                    count32++;
                                    listTags.Add(CANCommand32 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 2), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                    listCommands.Add("mainpackbit " + MyIni.ReadINI("CANCommands", CANCommand32) + ",1");
                                }
                                CAN32ValuesCount++;
                            }
                        }
                        else if (chars.Length == 1 && CAN8ValuesCount < MaxCAN8Values)
                        {
                            string CANCommand8;
                            double a = 2;
                            string charstroke;
                            int minbit; int maxbit; int bitcount;
                            //CANbit = Convert.ToInt32(chars.Length) * 8;
                            if (CheckBox_Filter_PGN.IsChecked == true)
                            {
                                //if (listCAN8.Contains(key.Remove(4)))
                                //{
                                if (IdPath != "")
                                {
                                    CANCommand8 = "CAN" + "8" + "BITR" + Convert.ToString(count8);
                                    sw.WriteLine(CANCommand8 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 6), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                    //swCMD.WriteLine(CANCommand8 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 6), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                    //sw.WriteLine("mainpackbit " + MyIni.ReadINI("CANCommands", CANCommand8) + ",1");
                                    if (count8 < 15)
                                    {
                                        if (_dictForCommitsPMR[key + IdPath][j].Contains("биты"))
                                        {

                                            //sw.WriteLine("can_r" + Convert.ToString(count8) + " " + _dictForCommitsPMR[key + IdPath][j].Remove(0, _dictForCommitsPMR[key + IdPath][j].IndexOf(",") + 2));
                                            f = "";
                                            charstroke = _dictForCommitsPMR[key + IdPath][j].Substring(_dictForCommitsPMR[key + IdPath][j].IndexOf("биты")+5, 4);
                                            charstroke = string.Concat(charstroke.Where(Char.IsDigit));
                                            minbit = Convert.ToInt32(charstroke.Substring(0, 1));
                                            maxbit = Convert.ToInt32(charstroke.Substring(1, 1));
                                            bitcount = maxbit - minbit + 1;
                                            for (int b = 0; b < bitcount; b++)
                                            {
                                                var result = Math.Pow(a, b);
                                                if (result == 1)
                                                {
                                                    f = f + "can_r" + Convert.ToString(count8) + ":" + Convert.ToString(minbit);
                                                }
                                                else
                                                {
                                                    f = f + "+can_r" + Convert.ToString(count8) + ":" + Convert.ToString(minbit) + "*const"+Convert.ToString(result);
                                                }
                                                minbit++;
                                            }
                                            obj.WriteLine(f);

                                            //sw.WriteLine("can_r" + Convert.ToString(count8) + f);
                                        }
                                        else
                                        {
                                            //sw.WriteLine("can_r" + Convert.ToString(count8) + " " + _dictForCommitsPMR[key + IdPath][j].Remove(0, _dictForCommitsPMR[key + IdPath][j].IndexOf(";") + 1));
                                            obj.WriteLine("can_r" + Convert.ToString(count8) + f);

                                        }
                                    }
                                    else if (count8 > 14 && count8 + 1 < 32)
                                    {
                                        if (_dictForCommitsPMR[key + IdPath][j].Contains("биты"))
                                        {
                                            //sw.WriteLine("can8bitr" + Convert.ToString(count8 + 1) + " " + _dictForCommitsPMR[key + IdPath][j].Remove(0, _dictForCommitsPMR[key + IdPath][j].IndexOf(",") + 2));
                                            //sw.WriteLine("can8bitr" + Convert.ToString(count8 + 1) + f);
                                            f = "";
                                            charstroke = _dictForCommitsPMR[key + IdPath][j].Substring(_dictForCommitsPMR[key + IdPath][j].IndexOf("биты") + 5, 4);
                                            charstroke = string.Concat(charstroke.Where(Char.IsDigit));
                                            minbit = Convert.ToInt32(charstroke.Substring(0, 1));
                                            maxbit = Convert.ToInt32(charstroke.Substring(1, 1));
                                            bitcount = maxbit - minbit + 1;
                                            for (int b = 0; b < bitcount; b++)
                                            {
                                                var result = Math.Pow(a, b);
                                                if (result == 1)
                                                {
                                                    f = f + "can8bitr" + Convert.ToString(count8) + ":" + Convert.ToString(minbit);
                                                }
                                                else
                                                {
                                                    f = f + "+can8bitr" + Convert.ToString(count8) + ":" + Convert.ToString(minbit) + "*const" + Convert.ToString(result);
                                                }
                                                minbit++;
                                            }
                                            obj.WriteLine(f);
                                        }
                                        else
                                        {
                                            //sw.WriteLine("can8bitr" + Convert.ToString(count8 + 1) + " " + _dictForCommitsPMR[key + IdPath][j].Remove(0, _dictForCommitsPMR[key + IdPath][j].IndexOf(";") + 1));
                                            obj.WriteLine("can8bitr" + Convert.ToString(count8 + 1) + f);
                                        }
                                    }
                                    count8++;
                                    listTags.Add(CANCommand8 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 6), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                    listCommands.Add("mainpackbit " + MyIni.ReadINI("CANCommands", CANCommand8) + ",1");

                                }
                                else
                                {

                                    CANCommand8 = "CAN" + "8" + "BITR" + Convert.ToString(count8);
                                    sw.WriteLine(CANCommand8 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 6), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                    //swCMD.WriteLine(CANCommand8 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 6), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                    //sw.WriteLine("mainpackbit " + MyIni.ReadINI("CANCommands", CANCommand8) + ",1");
                                    if (count8 < 15)
                                    {
                                        if (_dictForCommitsPMR[key][j].Contains("биты"))
                                        {
                                            //sw.WriteLine("can_r" + Convert.ToString(count8) + " " + _dictForCommitsPMR[key][j].Remove(0, _dictForCommitsPMR[key][j].IndexOf(",") + 2));
                                            //sw.WriteLine("can_r" + Convert.ToString(count8) + f);
                                            f = "";
                                            charstroke = _dictForCommitsPMR[key][j].Substring(_dictForCommitsPMR[key][j].IndexOf("биты") + 5, 4);
                                            charstroke = string.Concat(charstroke.Where(Char.IsDigit));
                                            minbit = Convert.ToInt32(charstroke.Substring(0, 1));
                                            maxbit = Convert.ToInt32(charstroke.Substring(1, 1));
                                            bitcount = maxbit - minbit + 1;
                                            for (int b = 0; b < bitcount; b++)
                                            {
                                                var result = Math.Pow(a, b);
                                                if (result == 1)
                                                {
                                                    f = f + "can_r" + Convert.ToString(count8) + ":" + Convert.ToString(minbit);
                                                }
                                                else
                                                {
                                                    f = f + "+can_r" + Convert.ToString(count8) + ":" + Convert.ToString(minbit) + "*const" + Convert.ToString(result);
                                                }
                                                minbit++;
                                            }
                                            obj.WriteLine(f);
                                        }
                                        else
                                        {
                                            //sw.WriteLine("can_r" + Convert.ToString(count8) + " " + _dictForCommitsPMR[key][j].Remove(0, _dictForCommitsPMR[key][j].IndexOf(";") + 1));
                                            obj.WriteLine("can_r" + Convert.ToString(count8) + f);
                                        }
                                    }
                                    else if (count8 > 14 && count8 + 1 < 32)
                                    {
                                        if (_dictForCommitsPMR[key][j].Contains("биты"))
                                        {
                                            //sw.WriteLine("can8bitr" + Convert.ToString(count8 + 1) + " " + _dictForCommitsPMR[key][j].Remove(0, _dictForCommitsPMR[key][j].IndexOf(",") + 2));
                                            //sw.WriteLine("can8bitr" + Convert.ToString(count8 + 1) + f);
                                            f = "";
                                            charstroke = _dictForCommitsPMR[key][j].Substring(_dictForCommitsPMR[key][j].IndexOf("биты") + 5, 4);
                                            charstroke = string.Concat(charstroke.Where(Char.IsDigit));
                                            minbit = Convert.ToInt32(charstroke.Substring(0, 1));
                                            maxbit = Convert.ToInt32(charstroke.Substring(1, 1));
                                            bitcount = maxbit - minbit + 1;
                                            for (int b = 0; b < bitcount; b++)
                                            {
                                                var result = Math.Pow(a, b);
                                                if (result == 1)
                                                {
                                                    f = f + "can8bitr" + Convert.ToString(count8) + ":" + Convert.ToString(minbit);
                                                }
                                                else
                                                {
                                                    f = f + "+can8bitr" + Convert.ToString(count8) + ":" + Convert.ToString(minbit) + "*const" + Convert.ToString(result);
                                                }
                                                minbit++;
                                            }
                                            obj.WriteLine(f);
                                        }
                                        else
                                        {
                                            //sw.WriteLine("can8bitr" + Convert.ToString(count8 + 1) + " " + _dictForCommitsPMR[key][j].Remove(0, _dictForCommitsPMR[key][j].IndexOf(";") + 1));
                                            obj.WriteLine("can8bitr" + Convert.ToString(count8 + 1) + f);
                                        }
                                    }
                                    count8++;
                                    listTags.Add(CANCommand8 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 6), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                    listCommands.Add("mainpackbit " + MyIni.ReadINI("CANCommands", CANCommand8) + ",1");

                                }

                                //}
                                //else
                                //{
                                //    CANCommand8 = "CAN" + "8" + "BITR" + Convert.ToString(count8);
                                //    sw.WriteLine(CANCommand8 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 6), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                //    swCMD.WriteLine(CANCommand8 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 6), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                //    sw.WriteLine("mainpackbit " + MyIni.ReadINI("CANCommands", CANCommand8) + ",1");
                                //    count8++;
                                //    listCAN8.Add(key.Remove(4));
                                //}

                            }
                            else
                            {
                                CANCommand8 = "CAN" + "8" + "BITR" + Convert.ToString(count8);
                                sw.WriteLine(CANCommand8 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 2), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                //swCMD.WriteLine(CANCommand8 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 2), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                //sw.WriteLine("mainpackbit " + MyIni.ReadINI("CANCommands", CANCommand8) + ",1");
                                if (count8 < 15)
                                {
                                    if (_dictForCommitsPMR[key][j].Contains("биты"))
                                    {
                                        //sw.WriteLine("can_r" + Convert.ToString(count8) + " " + _dictForCommitsPMR[key][j].Remove(0, _dictForCommitsPMR[key][j].IndexOf(",") + 2));
                                        //sw.WriteLine("can_r" + Convert.ToString(count8) + f);
                                        f = "";
                                        charstroke = _dictForCommitsPMR[key][j].Substring(_dictForCommitsPMR[key][j].IndexOf("биты") + 5, 4);
                                        charstroke = string.Concat(charstroke.Where(Char.IsDigit));
                                        minbit = Convert.ToInt32(charstroke.Substring(0, 1));
                                        maxbit = Convert.ToInt32(charstroke.Substring(1, 1));
                                        bitcount = maxbit - minbit + 1;
                                        for (int b = 0; b < bitcount; b++)
                                        {
                                            var result = Math.Pow(a, b);
                                            if (result == 1)
                                            {
                                                f = f + "can_r" + Convert.ToString(count8) + ":" + Convert.ToString(minbit);
                                            }
                                            else
                                            {
                                                f = f + "+can_r" + Convert.ToString(count8) + ":" + Convert.ToString(minbit) + "*const" + Convert.ToString(result);
                                            }
                                            minbit++;
                                        }
                                        obj.WriteLine(f);
                                    }
                                    else
                                    {
                                        //sw.WriteLine("can_r" + Convert.ToString(count8) + " " + _dictForCommitsPMR[key][j].Remove(0, _dictForCommitsPMR[key][j].IndexOf(";") + 1));
                                        obj.WriteLine("can_r" + Convert.ToString(count8) + f);
                                    }
                                }
                                else if (count8 > 14 && count8 + 1 < 32)
                                {
                                    if (_dictForCommitsPMR[key][j].Contains("биты"))
                                    {
                                        //sw.WriteLine("can8bitr" + Convert.ToString(count8 + 1) + " " + _dictForCommitsPMR[key][j].Remove(0, _dictForCommitsPMR[key][j].IndexOf(",") + 2));
                                        //sw.WriteLine("can8bitr" + Convert.ToString(count8 + 1) + f);
                                        f = "";
                                        charstroke = _dictForCommitsPMR[key][j].Substring(_dictForCommitsPMR[key][j].IndexOf("биты") + 5, 4);
                                        charstroke = string.Concat(charstroke.Where(Char.IsDigit));
                                        minbit = Convert.ToInt32(charstroke.Substring(0, 1));
                                        maxbit = Convert.ToInt32(charstroke.Substring(1, 1));
                                        bitcount = maxbit - minbit + 1;
                                        for (int b = 0; b < bitcount; b++)
                                        {
                                            var result = Math.Pow(a, b);
                                            if (result == 1)
                                            {
                                                f = f + "can8bitr" + Convert.ToString(count8) + ":" + Convert.ToString(minbit);
                                            }
                                            else
                                            {
                                                f = f + "+can8bitr" + Convert.ToString(count8) + ":" + Convert.ToString(minbit) + "*const" + Convert.ToString(result);
                                            }
                                            minbit++;
                                        }
                                        obj.WriteLine(f);
                                    }
                                    else
                                    {
                                        //sw.WriteLine("can8bitr" + Convert.ToString(count8 + 1) + " " + _dictForCommitsPMR[key][j].Remove(0, _dictForCommitsPMR[key][j].IndexOf(";") + 1));
                                        obj.WriteLine("can8bitr" + Convert.ToString(count8 + 1) + f);
                                    }
                                }
                                count8++;
                                listTags.Add(CANCommand8 + " " + Convert.ToString(Convert.ToInt32(key.Remove(0, 2), 16)) + "," + Convert.ToString(Convert.ToInt32(chars.Substring(0, 1)) - 1) + ",0,0,,0,0,0,0");
                                listCommands.Add("mainpackbit " + MyIni.ReadINI("CANCommands", CANCommand8) + ",1");
                            }
                            CAN8ValuesCount++;
                        }
                        sw.WriteLine();
                        //lastIndex = message.IndexOf("\r\n", firstIndex);

                        //string stroke = message.Substring(firstIndex, lastIndex - firstIndex);

                        //sw.WriteLine("CAN" + );
                        //string number = ((string)_dictForCommitsPMR[key][j].SkipWhile(c => !char.IsDigit(c)).TakeWhile(c => char.IsDigit(c)));
                        //sw.WriteLine("C/*AN")*/
                    }
                    sw.WriteLine();
                    sw.WriteLine();
                }
                //string IdAdress = _dictForCommitsPMR.Keys[i];
            }
            sw.Close();
            obj.Close();
            swCMD.WriteLine();
            for (int i = 0; i < listCommands.Count; i++)
            {
                swCMD.WriteLine(listCommands[i]);
            }
            swCMD.WriteLine();
            for (int i = 0; i < listTags.Count; i++)
            {
                swCMD.WriteLine(listTags[i]);
            }
            swCMD.Close();
            //}
        }

        private void LB_Uniq_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            //LB_Uniq.SelectionChanged += Lb_Uniq_SelectionChanged;
        }

        private void CB_FilterOneByte_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //CB_FilterOneByte.SelectionChanged += CB_FilterOneByte_SelectionChanged;
        }

        private void WindowSettings_Closed(object sender, EventArgs e)
        {
            var MyIni = new IniFiles("Settings.ini");
            //WindowSettings.Height = Convert.ToDouble(MyIni.ReadINI("Window", "Height"));
            //WindowSettings.Width = Convert.ToDouble(MyIni.ReadINI("Window", "Width"));
            MyIni.Write("Window", "Height", Convert.ToString(WindowSettings.Height));
            MyIni.Write("Window", "Width", Convert.ToString(WindowSettings.Width));
            MyIni.Write("Window", "Left", Convert.ToString(WindowSettings.Left));
            MyIni.Write("Window", "Top", Convert.ToString(WindowSettings.Top));
            //MyIni.Write("Window", "WindowState", Convert.ToString(WindowSettings.WindowState));
        }

        //private void CB_FilterOneByte_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (TabItemOneByte.IsSelected == true)
        //    {
        //        plotByte0.Visibility = Visibility.Hidden;
        //        plotByte1.Visibility = Visibility.Hidden;
        //        plotByte2.Visibility = Visibility.Hidden;
        //        plotByte3.Visibility = Visibility.Hidden;
        //        plotByte4.Visibility = Visibility.Hidden;
        //        plotByte5.Visibility = Visibility.Hidden;
        //        plotByte6.Visibility = Visibility.Hidden;
        //        plotByte7.Visibility = Visibility.Hidden;
        //        if (CB_FilterOneByte.SelectedItem != null)
        //        {
        //            _itemByte0Selected = (string)CB_FilterOneByte.SelectedItem;

        //        }
        //        //TabItemOneByte.Refresh();
        //        string msgId = (string)LB_Uniq.SelectedItem;
        //        LB_Uniq.SelectedIndex = -1;
        //        plotByte0.Visibility = Visibility.Visible;
        //        plotByte1.Visibility = Visibility.Visible;
        //        plotByte2.Visibility = Visibility.Visible;
        //        plotByte3.Visibility = Visibility.Visible;
        //        plotByte4.Visibility = Visibility.Visible;
        //        plotByte5.Visibility = Visibility.Visible;
        //        plotByte6.Visibility = Visibility.Visible;
        //        plotByte7.Visibility = Visibility.Visible;
        //        LB_Uniq.SelectedItem = msgId;
        //    }
        //}


        //if (TabItemOneByte.IsSelected = true)
        //{
        //    var grid = new Grid();
        //    grid.InvalidateVisual();
        //}

    }

}

