// Date   : 27.08.2016 14:11
// Project: LD36
// Author : bradur

using UnityEngine;
using System.Collections;

public class ProjectileShooter : MonoBehaviour {

    // make object pool later
    [SerializeField]
    Projectile projectilePrefab;

    [SerializeField]
    [Range(0.5f, 5f)]
    private float projectileInterval = 2f;

    [SerializeField]
    [Range(0f, 20f)]
    private float firstShotTimer = 2f;
    private bool shotFired = false;

    private float timer = 0f;
    [SerializeField]
    [Range(0.5f, 25f)]
    private float speed = 2f;

    private ProjectileHeading projectileHeading;

    void Update()
    {
        timer += Time.deltaTime;
        if (!shotFired && timer > firstShotTimer)
        {
            Shoot();
            shotFired = true;
            timer = 0f;
        }
        else if(timer > projectileInterval)
        {
            Shoot();
            timer = 0f;
        }
    }

    public void Init(ProjectileHeading projectileHeading)
    {
        this.projectileHeading = projectileHeading;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, GameManager.Rotations[(int)projectileHeading], transform.eulerAngles.z);
    }

    public void Shoot()
    {
        SoundManager.main.PlaySound(SoundClip.ArrowShoot);
        Projectile newProjectile = Instantiate(projectilePrefab);
        newProjectile.transform.parent = GameManager.main.ProjectileContainer;
        newProjectile.transform.position = transform.position + (transform.forward);
        newProjectile.Init(speed, projectileHeading);
    }

}
