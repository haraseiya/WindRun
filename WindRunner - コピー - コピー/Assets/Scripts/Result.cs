﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Result : MonoBehaviour
{
    private GameObject dis_text=null;
    public PlayerControl player_control = null;
    public GameObject result_screen = null;
    public GameObject result_object = null;

    // プレイヤーの距離
    int player_dis;

    // 経過時間
    float time = 0.0f;

    public void Start()
    {
        this.player_control = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
        this.dis_text = GameObject.Find("DistanceText");
    }

    public void Update()
    {
        this.GameResult();
    }

    public void GameResult()
    {
        // ゲーム終了時
        if (this.player_control.isPlayEnd())
        {
            // 距離テキストを消す
            dis_text.SetActive(false);

            // 結果を出す
            result_screen.SetActive(true);

            // 終了時の距離
            player_dis = (int)player_control.transform.position.x;

            Text result_text = result_object.GetComponent<Text>();

            // 結果テキスト
            result_text.text = player_dis + "m";

            time += Time.deltaTime;

            // 終了後2秒以上経ってるかつマウスが右クリックされたら
            if (time >= 2.0f && Input.GetMouseButtonDown(0))
            {
                SceneManager.LoadScene("TitleScene");
            }
        }
    }
}
