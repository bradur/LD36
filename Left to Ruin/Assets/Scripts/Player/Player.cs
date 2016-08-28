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

    private bool falling = false;
    float timeToFall = 0.2f;
    bool fallCheckDone = false;
    float timer;

    public void Init(int x, int z)
    {
        xPos = x;
        zPos = z;
        transform.position = new Vector3(x, transform.position.y, z);
    }

    void Update()
    {
        if (falling && !fallCheckDone)
        {
            timer += Time.deltaTime;
            if(timer > timeToFall)
            {
                SoundManager.main.PlaySound(SoundClip.PlayerFall);
                timer = 0f;
                fallCheckDone = true;
            }
        }
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
        {
            if (!Move(0, 1))
            {
                SoundManager.main.PlaySound(SoundClip.CantMove);
            }
        }
        else if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
        {
            if (!Move(0, -1))
            {
                SoundManager.main.PlaySound(SoundClip.CantMove);
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
        {
            if(!Move(-1, 0))
            {
                SoundManager.main.PlaySound(SoundClip.CantMove);
            }
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
        {
            if(!Move(1, 0))
            {
                SoundManager.main.PlaySound(SoundClip.CantMove);
            }
        }
    }

    bool Move(int x, int z)
    {
        if (Time.timeScale < 1f)
        {
            return false;
        }
        if (!falling)
        {
            int newXPos = (int)transform.position.x + x;
            int newZPos = (int)transform.position.z + z;

            SingleTile tile = TileManager.main.GetTile(newXPos, newZPos);
            if (tile != null)
            {
                if (tile.TileType == TileType.Wall)
                {
                    //Debug.Log("<b>move:</b> [" + newXPos + ", " + newZPos + "] <color=red>WALL</color>");
                    return false;
                }
                GenericObject tileObject = tile.TileObject;
                if (tileObject != null)
                {
                    if (tileObject.Movable)
                    {
                        if (!tileObject.Move(x, z))
                        {
                            return false;
                        }
                        if (tileObject.ObjectType == ObjectType.MovableBlock)
                        {
                            SoundManager.main.PlaySound(SoundClip.MoveBlock);
                        }
                        else
                        {
                            SoundManager.main.PlaySound(SoundClip.MoveTreasure);
                        }
                    }
                    else if (
                        tileObject.ObjectType == ObjectType.MovableTreasureRed ||
                        tileObject.ObjectType == ObjectType.MovableTreasureBlue ||
                        tileObject.ObjectType == ObjectType.MovableTreasureGreen
                    )
                    {
                        GameManager.main.GainItem(tileObject.GetComponent<MovableTreasure>().GetItem());
                        SoundManager.main.PlaySound(SoundClip.GainItem);
                        tileObject.RemoveFromTile();
                        Destroy(tileObject.gameObject);
                    }
                    else if (
                        tileObject.ObjectType == ObjectType.LockedDoorRed ||
                        tileObject.ObjectType == ObjectType.LockedDoorGreen ||
                        tileObject.ObjectType == ObjectType.LockedDoorBlue)
                    {
                        if (!tileObject.GetComponent<LockedDoor>().UnlockDoor(GameManager.main.Items))
                        {
                            return false;
                        }
                        SoundManager.main.PlaySound(SoundClip.UnlockDoor);
                        tileObject.RemoveFromTile();
                    }
                    else if (tileObject.ObjectType == ObjectType.ProjectileShooter)
                    {
                        return false;
                    }
                    else if (tileObject.ObjectType == ObjectType.LevelEnd)
                    {
                        SoundManager.main.PlaySound(SoundClip.LevelEnd);
                        GameManager.main.FinishLevel();
                    }
                }
            }
            if (tile.TileType == TileType.Hole)
            {
                falling = true;
            }
            //Debug.Log("<b>move:</b> [" + newXPos + ", " + newZPos + "] <color=green>EMPTY</color>");
            transform.position = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + z);
            xPos = (int)transform.position.x;
            zPos = (int)transform.position.z;
            return true;
        }
        return false;
    }

    void Die()
    {
        GameManager.main.GameOver();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Projectile")
        {
            SoundManager.main.PlaySound(SoundClip.PlayerHit);
            Die();
        }
        else if (collision.gameObject.tag == "Hole")
        {
            Die();
        }
        else if (collision.gameObject.tag == "MovableBlock")
        {
            falling = false;
            timer = 0f;
        }
    }
}
