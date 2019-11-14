﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class TitleScript : MonoBehaviour
{
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("WindRun");
        }
    }

    private void OnGUI()
    {
        GUI.Label(new Rect(Screen.width/2,Screen.height/2,128,32),"WindRun");
    }
}
