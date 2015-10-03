using UnityEngine;
using System.Collections;
using System;

public class Player : Humanoid
{
    [Header("Player")]
    public float MaxHP = 5000.0f;
    public float Speed = 5.0f;
    public float Cooldown = 0.3f;
    public float SunHP = 50.0f;

    public GameObject BulletPrefab;
    public GameObject TreeSprite;
    public GameObject HeroSprite;

    private PlayerState _myState;
    private PlayerInput _myInput;
    private Rigidbody2D _myBody;

    private float _timeElapsed = 0.0f;
    private bool _isDay = false;

	// Use this for initialization
	void Start ()
    {
        _timeElapsed = Cooldown;
        _myState = GetComponent<PlayerState>();
        _myInput = GetComponent<PlayerInput>();
        _myBody = GetComponent<Rigidbody2D>();

        GameController.Instance.OnDay += OnDay;
        GameController.Instance.OnNight += OnNight;
	}

    void OnDay()
    {
        _isDay = true;
        _myBody.velocity = Vector3.zero;
        _myBody.freezeRotation = true;
        TreeSprite.SetActive(true);
        HeroSprite.SetActive(false);
        transform.rotation = Quaternion.identity;
    }

    void OnNight()
    {
        _isDay = false;
        TreeSprite.SetActive(false);
        HeroSprite.SetActive(true);
    }
    
    protected override void OnUpdate()
    {
        if(_isDay)
        {
            if(_myInput.IsShooting())
            {
                Ray ray = GameController.Instance.MainCamera.ScreenPointToRay(Input.mousePosition);

                RaycastHit2D[] hits = Physics2D.GetRayIntersectionAll(ray);
                foreach(RaycastHit2D hit in hits)
                {
                    if (hit.collider != null && hit.collider.gameObject.layer == LayerMask.NameToLayer("Clickable"))
                    {
                        Clickable clickableComponent = hit.collider.gameObject.GetComponent<Clickable>();
                        if (clickableComponent != null)
                        {
                            clickableComponent.Click();
                        }
                    }
                }
            }
        }
        else
        {
            _myBody.velocity = _myInput.GetMovementVector() * Speed;

            Vector3 pos = GameController.Instance.MainCamera.WorldToScreenPoint(transform.position);
            Vector3 dir = Input.mousePosition - pos;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            _myBody.rotation = angle;

            _timeElapsed += Time.deltaTime;

            if (_myInput.IsShooting())
            {
                Shoot();
            }
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
        if(_timeElapsed < Cooldown)
        {
            return;
        }

        _timeElapsed = 0.0f;

        GameObject bullet = Instantiate(BulletPrefab, transform.position, transform.rotation) as GameObject;
        bullet.GetComponent<Bullet>().Shoot(transform.right);
        Destroy(bullet, 0.25f);
    }

    protected void SunCollected()
    {
        _myState.AddScore(50);
        HP += SunHP;
        if (GameController.Instance.OnPlayerHPChanged != null)
        {
            GameController.Instance.OnPlayerHPChanged();
        }
    }
}
