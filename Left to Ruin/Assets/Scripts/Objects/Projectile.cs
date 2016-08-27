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

    public void Init(float speed, ProjectileHeading projectileHeading)
    {
        this.projectileHeading = projectileHeading;
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, GameManager.Rotations[(int)projectileHeading], transform.eulerAngles.z);
        rb.velocity = transform.forward * speed;
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
