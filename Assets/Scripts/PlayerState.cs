using UnityEngine;
using System.Collections;
using System;

public class PlayerState : Singleton<PlayerState>
{
    //int parameter - score change value
    public Action<int> OnScoreChange;

    private int _score;
    public int Score
    {
        get
        {
            return _score;
        }
        protected set
        {
            if(value != _score)
            {
                int changeValue = value - _score;
                _score = value;

                if (OnScoreChange != null)
                {
                    OnScoreChange(changeValue);
                }
            }
        }
    }

    public float AttackStrength = 15.0f;

    void Start()
    {
        _score = 0;
    }

    public void AddScore(int value)
    {
        Score += Mathf.Abs(value);
    }

    public void SubstractScore(int value)
    {
        Score -= Mathf.Abs(value);
    }
}
