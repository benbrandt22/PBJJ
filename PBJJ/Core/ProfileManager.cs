using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using System.Text.RegularExpressions;

namespace PBJJ.Core
{
    public static class ProfileManager {
        
        private static StorageFolder ProfilesFolder {
            get {
                return Windows.Storage.ApplicationData.Current.LocalFolder;
            }
        }

        public static async Task SaveFile(string fileName, string content) {
            // replace bad characters
            fileName = RemoveBadFileNameChars(fileName);

            // save file; replace if exists.
            var file = await ProfilesFolder.CreateFileAsync(fileName,
                Windows.Storage.CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(file, content);
        }

        private static string RemoveBadFileNameChars(string fileName) {
            var badChars = Path.GetInvalidFileNameChars();
            foreach (var badChar in badChars) {
                fileName = fileName.Replace(badChar, '_');
            }
            return fileName;
        }

        private static async Task<string> ReadFile(string fileName) {
            var file = await ProfilesFolder.GetFileAsync(fileName);
            var content = await Windows.Storage.FileIO.ReadTextAsync(file);
            return content;
        }

        public static async Task<ProfilesViewModel> GetProfiles() {
            var files = await GetProfileFiles();
            var result = new ProfilesViewModel() {
                ProfileNames = files.Select(f => f.Name).ToArray()
            };
            return result;
        }

        private static async Task<List<StorageFile>> GetProfileFiles() {
            var files = await ProfilesFolder.GetFilesAsync();
            return files.ToList();
        }

        public static async void CreateNewProfile(NewProfileViewModel newProfileViewModel) {
            if (newProfileViewModel.Type == "Standard") {
                var newProfile = ProfileGenerator.GenerateStandardProfile(newProfileViewModel.FingerWidth, newProfileViewModel.OverallWidth);
                await SaveProfile(newProfile);
                return;
            }
            if (newProfileViewModel.Type == "Custom") {
                var newProfile = new JointProfile(newProfileViewModel.Name);
                await SaveProfile(newProfile);
                return;
            }
        }

        private static async Task SaveProfile(JointProfile jointProfile) {
            var filename = $"{jointProfile.Name}{(jointProfile.Name.EndsWith(".bjp") ? "" : ".bjp")}";
            var content = ElementsToFileData(jointProfile.Elements);
            await SaveFile(filename, content);
        }

        private static string ElementsToFileData(List<JointProfileElement> elements) {
            var lines = elements.Select(e => $"{e.Type.ToString()[0]} {e.Width:N3}");
            return string.Join("\r\n", lines);
        }

        private static List<JointProfileElement> FileDataToElements(string data) {
            var elements = new List<JointProfileElement>();
            var lines = data.Split(new[] {'\r', '\n'}, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines) {
                if (Regex.IsMatch(line, @"(s|S|f|F) \d(\.\d{1,3})?")) {
                    var firstChar = line[0].ToString();
                    var type = firstChar.ToLower() == "f"
                        ? JointProfileElement.JointProfileElementType.Finger
                        : JointProfileElement.JointProfileElementType.Slot;
                    var width = double.Parse(line.Substring(2));
                    var jpe = new JointProfileElement(type, width);
                    elements.Add(jpe);
                }
            }
            return elements;
        }

        public static async void LoadProfileByName(string name) {
            var fileData = await ReadFile(name);
            var jointElements = FileDataToElements(fileData);
            var jointProfile = new JointProfile(name);
            jointProfile.Elements.AddRange(jointElements);

            ProgrammableBoxJointJigApp.Instance.Profile = jointProfile;
        }

        public static async Task<ProfileDataModel> GetProfileData(string fileName) {
            fileName = fileName + (fileName.EndsWith(".bjp") ? "" : ".bjp");
            var data = await ReadFile(fileName);
            return new ProfileDataModel() {
                FileName = fileName,
                FileData = data
            };
        }
        
    }

    public class ProfileDataModel {
        public string FileName { get; set; }
        public string FileData { get; set; }
    }

    public class ProfilesViewModel {
        public string[] ProfileNames { get; set; }
    }
}
