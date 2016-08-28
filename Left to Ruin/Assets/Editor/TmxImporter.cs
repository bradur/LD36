// Date   : 28.08.2016 11:17
// Project: LD36
// Author : bradur

using UnityEditor;
using UnityEngine;
using System.IO;

public class TmxImporter : AssetPostprocessor {

    static bool first = true;

    public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
        foreach(string asset in importedAssets)
        {
            if (asset.EndsWith(".tmx"))
            {
                if (first)
                {
                    Debug.Log("<b>Map changes, saving...</b>");
                    first = false;
                }
                string filePath = "Assets/Resources/Maps/";
                string fileName = filePath + Path.GetFileNameWithoutExtension(asset) + ".xml";

                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }

                StreamReader reader = new StreamReader(asset);
                string fileData = reader.ReadToEnd();
                reader.Close();

                FileStream resourceFile = new FileStream(fileName, FileMode.Create, FileAccess.Write);
                StreamWriter writer = new StreamWriter(resourceFile);

                writer.Write(fileData);
                writer.Close();
                resourceFile.Close();

                AssetDatabase.Refresh(ImportAssetOptions.Default);
                Debug.Log("<b>Map file saved at: </b>" + fileName);
            }
        }
        first = true;
    }

}
