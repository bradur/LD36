// Date   : 27.08.2016 14:09
// Project: LD36
// Author : bradur

using UnityEngine;
using System.Collections;

public enum ProjectileHeading
{
    None,
    North,
    East,
    South,
    West
}

public class Projectile : MonoBehaviour
{

    [SerializeField]
    private Rigidbody rb;

    [SerializeField]
    private ProjectileHeading projectileHeading = ProjectileHeading.North;

    float initialCheck = 0.5f;
    float checkInterval = 0.25f;
    float timer = 0f;
    bool firstCheck = true;

    public void Init(float speed, ProjectileHeading projectileHeading)
    {
        this.projectileHeading = projectileHeading;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, GameManager.Rotations[(int)projectileHeading], transform.eulerAngles.z);
        rb.velocity = transform.forward * speed;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer > initialCheck && firstCheck)
        {
            firstCheck = false;
            Check();
            timer = 0f;
        } else if(timer > checkInterval)
        {
            Check();
            timer = 0f;
        }
    }

    void Check()
    {
        SingleTile singleTile = TileManager.main.GetTile((int)transform.position.x, (int)transform.position.z);
        if(singleTile.TileObject != null && singleTile.TileObject.ObjectType == ObjectType.MovableBlock)
        {
            Debug.Log("Projectile killed by check!");
            SoundManager.main.PlaySound(SoundClip.ArrowHit);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        SoundManager.main.PlaySound(SoundClip.ArrowHit);
        Destroy(gameObject);
    }
}
