using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SmartHDD;

namespace BiosFix
{
    /// <summary>
    /// Interaction logic for HardDrive.xaml
    /// </summary>
    public partial class HardDrive
    {
        public SmartHDD.HDD SelectedHdd { get; set; }

        public HardDrive()
        {
            InitializeComponent();
        }

        private void HardDrive_OnLoaded(object sender, RoutedEventArgs e)
        {



            var MyStat = new List<SmartHDD.HDD>();
            MyStat.Add(SelectedHdd);
            DataStats.ItemsSource = MyStat;

            var MySmart = new List<smart>();
            foreach (var r in SelectedHdd.Attributes)
            {

                if (r.Value.HasData)
                {
                    MySmart.Add(new smart
                    {
                        Attr = r.Value.Attribute,
                        Pass = r.Value.Threshold < r.Value.Current || r.Value.Threshold < r.Value.Worst,
                        Current = r.Value.Current,
                        Worst = r.Value.Worst,
                        Threshold = r.Value.Threshold
                    });
                }

            }

            SmartStats.ItemsSource = MySmart;
        }

        public class smart
        {
            public string Attr { get; set; }
            public bool Pass { get; set; } 
            public int Current { get; set; }
            public int Worst { get; set; }
            public int Threshold { get; set; }

        }
    }
}
