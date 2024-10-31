using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    public static Timer Instance { get; private set; }

    private TextMeshProUGUI text;
    private float timeRemaining;
    private bool timerRunning;
    private Action timeUpCallback;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        text = GetComponent<TextMeshProUGUI>();
    }

    public void StartTimer(float duration, Action callback = null)
    {
        timeRemaining = duration;
        timerRunning = true;
        timeUpCallback = callback;
    }

    public void StopTimer()
    {
        timerRunning = false;
    }

    public bool IsTimeUp()
    {
        return timeRemaining <= 0;
    }

    void Update()
    {
        if (!timerRunning) return;

        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining < 0) timeRemaining = 0;
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            text.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            timeRemaining = 0;
            timerRunning = false;
            timeUpCallback?.Invoke();
        }
    }
}