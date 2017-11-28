foreach (string key in abbreviations.Keys)
	{
		OutputText.Text += key + ", " + abbreviations[key] + "\n";
	}
	
	
string fullPath = file.FileName;
string fileName = file.SafeFileName;
string path = fullPath.Replace(fileName, "");


System.Windows.Forms.MessageBox.Show("", "", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);



// Now only process those messages which are not processed already.
            StreamReader incoming_file = new StreamReader(main_path + main_fileName + ".json");
            string line = incoming_file.ReadLine();             // first line is always Null
            while ((line = incoming_file.ReadLine()) != null)
            {
                try
                {
                    var record = JsonConvert.DeserializeObject<Dictionary<string, object>>(line);
                    string try_header = record["Header"].ToString();
                    
                }
                catch (Exception exc)
                {
                    System.Windows.Forms.MessageBox.Show("Ooops, file" + main_fileName + ".json seems to be broken. Processing terminated.", exc.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                
            }