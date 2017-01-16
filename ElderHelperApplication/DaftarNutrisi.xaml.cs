using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace ElderHelperApplication
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class DaftarNutrisi : Page
    {
        public DaftarNutrisi()
        {
            this.InitializeComponent();
        }

        private void mySearchBox_QuerySubmitted(SearchBox sender, SearchBoxQuerySubmittedEventArgs args)
        {
            string XMLFilePath = Path.Combine(Package.Current.InstalledLocation.Path, "Nutrition.xml");
            XDocument loadedData = XDocument.Load(XMLFilePath);

            var data = from query in loadedData.Descendants("food")
                       where query.Element("makanan").Value.Contains((mySearchBox.QueryText).ToLower())
                       select new Food
                       {
                           Makanan = (string)query.Element("makanan")
                       };
            FoodlistBox.ItemsSource = data;

            var data2 = from query in loadedData.Descendants("food")
                       where query.Element("makanan").Value.Contains((mySearchBox.QueryText).ToLower())
                       select new Food
                       {                          
                           Protein = (int)query.Element("protein")
                       };
            FoodlistBox2.ItemsSource = data2;

            var data3 = from query in loadedData.Descendants("food")
                        where query.Element("makanan").Value.Contains((mySearchBox.QueryText).ToLower())
                        select new Food
                        {
                            Karbohidrat = (int)query.Element("karbohidrat")
                        };
            FoodlistBox3.ItemsSource = data3;

            var data4 = from query in loadedData.Descendants("food")
                        where query.Element("makanan").Value.Contains((mySearchBox.QueryText).ToLower())
                        select new Food
                        {
                            Lemak = (int)query.Element("lemak")
                        };
            FoodlistBox4.ItemsSource = data4;
        }

        public class Food
        {
            string makanan;
            int karbohidrat;
            int protein;
            int lemak;

            public string Makanan
            {
                get { return makanan; }
                set { makanan = value; }
            }
            public int Karbohidrat
            {
                get { return karbohidrat; }
                set { karbohidrat = value; }
            }
            public int Protein
            {
                get { return protein; }
                set { protein = value; }
            }
            public int Lemak
            {
                get { return lemak; }
                set { lemak = value; }
            }
        }
    }
}
