using UnityEngine;
using System.Collections;
using System;

public class Sun : Clickable
{
    public override void Click()
    {
        GameController.Instance.Player.SendMessage("SunCollected");
        Destroy(gameObject);
    }
}
