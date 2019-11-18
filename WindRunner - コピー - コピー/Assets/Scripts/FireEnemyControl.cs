using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireEnemyControl : MonoBehaviour
{
    // 死んだときに発生させるエフェクト
    [SerializeField]
    GameObject DeathEffect;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "IceBullet")
        {
            Destroy(this.gameObject);
            Instantiate(DeathEffect, this.transform.position, Quaternion.identity);
        }
    }
}
