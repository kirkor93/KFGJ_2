﻿using UnityEngine;
using System.Collections;
using System;

public class Hipis : Humanoid
{
    public int ScoreToLoose = 50;

    protected override void OnUpdate()
    {
        //Update hipis
    }

    protected override void OnHit(Vector3 direction, float damageValue)
    {
        //Loose score
        PlayerState.Instance.SubstractScore(ScoreToLoose);
    }
}
