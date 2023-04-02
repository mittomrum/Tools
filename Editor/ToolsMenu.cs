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
    public static class ToolsMenu
    {
        [MenuItem("Tools/Setup/Initial Project Folders")]
        public static void CreateDefaultFolders()
        {
            CreateDirectories("_Project", "Scripts", "Art", "Scenes");
            Refresh();
        }
        [MenuItem("Tools/Setup/Load Manifest")]
        static async void LoadNewManifest()
        {
            var url = GetGistUrl("54769e89109842169d88029fea3714e1");
            var contents = await GetContents(url);
            ReplacePackageFile(contents);
        }

        //

        public static void CreateDirectories(string root, params string[] dir)
        {
            var fullPath = Combine(dataPath, root);
            foreach (var newDirectory in dir)
            {
                CreateDirectory(Combine(fullPath, newDirectory));
            }
        }

        static string GetGistUrl(string id, string user = "mittomrum") =>
            $"https://gist.github.com/{user}/{id}/raw";

        static async Task<string> GetContents(string url)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();
            return content;
        }
        static void ReplacePackageFile(string contents)
        {
            var existing = Path.Combine(Application.dataPath, "../Packages/manifest.json");
            File.WriteAllText(existing, contents);
            UnityEditor.PackageManager.Client.Resolve();
        }
    }
}
