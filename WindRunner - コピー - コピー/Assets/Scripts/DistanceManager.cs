using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DistanceManager: MonoBehaviour
{
    private GameObject dis_object = null;
    public GameObject dis_report = null;
    private GameObject player = null;
    float player_dis;
    float time = 0.0f;

    void Start()
    {
        dis_object = GameObject.Find("DistanceText");
        player = GameObject.Find("Player");
    }

    void Update()
    {
        time += Time.deltaTime;

        Text score_text = dis_object.GetComponent<Text>();
        Text dis_text = dis_report.GetComponent<Text>();
        player_dis = (int)player.transform.position.x;

        score_text.text = player_dis + "m";
        
        // 50m毎に報告する
        if (player_dis != 0 && player_dis % 50 == 0) 
        {
            dis_text.text = player_dis + "m";
            dis_report.SetActive(true);
            time = 0.0f;
        }
        // 2秒たったらfalse
        else if(time>=2.0f)
        {
            dis_report.SetActive(false);
        }
    }
}
