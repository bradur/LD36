using UnityEngine;
using UnityEditor;

internal sealed class ScriptKeywordProcessor : UnityEditor.AssetModificationProcessor {

    public static void OnWillCreateAsset(string path) {
        path = path.Replace(".meta", "");
        int index = path.LastIndexOf(".");
        if(index == -1) return;

        string file = path.Substring(index);
        if(file != ".cs" && file != ".js" && file != ".boo") return;
        index = Application.dataPath.LastIndexOf("Assets");
        path = Application.dataPath.Substring(0, index) + path;
        file = System.IO.File.ReadAllText(path);
        file = file.Replace("#CREATIONDATE#", System.DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
        file = file.Replace("#PROJECTNAME#", PlayerSettings.productName);
        file = file.Replace("#AUTHOR#", "bradur");
        System.IO.File.WriteAllText(path, file);
        AssetDatabase.Refresh();
    }
}
