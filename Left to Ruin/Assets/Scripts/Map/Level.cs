// Date   : 27.08.2016 08:59
// Project: LD36
// Author : bradur

using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour
{

    [SerializeField]
    private string mapFilePath;

    [SerializeField]
    private TextAsset mapFile;
    public TextAsset MapFile { get { return mapFile; } }

    [SerializeField]
    private Material mapMaterial;
    public Material MapMaterial { get { return mapMaterial; } }

    public string MapFilePath { get { return mapFilePath; } }

    public void SetMapPath(string newPath)
    {
        mapFilePath = newPath;
    }

}
