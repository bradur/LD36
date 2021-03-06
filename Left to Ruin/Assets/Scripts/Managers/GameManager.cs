// Date   : 27.08.2016 08:56
// Project: LD36
// Author : bradur

using UnityEngine;
using System.Collections.Generic;
using TiledSharp;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager main;
    [SerializeField]
    private TiledMesh tiledMeshPrefab;

    [SerializeField]
    private GenericObject[] objectPrefabs;

    private WorldManager world;

    [SerializeField]
    private Transform projectileContainer;
    public Transform ProjectileContainer { get { return projectileContainer; } }

    [SerializeField]
    private List<Level> levels;

    [SerializeField]
    private GameObject wallBlockPrefab;

    [SerializeField]
    private Player playerPrefab;
    private Player player;
    public Player Player { get { return player; } }

    [SerializeField]
    private int currentLevelNumber = 0;

    private Level currentLevel;
    public Level CurrentLevel { get { return currentLevel; } }

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
    private List<Item> items = new List<Item>();
    public List<Item> Items { get { return items; } }

    public void RemoveItem(Item item)
    {
        items.Remove(item);
        UIManager.main.RemoveItem(item);
    }

    public void GainItem(Item item)
    {
        items.Add(item);
        UIManager.main.AddItem(item);
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (GameObject.FindGameObjectsWithTag("GameManager").Length < 1)
        {
            gameObject.tag = "GameManager";
            main = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RestartLevel()
    {
        UIManager.main.ClearItems();
        UIManager.main.ClearDialogs();
        Time.timeScale = 1f;
        SceneManager.LoadScene("game");
        LoadLevel(levels[currentLevelNumber]);
    }

    public void FinishLevel()
    {
        UIManager.main.ClearDialogs();
        UIManager.main.ClearItems();
        currentLevelNumber++;
        
        if (currentLevelNumber > levels.Count - 1)
        {
            UIManager.main.AddDialog(
                "21st of December, 1879",
                "I thought I was lost for sure. But by a magnificent stroke of luck I have succeeded! I will bring with me all these amazing findings and help the world!",
                DialogAction.GameFinished,
                ""
            );
            currentLevelNumber--;
        } else {
            UIManager.main.AddDialog(
                levels[currentLevelNumber-1].LevelEndDate,
                levels[currentLevelNumber-1].LevelEndDescription,
                DialogAction.NextLevel,
                ""
            );
        }
    }

    public void GameOver()
    {
        UIManager.main.AddDialog(
            levels[currentLevelNumber].LevelEndDate,
            "I go to my death with a restless mind - there is still so much I don't know about this place!",
            DialogAction.Restart,
            ""
        );
    }

    public void OpenNextLevel()
    {
        UIManager.main.ClearDialogs();
        Time.timeScale = 1f;
        SceneManager.LoadScene("game");
    }

    /*void Start()
    {
        LoadLevel(levels[currentLevel]);
    }*/

    void OnLevelWasLoaded(int level)
    {
        Debug.Log("LEVEL LOADED: " + level);
        if (level == 0)
        {
            Time.timeScale = 1f;
            Destroy(gameObject);
        }
        else if (main == this) {
            
            LoadLevel(levels[currentLevelNumber]);
        }

    }

    void LoadLevel(Level level)
    {
        currentLevel = level;
        Time.timeScale = 0f;
        world = GameObject.FindGameObjectWithTag("World").GetComponent<WorldManager>();
        float yInterval = -3f;
        //TmxMap map = new TmxMap(level.MapFilePath);
        TmxMap map = new TmxMap(level.MapFile.text, "rnd");
        TileManager.main.Init(map.Width, map.Height);
        for (int i = 0; i < map.Layers.Count; i++)
        {
            TmxLayer layer = map.Layers[i];
            TileType tileType = (TileType)GameManager.IntParseFast(layer.Properties["TileType"]);
            if (tileType == TileType.Wall)
            {
                for (int x = 0; x < map.Width; x++)
                {
                    for (int z = 0; z < map.Height; z++)
                    {
                        int tileId = layer.Tiles[(map.Height - z - 1) * map.Width + x].Gid - 1;
                        if (tileId == -1)
                        {
                            continue;
                        }
                        GameObject wallBlock = (GameObject)Instantiate(wallBlockPrefab, new Vector3(x, wallBlockPrefab.transform.position.y, z), Quaternion.identity);
                        wallBlock.transform.SetParent(world.WallContainer, false);
                        wallBlock.gameObject.isStatic = true;
                    }
                }
            }

            TiledMesh tiledMesh = Instantiate(tiledMeshPrefab);
            if (tileType == TileType.Hole)
            {
                tiledMesh.transform.position = new Vector3(tiledMesh.transform.position.x, tiledMesh.transform.position.y + yInterval, tiledMesh.transform.position.z);
                tiledMesh.gameObject.tag = "Hole";
            }
            if (tileType == TileType.Bottom)
            {
                tiledMesh.transform.position = new Vector3(tiledMesh.transform.position.x, tiledMesh.transform.position.y + yInterval, tiledMesh.transform.position.z);
            }
            tiledMesh.transform.parent = world.transform;
            tiledMesh.Init(map.Width, map.Height, layer, level.MapMaterial);
        }
        foreach (TmxObjectGroup group in map.ObjectGroups)
        {
            foreach (TmxObjectGroup.TmxObject tmxObject in group.Objects)
            {
                int objectType = GameManager.IntParseFast(tmxObject.Properties["ObjectType"]);
                if ((ObjectType)objectType == ObjectType.PlayerSpawn)
                {
                    SpawnPlayer((int)tmxObject.X / tileSize, map.Height - (int)tmxObject.Y / tileSize);
                }
                else
                {
                    GenericObject genericObject = Instantiate(objectPrefabs[objectType]);
                    genericObject.transform.parent = world.transform;
                    Debug.Log(objectType + ": " + (int)objectPrefabs[objectType].transform.position.y);
                    genericObject.Init((int)tmxObject.X / tileSize, objectPrefabs[objectType].transform.position.y, map.Height - (int)tmxObject.Y / tileSize, (ObjectType)objectType, tmxObject.Properties);
                }
            }
        }
        Time.timeScale = 1f;
    }

    public void SpawnPlayer(int x, int z)
    {
        if (player == null)
        {
            player = Instantiate(playerPrefab);
            player.transform.SetParent(world.transform, false);
            player.Init(x, z);
            world.CameraFollower.SetTarget(player.transform);
        }
        else
        {
            Debug.Log("<b>warning:</b> trying to duplicate player!");
        }
    }

    public static int IntParseFast(string value)
    {
        int result = 0;
        try
        {
            for (int i = 0; i < value.Length; i++)
            {
                char letter = value[i];
                result = 10 * result + (letter - 48);
            }
        }
        catch (System.NullReferenceException)
        {
            result = -1;
        }
        return result;
    }
}
