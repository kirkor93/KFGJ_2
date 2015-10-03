using UnityEngine;
using System.Collections;
using System;

public class Player : Humanoid
{
    [Header("Player")]
    public float MaxHP = 5000.0f;
    public float Speed = 5.0f;
    public float Cooldown = 0.3f;

    public GameObject BulletPrefab;

    private PlayerState _myState;
    private PlayerInput _myInput;
    private Rigidbody2D _myBody;

    private float timeElapsed = 0.0f;

	// Use this for initialization
	void Start ()
    {
        timeElapsed = Cooldown;
        _myState = GetComponent<PlayerState>();
        _myInput = GetComponent<PlayerInput>();
        _myBody = GetComponent<Rigidbody2D>();
	}
    
    protected override void OnUpdate()
    {
        _myBody.velocity = _myInput.GetMovementVector() * Speed;

        Vector3 pos = GameController.Instance.MainCamera.WorldToScreenPoint(transform.position);
        Vector3 dir = Input.mousePosition - pos;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        _myBody.rotation = angle;

        timeElapsed += Time.deltaTime;

        if (_myInput.IsShooting())
        {
            Shoot();
        }
        if(Input.GetKey(KeyCode.K))
        {
            Hit(Vector3.zero, MaxHP, this);
        }
    }

    protected override void OnHit(Vector3 direction, float damageValue, Humanoid predator)
    {
        if(GameController.Instance.OnPlayerHPChanged != null)
        {
            GameController.Instance.OnPlayerHPChanged();
        }
    }

    protected override void OnDie()
    {
        GameController.Instance.PlayerDead();
    }

    void Shoot()
    {
        if(timeElapsed < Cooldown)
        {
            return;
        }

        timeElapsed = 0.0f;

        GameObject bullet = Instantiate(BulletPrefab, transform.position, transform.rotation) as GameObject;
        bullet.GetComponent<Bullet>().Shoot(transform.right);
        Destroy(bullet, 0.25f);
    }
}
