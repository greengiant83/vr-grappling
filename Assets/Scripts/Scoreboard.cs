using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    public TextMeshPro[] Labels;

    public static Scoreboard Instance;

    float timerStartTime;
    bool isRunning;

	void Start ()
    {
        Instance = this;
	}
	
	void Update ()
    {
        if(isRunning)
        {
            var elapsedTime = Time.fixedTime - timerStartTime;
            var text = elapsedTime.ToString("00.00");
            foreach(var label in Labels)
            {
                label.text = text;
            }
        }
	}

    public void StartHangtime()
    {
        timerStartTime = Time.fixedTime;
        isRunning = true;
    }

    public void StopHangtime()
    {
        isRunning = false;
    }
}
