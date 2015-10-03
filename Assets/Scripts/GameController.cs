using UnityEngine;
using System.Collections;
using System;

public enum Period
{
    DAY,
    NIGHT
}

public class GameController : Singleton<GameController>
{
    public Camera MainCamera;
    public GameObject Player;

    public float DayTime = 120.0f;
    public float NightTime = 60.0f;

    public Period CurrentPeriod = Period.NIGHT;

    public float Timer
    {
        get;
        protected set;
    }
    public float PeriodTime
    {
        get;
        protected set;
    }

    public Action OnDay;
    public Action OnNight;
    public Action OnGameOver;
    public Action OnPlayerHPChanged;

    void Start()
    {
        CurrentPeriod = Period.NIGHT;
        PeriodTime = NightTime;
        Timer = 0.0f;
        if(OnNight != null)
        {
            OnNight();
        }
    }

    void Update()
    {
        Timer += Time.deltaTime;
        if(Timer > PeriodTime)
        {
            ChangePeriod();
        }
    }

    private void ChangePeriod()
    {
        if(CurrentPeriod == Period.NIGHT)
        {
            Timer = 0.0f;
            PeriodTime = DayTime;
            CurrentPeriod = Period.DAY;
            if(OnDay != null)
            {
                OnDay();
            }
        }
        else
        {
            Timer = 0.0f;
            PeriodTime = NightTime;
            CurrentPeriod = Period.NIGHT;
            if(OnNight != null)
            {
                OnNight();
            }
        }
    }

    public void PlayerDead()
    {
        Time.timeScale = 0;
        if(OnGameOver != null)
        {
            OnGameOver();
        }
    }
}
