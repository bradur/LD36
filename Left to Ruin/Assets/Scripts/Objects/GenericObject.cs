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
    [SerializeField]
    private bool movable = false;
    public bool Movable { get { return movable; } }

    private Quaternion originalRotation;

    SingleTile currentTile;

    public void Init(int xPos, float yPos, int zPos, ObjectType objectType, PropertyDict properties)
    {
        originalRotation = transform.rotation;
        this.objectType = objectType;
        this.xPos = xPos;
        this.zPos = zPos;
        if (this.objectType == ObjectType.ProjectileShooter)
        {

            ProjectileShooter projectileShooter = GetComponent<ProjectileShooter>();
            projectileShooter.Init((ProjectileHeading)GameManager.IntParseFast(properties["ObjectRotation"]));
        } else if (properties.ContainsKey("ObjectRotation"))
        {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, GameManager.Rotations[GameManager.IntParseFast(properties["ObjectRotation"])], transform.eulerAngles.z);
        }
        transform.position = new Vector3(xPos, yPos, zPos);
        currentTile = TileManager.main.GetTile(xPos, zPos);
        currentTile.AddObject(this);
    }

    public bool Move(int x, int z)
    {
        if (movable && !falling)
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
                    transform.rotation = originalRotation;
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

    bool fallCheckDone = false;
    float timer = 0f;
    float timeToFall = 0.25f;

    void Update()
    {
        if (falling && !fallCheckDone)
        {
            timer += Time.deltaTime;
            if (timer > timeToFall)
            {
                SoundManager.main.PlaySound(SoundClip.BlockFall);
                timer = 0f;
                fallCheckDone = true;
            }
        }
    }

    public void MakeUnMovable()
    {
        movable = false;
    }

    public void RemoveFromTile()
    {
        if (currentTile != null)
        {
            currentTile.RemoveObject();
            currentTile = null;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Hole")
        {
            gameObject.layer = LayerMask.GetMask("Default");
            if (currentTile != null)
            {
                currentTile.RemoveObject();
            }
            falling = false;
            currentTile = null;
            transform.rotation = originalRotation;
        }
        else if (collision.gameObject.tag == "MovableBlock")
        {
            falling = false;
        }
    }
}