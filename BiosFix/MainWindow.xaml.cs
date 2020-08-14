using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Documents;
using MahApps.Metro.Controls;

namespace BiosFix
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            this.DataContext = this;
            if (!File.Exists("DnSmbios64.exe"))
            {
                MessageBox.Show("Unable to find required DNSMBios executable");
                //throw new DnSmbiosNotFound();
            }

            if (!File.Exists("UsrIdent64.exe"))
            {
                MessageBox.Show("Unable to find required UsrIdent64.exe executable");
                //throw new UsrIdent64NotFound();
            }
        }

        private void Write_OnClick(object sender, RoutedEventArgs e)
        {
            var TextDoc = new List<string>();
            
            foreach (var elem in FileText.GetChildObjects())
            {
               
                if (elem.GetType().ToString() == "System.Windows.Documents.Run")
                {
                    var run = elem as Run; 
                    TextDoc.Add(run.Text);
                }
                else if (elem.GetType().ToString() == "System.Windows.Documents.LineBreak")
                {
                    TextDoc.Add(Environment.NewLine);
                }
            }
            var TextRaw = String.Join(String.Empty, TextDoc);
            File.Delete("SMBIOIN.txt");
            File.WriteAllText("SMBIOIN.txt", TextRaw);
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.Verb = "runas";
            startInfo.FileName = @"DnSmbios64.exe";
            startInfo.Arguments = "FieldService";
            startInfo.RedirectStandardOutput = true;
            var process = Process.Start(startInfo);
            Output.Text = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
        }
        
        private void ExecuteChecksumRepair_OnClick(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Verb = "runas";
            startInfo.CreateNoWindow = true;
            startInfo.FileName = @"DnSmbios64.exe";
            startInfo.Arguments = "ChecksumRepair";
            startInfo.RedirectStandardOutput = true;

            var process = Process.Start(startInfo);
            Output.Text = process.StandardOutput.ReadToEnd();

            process.WaitForExit();
        }
        
        private void ExecuteStructRepair_OnClick(object sender, RoutedEventArgs e)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.Verb = "runas";
            startInfo.CreateNoWindow = true;
            startInfo.FileName = @"DnSmbios64.exe";
            startInfo.Arguments = "StructRepair";
            startInfo.RedirectStandardOutput = true;

            var process = Process.Start(startInfo);
            Output.Text = process.StandardOutput.ReadToEnd();

            process.WaitForExit();
        }
        
    }

    public class DriverUnavailable : Exception
    {
        public override string Message => "Driver not installed or version is not supported. Minimum tested version is __EPOS 2.0.0.4-1__";
    }

    public class DnSmbiosNotFound : Exception
    {
        public override string Message => $"DnSmbios not found. Drop into {System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)} or add to path";
    }

    public class UsrIdent64NotFound : Exception
    {
        public override string Message =>
            $"UsrIdent64 not found. Drop into {System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location)} or add to path";
    }

}
