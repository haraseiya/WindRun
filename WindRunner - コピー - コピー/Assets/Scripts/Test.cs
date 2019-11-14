using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Test : MonoBehaviour
{
    int score = 1234;

    private void Update()
    {
        int memo1 = 567;

        score++;
        memo1++;

        if(memo1>100)
        {
            int memo2 = 89;
            memo2++;
        }

        Debug.Log(score);
        Debug.Log(memo1);
    }
}
