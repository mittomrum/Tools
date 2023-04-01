using System.IO;
using UnityEditor;
using UnityEngine;
using static System.IO.Directory;
using static System.IO.Path;
using static UnityEngine.Application;
using static UnityEditor.AssetDatabase;

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
        public static void CreateDirectories(string root, params string[] dir)  {
            var fullPath = Combine(dataPath, root);
            foreach (var newDirectory in dir)
            {
                CreateDirectory(Combine(fullPath, newDirectory));
            }
        }
    }
}
