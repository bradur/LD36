// Date   : 27.08.2016 08:59
// Project: LD36
// Author : bradur

using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour
{

    [SerializeField]
    private TextAsset mapFile;
    public TextAsset MapFile { get { return mapFile; } }

    [SerializeField]
    private Material mapMaterial;
    public Material MapMaterial { get { return mapMaterial; } }

    [SerializeField]
    [TextArea]
    private string levelEndDescription;
    public string LevelEndDescription { get { return levelEndDescription; } }

    [SerializeField]
    private string levelEndDate;
    public string LevelEndDate { get { return levelEndDate; } }

}
