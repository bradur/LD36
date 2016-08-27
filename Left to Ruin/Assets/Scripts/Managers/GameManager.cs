// Date   : 27.08.2016 08:56
// Project: LD36
// Author : bradur

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TiledSharp;

public class GameManager : MonoBehaviour {

    public static GameManager main;
    [SerializeField]
    private TiledMesh tiledMeshPrefab;

    [SerializeField]
    private Transform world;

    [SerializeField]
    private List<Level> levels;

    private int currentLevel = 0;

    [SerializeField]
    private Player player;

    void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("GameManager").Length < 1)
        {
            gameObject.tag = "GameManager";
            main = this;
        } else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        LoadLevel(levels[currentLevel]);
    }

    void LoadLevel(Level level)
    {
        TmxMap map = new TmxMap(level.MapFilePath);
        TileManager.main.Init(map.Width, map.Height);
        player.Init(2, 2);
        foreach(TmxLayer layer in map.Layers)
        {
            TiledMesh tiledMesh = Instantiate(tiledMeshPrefab);
            tiledMesh.transform.parent = world;
            tiledMesh.Init(map.Width, map.Height, layer, level.MapMaterial);
        }

    }

    public static int IntParseFast(string value)
    {
        int result = 0;
        try
        {
            for(int i = 0; i < value.Length; i++)
            {
                char letter = value[i];
                result = 10 * result + (letter - 48);
            }
        }
        catch(System.NullReferenceException)
        {
            result = -1;
        }
        return result;
    }
}
