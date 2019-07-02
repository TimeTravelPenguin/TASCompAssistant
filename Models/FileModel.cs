using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TASCompAssistant.Models
{
    internal class FileModel
    {
        private DataModel Data = new DataModel();
        public string FileName { get; private set; } = "No file opened...";

        public FileModel()
        {

        }

        public ObservableCollection<CompetitionModel> OpenFile()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "TAS Comp File (*.tascomp)|*.tascomp";
            ofd.ShowDialog();

            try
            {
                var fileName = ofd.FileName;
                var filePath = Path.GetFullPath(fileName);
                string extension = Path.GetExtension(filePath);

                if (extension == ".tascomp")
                {
                    var fileData = File.ReadAllText(filePath);
                    var data = Data.OpenData(fileData);
                    return data;
                }
                else
                {
                    fileName = "No file opened...";
                    MessageBox.Show("Please open a valid .tascomp file", "Error opening file...");
                    return new ObservableCollection<CompetitionModel>();
                }
            }
            catch { FileName = "No File Loaded..."; } // Put errorhandling here for invalid JSON?
            return new ObservableCollection<CompetitionModel>();
        }

        public void SaveFile(ObservableCollection<CompetitionModel> data)
        {
            var saveData = Data.SaveData(data);

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "TAS Comp File (*.tascomp)|*.tascomp";
            sfd.DefaultExt = "tascomp";
            sfd.AddExtension = true;
            sfd.ShowDialog();
            try
            {
                string path = Path.GetFullPath(sfd.FileName);

                File.WriteAllText(path, saveData);
            }
            catch (Exception) { }
        }
    }
}
