using System.Windows.Forms;

namespace Euston_Leisure_Messaging_Service
{
    public class TruncatePath
    {
        public string Path;

        public TruncatePath(OpenFileDialog file)
        {
            if (file.FileName.Length > 40)
            {
                string fileName = file.SafeFileName;
                Path = file.FileName.Replace(fileName, "");
                if (fileName.Length < 34)
                {
                    Path = Path.Substring(0, 3) + "...\\" + Path.Substring(Path.Length - 40 + fileName.Length);
                }
                else
                {
                    Path = Path.Substring(0, 3) + "...\\";
                }
                Path += fileName;
            }
            else
            {
                Path = file.FileName;
            }
        }
    }
}