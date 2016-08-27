// Date   : 27.08.2016 16:45
// Project: LD36
// Author : bradur

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class LockedDoor : MonoBehaviour
{
    [SerializeField]
    private Item requiredItem;

    [SerializeField]
    private GameObject door;

    bool locked = true;
    public bool Locked { get { return locked; } }

    public bool UnlockDoor(List<Item> items)
    {
        if (!locked)
        {
            return true;
        }
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == requiredItem)
            {
                GameManager.main.RemoveItem(items[i]);
                Destroy(door);
                return true;
            }
        }
        return false;
    }
}
