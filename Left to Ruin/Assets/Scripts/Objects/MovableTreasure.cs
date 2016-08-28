// Date   : 27.08.2016 16:03
// Project: LD36
// Author : bradur

using UnityEngine;
using System.Collections;

public enum Item
{
    None,
    RedKey,
    GreenKey,
    BlueKey
}

public class MovableTreasure : MonoBehaviour {

    [SerializeField]
    private GameObject outerShell;
    private bool outerShellAlive = true;
    [SerializeField]
    private Item item;

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Projectile" && outerShellAlive)
        {
            Break();
        }
    }

    void Break()
    {
        SoundManager.main.PlaySound(SoundClip.TreasureBreak);
        Destroy(outerShell);
        GenericObject genericObject = GetComponent<GenericObject>(); 
        genericObject.MakeUnMovable();
        outerShellAlive = false;
    }
    
    public Item GetItem()
    {
        return item;
    }
}
