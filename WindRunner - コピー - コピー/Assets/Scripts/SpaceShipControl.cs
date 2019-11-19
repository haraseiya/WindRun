using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShipControl : MonoBehaviour
{
    private GameObject player = null;
    private Vector3 position_offset = Vector3.zero;

    private float AIRCRAFT_HEIGHT = 20.0f;      // 機体の高さ

    public float amplitude = 0.01f;             // 振幅
    private int frameCnt = 0;                   // フレームカウント

    void Start()
    {
        // メンバー変数Playerに、PlayerTagのオブジェクトを取得
        this.player = GameObject.FindGameObjectWithTag("Player");

        // カメラの位置とプレイヤーの位置の差分を保管
        this.position_offset = this.transform.position - this.player.transform.position;
    }

    void LateUpdate()
    {
        // カメラの現在位置をnew_positionに取得
        Vector3 new_position = this.transform.position;

        // プレイヤーのX座標に差分を足して、変数new_positionのXに代入する
        new_position.x = this.player.transform.position.x + this.position_offset.x;

        // 縦振幅の計算
        new_position.y = AIRCRAFT_HEIGHT + Mathf.PingPong(Time.time, amplitude);

        // カメラの位置を、新しい位置(new_position)に更新
        this.transform.position = new_position;
    }
}
