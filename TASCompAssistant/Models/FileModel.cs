using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TASCompAssistant.Types;

namespace TASCompAssistant.Models
{
    internal class FileModel : PropertyChangedBase
    {
        private DataModel Data = new DataModel();

        private string _fileName = "No file opened...";
        public string FileName
        {
            get => _fileName;
            private set => SetValue(ref _fileName, value);
        }

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
                    FileName = filePath;
                    var fileData = File.ReadAllText(filePath);
                    var data = Data.OpenData(fileData);
                    return data;
                }
                else
                {
                    FileClear();
                    MessageBox.Show("Please open a valid .tascomp file", "Error opening file...");
                    return new ObservableCollection<CompetitionModel>();
                }
            }
            catch   // Put errorhandling here for invalid JSON?
            {
                FileClear();
            }

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

                FileName = path;

                File.WriteAllText(path, saveData);
            }
            catch (Exception)
            {
                FileName = "File failed to save...";
            }
        }

        internal void FileClear()
        {
            FileName = "No file Opened...";
        }
    }
}
