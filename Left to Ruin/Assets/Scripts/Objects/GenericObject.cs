// Date   : 27.08.2016 12:53
// Project: LD36
// Author : bradur

using UnityEngine;
using System.Collections;

public class GenericObject : MonoBehaviour
{
    [SerializeField]
    private ObjectType objectType;
    public ObjectType ObjectType { get { return objectType; } }

    private int xPos;
    private int zPos;
    public int XPos { get { return xPos; } }
    public int ZPos { get { return zPos; } }

    SingleTile currentTile;

    public void Init(int xPos, int zPos, ObjectType objectType)
    {
        this.objectType = objectType;
        this.xPos = xPos;
        this.zPos = zPos;
        transform.position = new Vector3(xPos, 0f, zPos);
        Debug.Log(xPos + ", " + ZPos);
        currentTile = TileManager.main.GetTile(xPos, zPos);
        currentTile.AddObject(this);
    }

    public bool Move(int x, int z)
    {
        int newXPos = (int)transform.position.x + x;
        int newZPos = (int)transform.position.z + z;
        SingleTile tile = TileManager.main.GetTile(newXPos, newZPos);
        if (tile != null)
        {
            if (tile.TileType == TileType.Wall)
            {
                Debug.Log("<b>blockmove:</b> [" + newXPos + ", " + newZPos + "] <color=red>WALL</color>");
                return false;
            }
            if (tile.TileObject != null)
            {
                Debug.Log("<b>blockmove:</b> [" + newXPos + ", " + newZPos + "] <color=red>ANOTHER OBJECT</color>");
                return false;
            }
        }
        tile.AddObject(this);
        currentTile.RemoveObject();
        currentTile = tile;
        transform.position = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
        xPos = (int)transform.position.x;
        zPos = (int)transform.position.z;
        return true;
    }
}