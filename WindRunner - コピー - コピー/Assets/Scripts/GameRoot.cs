using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameRoot : MonoBehaviour
{
    // 経過時間を保持
    public float step_timer = 0.0f;         
    public PlayerControl player = null;

    private void Start()
    {
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl>();
    }

    private void Update()
    {
        // 刻々と経過時間を足していく
        this.step_timer += Time.deltaTime;

        //SceneManager.LoadScene("TitleScene");
    }

    public float getPlayTime()
    {
        float time;
        time = this.step_timer;

        // 呼び出しもとに、経過時間を教えてあげる
        return (time);                      
    }
}
