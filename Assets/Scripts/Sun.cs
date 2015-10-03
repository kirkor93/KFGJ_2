using UnityEngine;
using System.Collections;
using System;

public class Sun : Clickable
{
    public float Speed = 3.0f;

    void Update()
    {
        transform.position += Vector3.down * Time.deltaTime * Speed;

        Destroy(gameObject, 10.0f);
    }

    public override void Click()
    {
        GameController.Instance.Player.SendMessage("SunCollected");
        Destroy(gameObject);
    }
}
