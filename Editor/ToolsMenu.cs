using System.IO;
using UnityEditor;
using UnityEngine;
using static System.IO.Directory;
using static System.IO.Path;
using static UnityEngine.Application;
using static UnityEditor.AssetDatabase;
using System.Threading.Tasks;
using System.Net.Http;

namespace tomrum
{

    public static class Folders
    {
        public static void CreateDirectories(string root, params string[] dir)
        {
            foreach (var newDirectory in dir)
            {
                CreateDirectory(Combine(dataPath, root, newDirectory));
            }
        }
    }

    public static class Packages
    {
        public static async Task ReplacePackagesFromGist(string id, string user = "mittomrum")
        {
            var url = GetGistUrl(id, user);
            var contents = await GetContents(url);
            ReplacePackageFile(contents);
        }

        private static void ReplacePackageFile(string contents)
        {
            var existing = Path.Combine(Application.dataPath, "../Packages/manifest.json");
            File.WriteAllText(existing, contents);
            UnityEditor.PackageManager.Client.Resolve();
        }

        private static string GetGistUrl(string id, string user) =>
            $"https://gist.github.com/{user}/{id}/raw";

        private static async Task<string> GetContents(string url)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
        public static void InstallUnityPackage(string packageName){
            UnityEditor.PackageManager.Client.Add($"com.unity.{packageName}");
        }
    }

    public static class ToolsMenu
    {
        [MenuItem("Tools/Setup/Initial Project Folders")]
        static void CreateDefaultFolders()
        {
            Folders.CreateDirectories("_Project", "Scripts", "Art", "Scenes");
            Refresh();
        }

        [MenuItem("Tools/Setup/Load Manifest")]
        static async void LoadNewManifest()
        {
            await Packages.ReplacePackagesFromGist("54769e89109842169d88029fea3714e1");
        }

        [MenuItem("Tools/Setup/Packages/New Input System")]
        static void AddNewInputSystem() => Packages.InstallUnityPackage("inputsystem");

        [MenuItem("Tools/Setup/Packages/ProBuilder")]
        static void AddNewInputSystem() => Packages.InstallUnityPackage("ProBuilder");

        [MenuItem("Tools/Setup/Packages/Cinemachine")]
        static void AddNewInputSystem() => Packages.InstallUnityPackage("Cinemachine");
        
    }
}
