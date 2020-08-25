using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using MahApps.Metro.Controls;

namespace BiosFix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///

    public partial class MainWindow
    {

        public static readonly DependencyProperty CurrentStatus = 
            DependencyProperty.Register("State",typeof(string), typeof(MainWindow));

        public string State
        {
            get => this.GetValue(CurrentStatus) as string;
            set => this.SetValue(CurrentStatus, value);
        }

    public MainWindow()
        {
            this.DataContext = this;
            if (!File.Exists("DnSmbios64.exe"))
            {
                State = "Unable to find required DNSMBios executable";
            }

            if (!File.Exists("UsrIdent64.exe"))
            {
                State = "Unable to find required UsrIdent64.exe executable";
            }
        }

        private void Write_OnClick(object sender, RoutedEventArgs e)
        {
            var textDoc = new List<string>();
            
            foreach (var elem in FileText.GetChildObjects())
            {
                switch (elem.GetType().ToString())
                {
                    case "System.Windows.Documents.Run":
                    {
                        var run = elem as Run; 
                        textDoc.Add(run?.Text);
                        break;
                    }
                    case "System.Windows.Documents.LineBreak":
                        textDoc.Add(Environment.NewLine);
                        break;
                }
            }
            var textRaw = String.Join(string.Empty, textDoc);
            File.Delete("SMBIOIN.txt");
            File.WriteAllText("SMBIOIN.txt", textRaw);
            var startInfo = new ProcessStartInfo
            {
                CreateNoWindow = true,
                Verb = "runas",
                FileName = @"DnSmbios64.exe",
                Arguments = "FieldService",
                RedirectStandardOutput = true
            };
            var process = Process.Start(startInfo);
            Output.Text = process?.StandardOutput.ReadToEnd();
            process?.WaitForExit();
        }
        
        private void ExecuteChecksumRepair_OnClick(object sender, RoutedEventArgs e)
        {
            var startInfo = new ProcessStartInfo
            {
                Verb = "runas",
                CreateNoWindow = true,
                FileName = @"DnSmbios64.exe",
                Arguments = "ChecksumRepair",
                RedirectStandardOutput = true
            };

            var process = Process.Start(startInfo);
            Output.Text = process?.StandardOutput.ReadToEnd();

            process?.WaitForExit();
        }
        
        private void ExecuteStructRepair_OnClick(object sender, RoutedEventArgs e)
        {
            var startInfo = new ProcessStartInfo
            {
                Verb = "runas",
                CreateNoWindow = true,
                FileName = @"DnSmbios64.exe",
                Arguments = "StructRepair",
                RedirectStandardOutput = true
            };

            var process = Process.Start(startInfo);
            Output.Text = process?.StandardOutput.ReadToEnd();

            process?.WaitForExit();
        }
        
    }

    public class DriverUnavailable : Exception
    {
        public override string Message => "Driver not installed or version is not supported. Minimum tested version is __EPOS 2.0.0.4-1__";
    }

    public class DnSmbiosNotFound : Exception
    {
        public override string Message => $"DnSmbios not found. Drop into {System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly()?.Location)} or add to path";
    }

    public class UsrIdent64NotFound : Exception
    {
        public override string Message =>
            $"UsrIdent64 not found. Drop into {System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly()?.Location)} or add to path";
    }

}
