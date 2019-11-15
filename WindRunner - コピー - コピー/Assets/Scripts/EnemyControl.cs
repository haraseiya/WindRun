using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    // MapCreatorを保管するための変数
    public MapCreator map_creator = null;

    private void Start()
    {
        // MapCreatorを取得して、メンバー変数map_creatorに保管
        map_creator = GameObject.Find("GameRoot").GetComponent<MapCreator>();
    }

    private void Update()
    {
        // 見切れてるなら
        if (this.map_creator.isDelete_enemy(this.gameObject))
        {
            // 自分自身を削除
            GameObject.Destroy(this.gameObject);
        }
    }
}
