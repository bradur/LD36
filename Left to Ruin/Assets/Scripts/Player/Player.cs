// Date   : 27.08.2016 09:51
// Project: LD36
// Author : bradur

using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    private int xPos;
    private int zPos;
    public int XPos { get { return xPos; } }
    public int ZPos { get { return zPos; } }

    public void Init(int x, int z)
    {
        xPos = x;
        zPos = z;
        transform.position = new Vector3(x, transform.position.y, z);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
        {
            Move(0, 1);
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {
            Move(0, -1);
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            Move(-1, 0);
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            Move(1, 0);
        }
    }

    bool Move(int x, int z)
    {
        int newXPos = (int)transform.position.x + x;
        int newZPos = (int)transform.position.z + z;

        SingleTile tile = TileManager.main.GetTile(newXPos, newZPos);
        if (tile != null)
        {
            if (tile.TileType == TileType.Wall)
            {
                Debug.Log("<b>move:</b> [" + newXPos + ", " + newZPos + "] <color=red>WALL</color>");
                return false;
            }
            GenericObject tileObject = tile.TileObject;
            if (tileObject != null)
            {
                if(tileObject.ObjectType == ObjectType.MovableBlock)
                {
                    if (!tileObject.Move(x, z))
                    {
                        return false;
                    }
                }
            }
        }
        Debug.Log("<b>move:</b> [" + newXPos + ", " + newZPos + "] <color=green>EMPTY</color>");
        transform.position = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
        xPos = (int)transform.position.x;
        zPos = (int)transform.position.z;
        return true;
    }
}
