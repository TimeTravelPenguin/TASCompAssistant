using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Forms;
using TASCompAssistant.Types;

namespace TASCompAssistant.Models
{
    internal class FileModel : PropertyChangedBase
    {
        private readonly DataModel Data = new DataModel();
        private string _fileName = "No file opened...";

        public string FileName
        {
            get => _fileName;
            private set => SetValue(ref _fileName, value);
        }

        public ObservableCollection<CompetitionTaskModel> OpenFile()
        {
            var ofd = new OpenFileDialog
            {
                Filter = "TAS Comp File (*.tascomp)|*.tascomp"
            };
            ofd.ShowDialog();

            try
            {
                var fileName = ofd.FileName;
                var filePath = Path.GetFullPath(fileName);
                var extension = Path.GetExtension(filePath);

                if (extension == ".tascomp")
                {
                    FileName = filePath;
                    var fileData = File.ReadAllText(filePath);
                    var data = Data.OpenData(fileData);
                    return data;
                }

                FileClear();
                MessageBox.Show("Please open a valid .tascomp file", "Error opening file...");
                return new ObservableCollection<CompetitionTaskModel>();
            }
            catch // Put errorhandling here for invalid JSON?
            {
                FileClear();
            }

            return new ObservableCollection<CompetitionTaskModel>();
        }

        public void SaveFile(ObservableCollection<CompetitionTaskModel> data)
        {
            var saveData = Data.SaveData(data);

            var sfd = new SaveFileDialog
            {
                Filter = "TAS Comp File (*.tascomp)|*.tascomp",
                DefaultExt = "tascomp",
                AddExtension = true
            };

            sfd.ShowDialog();

            try
            {
                var path = Path.GetFullPath(sfd.FileName);

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