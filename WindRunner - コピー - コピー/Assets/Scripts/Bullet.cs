using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float bullet_speed = -0.2f;

    void Update()
    {
        transform.Translate(0, bullet_speed, 0);
        Destroy(this.gameObject, 7.0f);
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
    }
}
