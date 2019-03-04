using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.WindowsAzure.MobileServices;
using System.Collections.ObjectModel;
using Syncfusion.UI.Xaml.Maps;
using System.Threading.Tasks;
using System.ComponentModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Niti_Pune_Water
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        static ObservableCollection<DataTest> dataList = new ObservableCollection<DataTest>();

        public event PropertyChangedEventHandler PropertyChanged;
        internal void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        public List<DataTempClass> testList { get; set; }
        public float AvgTemp { get; set; }
        public string totalCoins { get; set; }
        public int ttlCoins { get; set; }
        public string avgTemp { get; set; }

        public MainPage()
        {
 
            this.InitializeComponent();
            avgTemp = AvgTemp / dataList.Count + " C";
            listView.ItemsSource = dataList;
        }

    private async void Button_Click(object sender, RoutedEventArgs e)
        {
            dataList.Clear();
            AvgTemp = 0;
            ttlCoins = 0;
            var client = new MobileServiceClient("https://iotwater.azurewebsites.net");
            IMobileServiceTable<logs> itemTable = client.GetTable<logs>();
            List<logs> items = await itemTable.ToListAsync();
            List<logs> dt = new List<logs>();
            foreach(logs it in items)
            {
                DataTest d = new DataTest();
                d.Coins = it.coins.ToString() + " Coins";
                d.Temp = "Temp: " + it.temperature; ;
                d.Time = "Time: " + it.createdat.Remove(16);
                ttlCoins = ttlCoins + int.Parse(it.coins.ToString());
                totalCoins = (ttlCoins + int.Parse(it.coins.ToString())).ToString()+" Coins";
                AvgTemp = AvgTemp + it.temperature;
                dataList.Add(d);
            }
            avgTemp = AvgTemp / dataList.Count + " C";
            coinsBlock.Text = ttlCoins.ToString();
            tempBlock.Text = avgTemp;
            totalCoinBlock.Text = (ttlCoins).ToString();
            listView.ItemsSource = dataList;
            dataList.CollectionChanged += DataList_CollectionChanged;
        }

        private void DataList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {

        }

        private async Task lolFunc()
        {
            var client = new MobileServiceClient("https://iotwater.azurewebsites.net");
            IMobileServiceTable<Message> itemTable = client.GetTable<Message>();
            List<Message> items = await itemTable.ToListAsync();
            List<DataTempClass> dt = new List<DataTempClass>();
            foreach (Message it in items)
            {
                DataTempClass temp = new DataTempClass();
                temp.Latitude = it.latitude + 'N';
                temp.Longitude = it.longitude.ToString() + 'E';
                dt.Add(temp);
            }
            testList = dt;
        }
        
        void loadData()
        {
            DataTest d = new DataTest();
            d.Coins = "10";
            d.Temp = "Temp: 27 C";
            d.Time = "Time: " + DateTime.Now.TimeOfDay.ToString().Remove(5);
            d.avgTemp = "21";
            d.totalCoins = "13";
            dataList.Add(d);
        }
    }
    class DataTest
    {
        public string Coins { get; set; }
        public string Time { get; set; }
        public string Temp { get; set; }
        public string avgTemp { get; set; }
        public string totalCoins { get; set; }

    }

}

