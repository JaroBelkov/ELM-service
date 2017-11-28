using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Windows;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Collections;
using System.Threading;

namespace Euston_Leisure_Messaging_Service
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string main_path = Directory.GetCurrentDirectory();
        public static string main_fileName;
        public static Dictionary<string, string> abbreviations = new Dictionary<string, string>();
        public static StreamReader incoming_file;
        public static int line_counter;

        public MainWindow()
        {
            InitializeComponent();

            //Check installation and, btw, initialize dictionary 'abbreviations'.
            try
            {
                string file = main_path + "\\Newtonsoft.Json.dll";
                if (!File.Exists(file))
                {
                    throw new FileNotFoundException("Could not find Newtonsoft.Json.dll in current directory.");
                }
                StreamReader sr = new StreamReader(main_path + "\\textwords.csv");
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    var values = line.Split(',');
                    abbreviations.Add(values[0], values[0] + " <" + values[1] + ">");
                }
            }
            catch (Exception exc)
            {
                OutputText.Foreground = System.Windows.Media.Brushes.Red;
                OutputText.Text = exc.Message + "\n\nOoops, it seems like some files are missing or broken. Please, fix it and restart the application.";
                return;
            }

            // Necessary files are present, now allow to find files with incoming messages and with processed messages.
            RealTime.IsEnabled = true;
            ManualProcess.IsEnabled = true;
            ChooseFileToProcess.IsEnabled = true;
            ChooseDirToShow.IsEnabled = true;
            ShowSingleMessage.IsEnabled = true;
        }

        private void ChooseFileToProcess_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog            // Choosing file with incoming messages
            {
                InitialDirectory = main_path,
                Filter = "json files (*.json)|*.json",
                RestoreDirectory = true
            };

            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)       // if file was actually chosen, prepare it for processing   
            {
                main_path = file.FileName.Replace(file.SafeFileName, "");               // exctracts path without filename
                main_fileName = Path.GetFileNameWithoutExtension(file.SafeFileName);    // exctracts filename without extension (that is .json),
                                                                                        // and also used as directory name for processed messages
                TruncatePath display_path = new TruncatePath(file);                     // Truncates path if too long for the label
                CurrentFile.Content = display_path.Path;                                // Display path in the label
                ProcessCurrentFile.IsEnabled = true;                                    // Enable the rest of elements
                Trending.IsEnabled = true;
                Mentions.IsEnabled = true;
                Sir.IsEnabled = true;

                // check, if the file was processed before, and if not, create directory and empty first files for it.
                if (!Directory.Exists(main_path + main_fileName))
                {
                    Directory.CreateDirectory(main_path + main_fileName);
                    StreamWriter sw = new StreamWriter(main_path + main_fileName + "\\000000000.json", false, Encoding.ASCII);
                    sw.WriteLine("Null");
                    sw.Close();
                    File.CreateText(main_path + main_fileName + "\\trending.txt").Close();
                    File.CreateText(main_path + main_fileName + "\\mentions.txt").Close();
                    File.CreateText(main_path + main_fileName + "\\sir_list.txt").Close();
                }
                else  // if it was processed before, populate trending, mentions and SIR lists
                {
                    TrendingText.Text = File.ReadAllText(main_path + main_fileName + "\\trending.txt");
                    MentionsText.Text = File.ReadAllText(main_path + main_fileName + "\\mentions.txt");
                    SirText.Text = File.ReadAllText(main_path + main_fileName + "\\sir_list.txt");
                }

                // open Stream and read the first line (it is always Null)
                StreamReader incoming_file = new StreamReader(main_path + main_fileName + ".json");
                string line = incoming_file.ReadLine();
                line_counter = 0;

                // File is ready to be processed. Return.
            }

            // else: the user did not choose a file. Return with elements disabled.
        }

        private void ProcessCurrentFile_Click(object sender, RoutedEventArgs e)
        {
            string line;
            if ((line = incoming_file.ReadLine()) != null){
                line_counter++;

                try
                {
                    var record = JsonConvert.DeserializeObject<Dictionary<string, object>>(line);
                    string header = record["Header"].ToString();
                    if (!File.Exists(main_path + main_fileName + "\\" + header + ".json"))
                    {
                        ProcessSingleMessage message = new ProcessSingleMessage(record);

                    }
                    else if (ManualProcess.IsChecked == true)
                    {
                        // TO DO - show from existing file
                    }
                    else
                    {
                        // skip
                    }

                }
                catch (Exception exc)
                {
                    var answer =  System.Windows.Forms.MessageBox.Show("Ooops, file" + main_fileName + ".json seems to be broken on the line number " + line_counter.ToString() + ". Processing will continue on the next line, unless cancelled.", exc.Message, MessageBoxButtons.OKCancel, MessageBoxIcon.Error);
                    if (answer == System.Windows.Forms.DialogResult.Cancel)
                    {
                        return;
                    }
                }


            }

        }
            
    }

    
}
