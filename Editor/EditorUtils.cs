using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;

namespace Mvp.Editor
{
    public class EditorUtils
    {
        public static string GetCurrentDirectoryAssetPathFromSelection()
        {
            return ConvertFullAbsolutePathToAssetPath(
                GetCurrentDirectoryAbsolutePathFromSelection());
        }
        
        public static string GetCurrentDirectoryAbsolutePathFromSelection()
        {
            var folderPath = TryGetSelectedFolderPathInProjectsTab();

            if (folderPath != null)
            {
                return folderPath;
            }

            var filePath = TryGetSelectedFilePathInProjectsTab();

            if (filePath != null)
            {
                return Path.GetDirectoryName(filePath);
            }

            return Application.dataPath;
        }
        
        public static string ConvertFullAbsolutePathToAssetPath(string fullPath)
        {
            fullPath = Path.GetFullPath(fullPath);

            var assetFolderFullPath = Path.GetFullPath(Application.dataPath);

            if (fullPath.Length == assetFolderFullPath.Length)
            {
                Assert.IsTrue(fullPath == assetFolderFullPath);
                return "Assets";
            }

            var assetPath = fullPath.Remove(0, assetFolderFullPath.Length + 1).Replace("\\", "/");
            return "Assets/" + assetPath;
        }
        
        public static string TryGetSelectedFilePathInProjectsTab()
        {
            return OnlyOrDefault(GetSelectedFilePathsInProjectsTab());
        }

        public static List<string> GetSelectedFilePathsInProjectsTab()
        {
            return GetSelectedPathsInProjectsTab()
                .Where(x => File.Exists(x)).ToList();
        }
        
          // Returns the best guess directory in projects pane
        // Useful when adding to Assets -> Create context menu
        // Returns null if it can't find one
        // Note that the path is relative to the Assets folder for use in AssetDatabase.GenerateUniqueAssetPath etc.
        public static string TryGetSelectedFolderPathInProjectsTab()
        {
            return OnlyOrDefault(GetSelectedFolderPathsInProjectsTab());
        }
        
        public static List<string> GetSelectedPathsInProjectsTab()
        {
            var paths = new List<string>();

            UnityEngine.Object[] selectedAssets = Selection.GetFiltered(
                typeof(UnityEngine.Object), SelectionMode.Assets);

            foreach (var item in selectedAssets)
            {
                var relativePath = AssetDatabase.GetAssetPath(item);

                if (!string.IsNullOrEmpty(relativePath))
                {
                    var fullPath = Path.GetFullPath(Path.Combine(
                        Application.dataPath, Path.Combine("..", relativePath)));

                    paths.Add(fullPath);
                }
            }

            return paths;
        }
        
        // Note that the path is relative to the Assets folder
        public static List<string> GetSelectedFolderPathsInProjectsTab()
        {
            return GetSelectedPathsInProjectsTab()
                .Where(x => Directory.Exists(x)).ToList();
        }

        // Return the first item when the list is of length one and otherwise returns default
        public static TSource OnlyOrDefault<TSource>(IEnumerable<TSource> source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (source.Count() > 1)
            {
                return default(TSource);
            }

            return source.FirstOrDefault();
        }
    }
}