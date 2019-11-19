using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCreator : MonoBehaviour
{
    // エネミーを格納する配列
    public GameObject[] enemyPrefabs;

    // 作成したエネミーの個数
    private int enemy_count = 0;

    // エネミーの位置と種類を決めて生成する
    public void createEnemy(Vector3 enemy_position)
    {
        // 作成すべきエネミーの種類を求める
        int next_enemy_type = Random.Range(0,2);

        // エネミーを生成し、goに保管
        GameObject go = GameObject.Instantiate(this.enemyPrefabs[next_enemy_type]) as GameObject;

        // エネミーの位置を移動
        go.transform.position = enemy_position;

        // エネミーのカウントをインクリメント
        this.enemy_count++;
    }

}
