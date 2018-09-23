using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public TextMeshPro TimeLabel;
    public TextMeshPro ScoreLabel;
    public AudioClip GameOverSound;
    public AudioClip GameBeginSound;
    public AudioClip FailSound;

    public static GameManager Instance;

    float gameLength = 3 * 60;
    float startTime;
    float endTime;
    int score = 0;
    bool isGameRunning = false;
    AudioSource audioSource;

	void Start ()
    {
        Instance = this;
        audioSource = GetComponent<AudioSource>();
        //StartGame();
	}
	
	void Update ()
    {
        if(isGameRunning)
        {
            var timeRemaining = TimeSpan.FromSeconds(endTime - Time.fixedTime);
            if (timeRemaining.TotalSeconds > 0)
            {
                TimeLabel.text = "Time: " + timeRemaining.Minutes.ToString("D2") + ":" + timeRemaining.Seconds.ToString("D2") + "." + timeRemaining.Milliseconds.ToString("D3");
            }
            else EndGame(false);
        }
	}

    public void AddPoints(int points)
    {
        if (!isGameRunning) return;

        score += points;
        if (score < 0) score = 0;

        ScoreLabel.text = "Score: " + score.ToString("#,##0");
    }

    public void StartGame()
    {
        score = 0;
        ScoreLabel.text = "Score: 0";
        startTime = Time.fixedTime;
        endTime = startTime + gameLength;
        isGameRunning = true;
        audioSource.PlayOneShot(GameBeginSound);
    }

    public void EndGame(bool isFail)
    {
        if (!isGameRunning) return;

        isGameRunning = false;
        TimeLabel.text = "Game Over";
        audioSource.PlayOneShot(isFail ? FailSound : GameOverSound);
    }
}
