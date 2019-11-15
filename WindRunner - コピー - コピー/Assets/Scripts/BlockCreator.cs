using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockCreator : MonoBehaviour
{
    // ブロックを格納する配列
    public GameObject[] blockPrefabs;

    // 作成したブロックの個数
    private int block_count = 0;

    public void createBlock(Vector3 block_position)
    {
        // 作成すべきブロックの種類を求める
        int next_block_type = this.block_count % blockPrefabs.Length;

        // ブロックを生成し、goに保管
        GameObject go = GameObject.Instantiate(this.blockPrefabs[next_block_type]) as GameObject;

        // ブロックの位置を移動
        go.transform.position = block_position;

        // ブロックのカウントをインクリメント
        this.block_count++;
    }
}
