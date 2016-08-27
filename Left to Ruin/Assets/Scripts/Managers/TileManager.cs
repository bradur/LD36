// Date   : 27.08.2016 10:07
// Project: LD36
// Author : bradur

using UnityEngine;
using System.Collections;

public enum TileType {
    None,
    Floor,
    Wall,
    Hole
};

public enum ObjectType
{
    None,
    MovableBlock,
    ProjectileShooter
}

public class TileManager : MonoBehaviour {

    public static TileManager main;
    private int tileCountX;
    private int tileCountZ;

    void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("TileManager").Length < 1)
        {
            gameObject.tag = "TileManager";
            main = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public SingleTile[] tiles;

    public void Init(int tileCountX, int tileCountZ)
    {
        this.tileCountX = tileCountX;
        this.tileCountZ = tileCountZ;
        tiles = new SingleTile[tileCountX * tileCountZ];
    }

    public void AddTile(int x, int z, TileType tileType)
    {
        tiles[tileCountX * z + x] = new SingleTile(x, z, tileType);
    }

    public void AddObjectToTile(int x, int z, GenericObject newObj)
    {
        tiles[tileCountX * z + x].AddObject(newObj);
    }

    public SingleTile GetTile(int x, int z)
    {
        return tiles[tileCountX * z + x];
    }
}

public class SingleTile
{
    private int x;
    private int z;
    private TileType tileType;
    public TileType TileType { get { return tileType; } }
    private GenericObject tileObject = null;
    public GenericObject TileObject { get { return tileObject; } }

    public SingleTile(int x, int z, TileType tileType)
    {
        this.x = x;
        this.z = z;
        this.tileType = tileType;
    }

    public void AddObject(GenericObject tileObject)
    {
        if(this.tileObject == null)
        {
            Debug.Log("ERROR: " + tileObject.name + " already contains an object.");
        }
        this.tileObject = tileObject;
    }

    public void RemoveObject()
    {
        this.tileObject = null;
    }
}
