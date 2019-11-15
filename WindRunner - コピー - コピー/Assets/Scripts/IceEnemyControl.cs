using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceEnemyControl : MonoBehaviour
{
    // 死んだときに発生させるエフェクト
    [SerializeField]
    GameObject DeathEffect;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "FireBullet")
        {
            Destroy(this.gameObject);
            Instantiate(DeathEffect, this.transform.position, Quaternion.Euler(0, 90, 90));
        }
    }
}
