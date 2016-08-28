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

public class SoundManager : MonoBehaviour
{

    [SerializeField]
    private AudioSource[] sounds;

    public static SoundManager main;

    [SerializeField]
    private AudioSource musicTheme;
    bool muted = false;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (GameObject.FindGameObjectsWithTag("SoundManager").Length < 1)
        {
            gameObject.tag = "SoundManager";
            if (!muted)
            {
                musicTheme.Play();
            }
            main = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.M))
        {
            ToggleMute();
        }
    }

    public void ToggleMute()
    {
        muted = !muted;
        if(muted == true)
        {
            AudioListener.volume = 0f;
            musicTheme.Pause();
        } else
        {
            AudioListener.volume = 1f;
            musicTheme.UnPause();
        }
        UIManager.main.ToggleMute(muted);
    }

    public void PlaySound(SoundClip soundClip)
    {
        if (!muted)
        {
            sounds[(int)soundClip].Play();
        }
    }
}
