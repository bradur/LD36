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
    private GenericObject[] objectPrefabs;

    [SerializeField]
    private Transform world;

    [SerializeField]
    private Transform projectileContainer;
    public Transform ProjectileContainer { get { return projectileContainer; } }

    [SerializeField]
    private List<Level> levels;

    private int currentLevel = 0;

    private static int[] rotations = new int[] {
        -1, // None
        0,
        90,
        180,
        270
    };

    public static int[] Rotations { get { return rotations; } }

    [SerializeField]
    private int tileSize = 64;

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
        float yInterval = -3f;
        TmxMap map = new TmxMap(level.MapFilePath);
        
        TileManager.main.Init(map.Width, map.Height);
        player.Init(5, 5);
        for(int i = 0; i < map.Layers.Count; i++)
        {
            TmxLayer layer = map.Layers[i];
            TileType tileType = (TileType)GameManager.IntParseFast(layer.Properties["TileType"]);
            TiledMesh tiledMesh = Instantiate(tiledMeshPrefab);
            if(tileType == TileType.Hole)
            {
                tiledMesh.transform.position = new Vector3(tiledMesh.transform.position.x, tiledMesh.transform.position.y + yInterval, tiledMesh.transform.position.z);
                tiledMesh.gameObject.tag = "Hole";
            }
            tiledMesh.transform.parent = world;
            tiledMesh.Init(map.Width, map.Height, layer, level.MapMaterial);
        }
        foreach(TmxObjectGroup group in map.ObjectGroups)
        {
            foreach(TmxObjectGroup.TmxObject tmxObject in group.Objects)
            {
                int objectType = GameManager.IntParseFast(tmxObject.Properties["ObjectType"]);
                GenericObject genericObject = Instantiate(objectPrefabs[objectType]);
                genericObject.transform.parent = world;
                genericObject.Init((int)tmxObject.X / tileSize, map.Height - (int)tmxObject.Y / tileSize, (ObjectType)objectType, tmxObject.Properties);
            }
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
