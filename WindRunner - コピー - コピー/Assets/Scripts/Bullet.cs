using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    void FixedUpdate()
    {
        float bullet_speed = -30.0f * Time.deltaTime;
        transform.Translate(0, bullet_speed, 0);
        Destroy(this.gameObject, 5.0f);
    }

    void OnCollisionEnter(Collision collision)
    {
        Destroy(this.gameObject);
        Debug.Log("呼ばれたよ");
    }
}
