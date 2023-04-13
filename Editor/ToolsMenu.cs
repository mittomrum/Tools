using System.IO;
using UnityEditor;
using UnityEngine;
using static System.IO.Directory;
using static System.IO.Path;
using static UnityEngine.Application;
using static UnityEditor.AssetDatabase;
using System.Threading.Tasks;
using System.Net.Http;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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
        public static void InstallUnityPackage(string packageName)
        {
            UnityEditor.PackageManager.Client.Add($"com.unity.{packageName}");
        }
    }
    public static class ScriptGenerator
    {
        public static async Task ReplaceScriptFile(string id, string user = "mittomrum")
        {
            var url = GetGistUrl(id, user);
            var contents = await GetContents(url);
            ReplacePackageFile(contents);
        }

        private static void ReplacePackageFile(string contents)
        {
            var existing = Path.Combine(Application.dataPath, "../Assets/_Project/Scripts/Movement");
            File.WriteAllText(existing, contents);
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
    }

    public static class ToolsMenu
    {
        [MenuItem("Tools/Setup/Initial Project Folders")]
        static void CreateDefaultFolders()
        {
            Folders.CreateDirectories("_Project", "Scenes", "Prefabs", "Canvas");
            Folders.CreateDirectories("_Project/_Scripts/", "Scriptables", "Systems", "Managers");
            Folders.CreateDirectories("_Project/Resources/Art", "Sprites", "Materials");
            Folders.CreateDirectories("_Project/Resources/Audio");

            Refresh();
        }

        [MenuItem("Tools/Setup/Initial Project Stucture (Inspector)")]
        [System.Obsolete]
        static void Create()
        {
            GameObject managers = new GameObject("Managers");
            GameObject exampleUnityManager = new GameObject("Example UnityManager");
            exampleUnityManager.transform.SetParent(managers.transform, false);
            GameObject exampleGameManager = new GameObject("Example GameManager");
            exampleGameManager.transform.SetParent(managers.transform, false);

            GameObject setup = new GameObject("Setup");
            GameObject mainCamera = new GameObject("Main Camera");
            mainCamera.AddComponent<Camera>();
            GameObject directionalLight = new GameObject("Directional Light");
            directionalLight.AddComponent<Light>();
            GameObject eventSystem = new GameObject("EventSystem");
            eventSystem.AddComponent<EventSystem>();
            eventSystem.AddComponent<StandaloneInputModule>();
            setup.transform.SetParent(managers.transform, false);

            GameObject environment = new GameObject("Environment");
            GameObject exampleUnits = new GameObject("Example Units");
            exampleUnits.transform.SetParent(environment.transform, false);

            GameObject canvases = new GameObject("Canvases");
            GameObject mainCanvas = new GameObject("Main Canvas");
            mainCanvas.AddComponent<Canvas>();
            mainCanvas.AddComponent<CanvasScaler>();
            mainCanvas.AddComponent<GraphicRaycaster>();
            mainCanvas.transform.SetParent(canvases.transform, false);

            GameObject systems = new GameObject("Systems");
            GameObject audioSystem = new GameObject("Audio System");
            audioSystem.transform.SetParent(systems.transform, false);
            GameObject resourceSystem = new GameObject("Resource System");
            resourceSystem.transform.SetParent(systems.transform, false);
        }


        [MenuItem("Tools/Setup/Initial Scripts (Movement)")]
        static async void LoadNewScripts()
        {
            await ScriptGenerator.ReplaceScriptFile("169f3da32129a3506c5d7a1e2dc6d072");
            Debug.Log("Done Loading");
        }

        [MenuItem("Tools/Setup/Load Manifest")]
        static async void LoadNewManifest()
        {
            await Packages.ReplacePackagesFromGist("54769e89109842169d88029fea3714e1");
        }

        [MenuItem("Tools/Setup/Packages/New Input System")]
        static void AddNewInputSystem() => Packages.InstallUnityPackage("inputsystem");

        [MenuItem("Tools/Setup/Packages/ProBuilder")]
        static void AddNewProbuilder() => Packages.InstallUnityPackage("probuilder");

        [MenuItem("Tools/Setup/Packages/Cinemachine")]
        static void AddNewCinemachine() => Packages.InstallUnityPackage("cinemachine");
    }
}
