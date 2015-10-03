using UnityEngine;
using System.Collections;
using System;

public class Sun : Clickable
{
    public AudioClip CollectEffect;
    public float Speed = 3.0f;

    void Update()
    {
        transform.position += Vector3.down * Time.deltaTime * Speed;

        Destroy(gameObject, 10.0f);
    }

    public override void Click()
    {
        AudioSource.PlayClipAtPoint(CollectEffect, GameController.Instance.MainCamera.transform.position);
        GameController.Instance.Player.SendMessage("SunCollected");
        Destroy(gameObject);
    }
}
