// Date   : 28.08.2016 16:49
// Project: LD36
// Author : bradur

using UnityEngine;
using System.Collections;

public enum SoundClip
{
    None,
    MoveBlock,
    CantMove,
    PlayerFall,
    BlockFall,
    ArrowShoot,
    ArrowHit,
    PlayerHit,
    LevelEnd,
    MoveTreasure,
    TreasureBreak,
    GainItem,
    UnlockDoor
}

public class SoundManager : MonoBehaviour {

    [SerializeField]
    private AudioSource[] sounds;

    public static SoundManager main;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (GameObject.FindGameObjectsWithTag("SoundManager").Length < 1)
        {
            gameObject.tag = "SoundManager";
            main = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySound(SoundClip soundClip)
    {
        sounds[(int)soundClip].Play();
    }
}
