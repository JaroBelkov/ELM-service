using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
//using System.Diagnostics;

namespace Message_Editor
{
    public partial class MainWindow : Window
    {
        public static string message_type;
        public static string main_path = Directory.GetCurrentDirectory();
        public static string my_database_path = main_path + "\\db.json";

        public MainWindow()
        {
            InitializeComponent();
            message_type = "SMS";
            SMS.IsChecked = true;
            Editor.Activate();
            
            // Search the "database" for the last ID and btw check the installation
            string last_line = "";
            while (last_line == "")
            {
                try
                {
                    last_line = File.ReadLines(my_database_path).Last();
                    if (last_line != "Null")
                    {
                        var try_record = JsonConvert.DeserializeObject<Dictionary<string, object>>(last_line);
                        string try_ID = try_record["Header"].ToString();
                    }
                }
                catch (Exception exc)
                {
                    var answer = System.Windows.Forms.MessageBox.Show("Could not find db.json or it is broken.\nWould you like to to open file dialog to specify the path?\nOr create and/or fix the file, and then click 'No' to retry.\nOr click 'Cancel' to create a new db.json with Null. WARNING: if db.json exists in current directory, it will be overridden!!", "Exception: " + exc.Message, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Error);
                    if (answer == System.Windows.Forms.DialogResult.Cancel)
                    {
                        last_line = "Null";
                        StreamWriter sw = new StreamWriter(my_database_path, false, Encoding.ASCII);
                        sw.WriteLine("Null");
                        sw.Close();
                    }
                    else if(answer == System.Windows.Forms.DialogResult.Yes)
                    {
                        OpenFileDialog file = new OpenFileDialog();
                        file.InitialDirectory = main_path;
                        file.Filter = "json files (*.json)|*.json";
                        file.RestoreDirectory = true;
                        if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        {
                            my_database_path = file.FileName;
                        }
                        
                        last_line = "";
                    }
                    else
                    {
                        last_line = "";
                    }
                }
            }

            string current_ID = "000000001";
            if (last_line != "Null")
            {
                var last_record = JsonConvert.DeserializeObject<Dictionary<string, object>>(last_line);
                string last_ID = last_record["Header"].ToString();
                current_ID = (int.Parse(last_ID) + 1).ToString("D9");
            }

            TextInput_Header.Text = current_ID;
        }

        public static bool EmailIsValid(string email)    // "Stolen" from stackoverflow.com
        {
            string expresion;
            expresion = "\\w+([-+.']\\w+)*@\\w+([-.]\\w+)*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(email, expresion))
            {
                if (Regex.Replace(email, expresion, string.Empty).Length == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        // Phone numbers and Sport Centres Codes are numbers only
        private string ExctractNumbersOnly(string anyString)
        {
            string newString = "";
            for (byte i = 0; i < anyString.Length; i++)
            {

                if (anyString[i] >= '0' && anyString[i] <= '9')
                {
                    newString += anyString[i];
                }
            }
            return newString;
        }

        // Called after click to SMS or Tweet radio-button
        private void NoSubject(String my_sign)
        {
            TextInput_Subject.Width = 323;
            TextInput_Subject.Text = "n/a";
            TextInput_Subject.IsEnabled = false;
            TextInput_Sender.Text = my_sign;                 // International phone number starts with '+', Twitter account with '@'
            TextInput_Sender.Select(1, 0);
            TextInput_Sender.Focus();
            TextInput_Sender.MaxLength = 16;                 // max length of int. phone number and twitter ID
            TextInput_Message.Height = 465;                  // Covers inputs for SIR E-mail
        }

        //Three following are to happen: on radio-button change
        //      / 1st - SMS
        private void SMS_Checked(object sender, RoutedEventArgs e)       // 1. SMS cannot have 'subject'.
        {
            TextInput_Subject.BorderBrush = System.Windows.Media.Brushes.LightGray;
            message_type = "SMS";
            NoSubject("+");  //  SMS cannot have 'subject'.
            TextInput_Message.MaxLength = 140;  // Maximum length of the message
            if (TextInput_Message.Text.Length > 140)   // if was filled in previuously: shorten
            {
                TextInput_Message.Text = TextInput_Message.Text.Substring(0, 139);
            }
        }

        //      / 2nd - Tweet
        private void Tweet_Checked(object sender, RoutedEventArgs e)
        {
            TextInput_Subject.BorderBrush = System.Windows.Media.Brushes.LightGray;
            message_type = "Tweet";
            NoSubject("@");  //  Tweet cannot have 'subject'.
            TextInput_Message.MaxLength = 140;
            if (TextInput_Message.Text.Length > 140)
            {
                TextInput_Message.Text = TextInput_Message.Text.Substring(0, 139);
            }

        }

        // 3rd -  E-mail 
        private void Email_Checked(object sender, RoutedEventArgs e)
        {
            message_type = "SIR";               // at first switches to SIR
            TextInput_Subject.Text = "SIR " + DateTime.Today.ToString(format: "dd/MM/yy");  // pre-populate for SIR
            TextInput_Subject.IsEnabled = true;
            TextInput_Subject.Width = 124;                   // allow to enter Nature of Accident
            TextInput_Sender.Text = "@";                     // pre-populate 'sender' field fol email entry
            TextInput_Sender.Select(0, 0);
            TextInput_Sender.Focus();
            TextInput_Sender.MaxLength = 254;                // max length of email address
            TextInput_Message.Height = 386;                  // uncover inputs for SIR
            TextInput_Message.MaxLength = 1028;              // maximum length of the message

        }

        // If it is not SIR: delete pre-poluated
        private void TextInput_Subject_GotFocus(object sender, RoutedEventArgs e)
        {
            if (message_type == "SIR")
            {
                message_type = "Email";
                TextInput_Subject.Text = "";
                TextInput_Subject.Width = 482;
                TextInput_Message.Height = 465;       // cover inputs for SIR
            }
        }

        // Make sure no delimiter is in the 'Subject'
        private void TextInput_Subject_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TextInput_Subject.BorderBrush = System.Windows.Media.Brushes.LightGray;
            while (TextInput_Subject.Text.Contains(";;"))                      // double-semicolon is delimiter, thus forbidden
            {
                TextInput_Subject.Text = TextInput_Sender.Text.Replace(";;", ";");
            }
        }

        //Entering Sport Centre Code:

        //     / Clear numbers onFocus:
        private void TextInput_SportCentreCode1_GotFocus(object sender, RoutedEventArgs e) => TextInput_SportCentreCode1.Text = "";
        private void TextInput_SportCentreCode2_GotFocus(object sender, RoutedEventArgs e) => TextInput_SportCentreCode2.Text = "";
        private void TextInput_SportCentreCode3_GotFocus(object sender, RoutedEventArgs e) => TextInput_SportCentreCode3.Text = "";

        //      / Exctract numbers only and
        //      / when fields are completed, move cursor from 1st field to 2nd, then from 2nd to 3rd:
        private void TextInput_SportCentreCode1_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TextInput_SportCentreCode1.Text = ExctractNumbersOnly(TextInput_SportCentreCode1.Text);
            TextInput_SportCentreCode1.Select(TextInput_SportCentreCode1.Text.Length, 0);
            TextInput_SportCentreCode1.Focus();

            if (TextInput_SportCentreCode1.Text.Length > 1)
            {
                TextInput_SportCentreCode2.Focus();
            }
        }

        private void TextInput_SportCentreCode2_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TextInput_SportCentreCode2.Text = ExctractNumbersOnly(TextInput_SportCentreCode2.Text);
            TextInput_SportCentreCode2.Select(TextInput_SportCentreCode2.Text.Length, 0);
            TextInput_SportCentreCode2.Focus();

            if (TextInput_SportCentreCode2.Text.Length > 2)
            {
                TextInput_SportCentreCode3.Focus();
            }
        }

        private void TextInput_SportCentreCode3_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TextInput_SportCentreCode3.Text = ExctractNumbersOnly(TextInput_SportCentreCode3.Text);
            TextInput_SportCentreCode3.Select(TextInput_SportCentreCode3.Text.Length, 0);
            TextInput_SportCentreCode3.Focus();
        }

        // Editing 'Sender'
        private void TextInput_Sender_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            TextInput_Sender.BorderBrush = System.Windows.Media.Brushes.LightGray;

            if (message_type == "SMS")
            {
                if (TextInput_Sender.Text.Length > 1)
                {
                    TextInput_Sender.Text = "+" + ExctractNumbersOnly(TextInput_Sender.Text.Substring(1));
                    TextInput_Sender.Select(TextInput_Sender.Text.Length, 0);
                    TextInput_Sender.Focus();
                }
                else
                {
                    TextInput_Sender.Text = "+";
                    TextInput_Sender.Select(1, 0);
                    TextInput_Sender.Focus();
                }
            }

            if (message_type == "Tweet")
            {
                string new_string = "";
                for (byte i = 1; i < TextInput_Sender.Text.Length; i++)
                {
                    if ((TextInput_Sender.Text[i] >= '0' && TextInput_Sender.Text[i] <= '9') || (TextInput_Sender.Text[i] >= 'a' && TextInput_Sender.Text[i] <= 'z') || (TextInput_Sender.Text[i] >= 'A' && TextInput_Sender.Text[i] <= 'Z') || TextInput_Sender.Text[i] == '_')
                    {
                        new_string += TextInput_Sender.Text[i];
                    }
                }
                TextInput_Sender.Text = "@" + new_string;


                TextInput_Sender.Select(TextInput_Sender.Text.Length, 0);
                TextInput_Sender.Focus();
            }
        }

        // Incorrect 'Sender' format?
        public void TextInput_Sender_LostFocus(object sender, RoutedEventArgs e)
        {


            if (message_type == "SMS" && TextInput_Sender.Text.Length < 13)    // Intl. phone number must be at least 13 chars (including '+')
            {
                TextInput_Sender.BorderBrush = System.Windows.Media.Brushes.Red;
            }

            if (message_type == "Tweet" && TextInput_Sender.Text.Length < 4)  // Twitter ID is at least 4 chars long (incl. '@')
            {
                TextInput_Sender.BorderBrush = System.Windows.Media.Brushes.Red;
            }

            if ((message_type == "Email" || message_type == "SIR") && !EmailIsValid(TextInput_Sender.Text))  // Must be valid email address
            {
                TextInput_Sender.BorderBrush = System.Windows.Media.Brushes.Red;
            }
        }

        // Make sure no delimiter is in the message text (body):
        private void TextInput_Message_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            while (TextInput_Message.Text.Contains(";;"))
            {
                TextInput_Message.Text = TextInput_Sender.Text.Replace(";;", ";");
            }
        }

        // The user has finished input, wants to save to the file (database):

        private void ButtonInput_Save_Click(object sender, RoutedEventArgs e)
        {
            if (TextInput_Message.Text.Length < 1)      // At least one character must be in a message
            {
                System.Windows.Forms.MessageBox.Show("Please, write at least one character to the main 'Message' box, and press the button 'Save' again.", "No message body", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (message_type == "SMS" && TextInput_Sender.Text.Length < 13)    // Intl. phone number must be at least 13 chars (including '+')
            {
                TextInput_Sender.BorderBrush = System.Windows.Media.Brushes.Red;
                System.Windows.Forms.MessageBox.Show("Internatioanl phone number begins with '+' (instead of '00') and comprises of twelve to fifteen numbers\nPlease, correct, and press the button 'Save' again.", "Intl. phone number too short.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (message_type == "Tweet" && TextInput_Sender.Text.Length < 4)  // Twitter ID is at least 4 chars long (incl. '@')
            {
                TextInput_Sender.BorderBrush = System.Windows.Media.Brushes.Red;
                System.Windows.Forms.MessageBox.Show("Twitter ID (username) begins with '@' and is at least 4 characters long.\nPlease, correct, and press the button 'Save' again.", "Twitter ID too short.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }



            if (message_type == "Email" && TextInput_Subject.Text.Length < 20)
            {
                TextInput_Subject.BorderBrush = System.Windows.Media.Brushes.Red;
                System.Windows.Forms.MessageBox.Show("'Subject' in E-mail must contain at least twenty characters.\nPlease, correct, and press the button 'Save' again.", "Subject too short", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (message_type == "SIR")
            {
                if (TextInput_SportCentreCode1.Text.Length + TextInput_SportCentreCode2.Text.Length + TextInput_SportCentreCode3.Text.Length < 7)
                {
                    System.Windows.Forms.MessageBox.Show("Sport Centre Code must be in format XX-XXX-XX.\nPlease, correct, and press the button 'Save' again.", "Invalid Sport Centre Code.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                if (ComboInput_NatureOfIncident.SelectedIndex == 0)
                {
                    System.Windows.Forms.MessageBox.Show("Please, select the nature of the incident, and press the button 'Save' again.", "Nature of Incident not selected.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            Dictionary<string, object> incoming_message = new Dictionary<string, object>();
            Dictionary<string, string> my_body = new Dictionary<string, string>();

            incoming_message.Add("Header", TextInput_Header.Text);
            my_body.Add("Sender", TextInput_Sender.Text);

            if (message_type == "Email" || message_type == "SIR")
            {
                if (!EmailIsValid(TextInput_Sender.Text))
                {
                    // Must be valid email address
                    TextInput_Sender.BorderBrush = System.Windows.Media.Brushes.Red;
                    System.Windows.Forms.MessageBox.Show("Invalid E-mail address. Please, refer to https://en.wikipedia.org/wiki/Email_address, correct, and press the button 'Save' again.", "Invalid E-mail address.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                else
                {
                    my_body.Add("Subject", TextInput_Subject.Text);
                    if (message_type == "SIR")
                    {
                        TextInput_Message.Text = TextInput_SportCentreCode1.Text + "-" + TextInput_SportCentreCode2.Text + "-" + TextInput_SportCentreCode3.Text + "\n" + ComboInput_NatureOfIncident.Text + "\n" + TextInput_Message.Text;
                    }
                }
            }

            my_body.Add("Message Text", TextInput_Message.Text);
            incoming_message.Add("Body", my_body);


            string json = JsonConvert.SerializeObject(incoming_message);
            StreamWriter sw = new StreamWriter(my_database_path, true, Encoding.ASCII);
            sw.WriteLine(json);
            sw.Close();
            
            // Restart the editor:
            TextInput_Header.Text = (int.Parse(TextInput_Header.Text) + 1).ToString("D9");
            TextInput_Message.Text = "";
            TextInput_SportCentreCode1.Text = "";
            TextInput_SportCentreCode2.Text = "";
            TextInput_SportCentreCode3.Text = "";
            ComboInput_NatureOfIncident.SelectedIndex = 0;
            message_type = "SMS";
            SMS.IsChecked = false;
            SMS.IsChecked = true;
        }

        private void ButtonInput_Save_As_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog file = new SaveFileDialog();
            file.Filter = "json files (*.json)|*.json";
            file.RestoreDirectory = true;
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(file.FileName, false, Encoding.ASCII);
                sw.WriteLine("Null");
                sw.Close();
                TextInput_Header.Text = "000000001";
                my_database_path = file.FileName;
                ButtonInput_Save_Click(sender, e);
            }
        }

        private void ButtonInput_Save_To_Click(object sender, RoutedEventArgs e)
        {
            string path;
            OpenFileDialog file = new OpenFileDialog();
            file.InitialDirectory = my_database_path;
            file.Filter = "json files (*.json)|*.json";
            file.RestoreDirectory = true;
            if (file.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                path = file.FileName;
                string last_line = "";
                System.Windows.Forms.MessageBox.Show(path, "Toto je path", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                try
                {
                    last_line = File.ReadLines(path).Last();
                    if (last_line != "Null")
                    {
                        var try_record = JsonConvert.DeserializeObject<Dictionary<string, object>>(last_line);
                        string try_ID = try_record["Header"].ToString();
                    }
                }
                catch (Exception)
                {
                    last_line = "Null";
                    StreamWriter sw = new StreamWriter(path, false, Encoding.ASCII);
                    sw.WriteLine("Null");
                    sw.Close();
                }
                finally
                {
                    string current_ID = "000000001";
                    if (last_line != "Null")
                    {
                        var last_record = JsonConvert.DeserializeObject<Dictionary<string, object>>(last_line);
                        string last_ID = last_record["Header"].ToString();
                        current_ID = (int.Parse(last_ID) + 1).ToString("D9");
                    }

                    TextInput_Header.Text = current_ID;
                    my_database_path = path;
                    ButtonInput_Save_Click(sender, e);
                }
            }
        }
    }
}
