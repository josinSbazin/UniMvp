using System.IO;
using System.Linq;
using UnityEditor;

namespace Mvp.Editor
{
    public class MvpMenuItems
    {
        private const string ViewTemplate =
            "using UnityEngine;"
            + "\nusing Mvp;"
            + "\n"
            + "\npublic class VIEW_CLASS_NAME : View<VIEW_CLASS_NAME, PRESENTER_CLASS_NAME>"
            + "\n{"
            + "\n    protected override void FindComponents()"
            + "\n    {"
            + "\n        //find components here"
            + "\n    }"
            + "\n"
            + "\n    protected override void Awake()"
            + "\n    {"
            + "\n        base.Awake();"
            + "\n        //view awake logic here"
            + "\n    }"
            + "\n}";

        private const string PresenterTamplate =
            "using UnityEngine;"
            + "\nusing Mvp;"
            + "\n"
            + "\npublic class PRESENTER_CLASS_NAME : Presenter<VIEW_CLASS_NAME, PRESENTER_CLASS_NAME>"
            + "\n{"
            + "\n    public override void Init()"
            + "\n    {"
            + "\n        //init here"
            + "\n    }"
            + "\n}";


        [MenuItem("Assets/Create/Mvp/View And Presenter Pair", false, 1)]
        public static void CreateViewAndPresenterPair()
        {
            AddMvpPair("View And Presenter Pair");
        }

        static void AddMvpPair(string friendlyName)
        {
            var directoryPath = EditorUtils.GetCurrentDirectoryAbsolutePathFromSelection();
            var absolutePath = EditorUtility.SaveFolderPanel("Choose folder for " + friendlyName, directoryPath, "");

            if (absolutePath == "")
            {
                return;
            }

            var end = GetEnd(absolutePath);

            var viewAbsolutePath = absolutePath + end + "View.cs";
            var presenterAbsolutePath = absolutePath + end + "Presenter.cs";

            var viewClassName = Path.GetFileNameWithoutExtension(viewAbsolutePath);
            var presenterClassName = Path.GetFileNameWithoutExtension(presenterAbsolutePath);

            File.WriteAllText(viewAbsolutePath,
                ViewTemplate.Replace("VIEW_CLASS_NAME", viewClassName)
                    .Replace("PRESENTER_CLASS_NAME", presenterClassName));
            File.WriteAllText(presenterAbsolutePath,
                PresenterTamplate.Replace("VIEW_CLASS_NAME", viewClassName)
                    .Replace("PRESENTER_CLASS_NAME", presenterClassName));

            AssetDatabase.Refresh();

            var assetPath = EditorUtils.ConvertFullAbsolutePathToAssetPath(absolutePath);

            EditorUtility.FocusProjectWindow();
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
        }

        private static string GetEnd(string path)
        {
            return Path.AltDirectorySeparatorChar + path.Split(Path.AltDirectorySeparatorChar).Last();
        }
    }
}