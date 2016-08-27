// Date   : 27.08.2016 12:53
// Project: LD36
// Author : bradur

using UnityEngine;
using System.Collections;
using TiledSharp;

public class GenericObject : MonoBehaviour
{
    [SerializeField]
    private ObjectType objectType;
    public ObjectType ObjectType { get { return objectType; } }

    private int xPos;
    private int zPos;
    public int XPos { get { return xPos; } }
    public int ZPos { get { return zPos; } }
    bool falling = false;

    SingleTile currentTile;

    public void Init(int xPos, int zPos, ObjectType objectType, PropertyDict properties)
    {
        this.objectType = objectType;
        this.xPos = xPos;
        this.zPos = zPos;
        if(this.objectType == ObjectType.ProjectileShooter)
        {

            ProjectileShooter projectileShooter = GetComponent<ProjectileShooter>();
            projectileShooter.Init((ProjectileHeading)GameManager.IntParseFast(properties["ObjectRotation"]));
        }
        transform.position = new Vector3(xPos, 0.75f, zPos);
        currentTile = TileManager.main.GetTile(xPos, zPos);
        currentTile.AddObject(this);
    }

    public bool Move(int x, int z)
    {
        if (objectType == ObjectType.MovableBlock && !falling)
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
                if (tile.TileType == TileType.Hole)
                {
                    falling = true;
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
        return false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Hole")
        {
            gameObject.layer = LayerMask.GetMask("Default");
            currentTile.RemoveObject();
            falling = false;
            currentTile = null;
        }
        else if (collision.gameObject.tag == "MovableBlock")
        {
            falling = false;
        }
    }
}