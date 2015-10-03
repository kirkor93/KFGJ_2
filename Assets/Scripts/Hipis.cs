using UnityEngine;
using System.Collections;
using System;

public class Hipis : Humanoid
{
    [Header("Hipis")]
    public float Speed = 6.0f;
    public int ScoreToLoose = 50;

    protected override void OnUpdate()
    {
        //Update hipis
        Vector3 direction = GameController.Instance.Player.transform.position - transform.position;
        if(direction.magnitude > 2.5f)
        {
            transform.position += direction.normalized * Time.deltaTime * Speed;
        }
    }

    protected override void OnHit(Vector3 direction, float damageValue, Humanoid predator)
    {
        //Loose score
        if(predator.IsPlayer)
        {
            PlayerState.Instance.SubstractScore(ScoreToLoose);
        }
    }

    protected override void OnDie()
    {
        Spawner.Instance.HipisDead();
    }

    void Stop()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Damagable"))
        {
            col.gameObject.SendMessage("Stop");
        }
    }
}
