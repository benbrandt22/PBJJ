using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace PBJJ.Core
{
    public static class ProfileManager {
        
        private static StorageFolder ProfilesFolder {
            get {
                return Windows.Storage.ApplicationData.Current.LocalFolder;
            }
        }

        private static async Task SaveFile(string fileName, string content) {
            // save file; replace if exists.
            var file = await ProfilesFolder.CreateFileAsync(fileName,
                Windows.Storage.CreationCollisionOption.ReplaceExisting);
            await Windows.Storage.FileIO.WriteTextAsync(file, content);
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
    }

    public class ProfilesViewModel {
        public string[] ProfileNames { get; set; }
    }
}
