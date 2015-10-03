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
    private Animator _myAnimator;

    private float _timeElapsed = 0.0f;
    private bool _isDay = false;

    private RigidbodyConstraints2D _nightConstraints;
    private RigidbodyConstraints2D _dayConstraints;

	// Use this for initialization
	void Start ()
    {
        _timeElapsed = Cooldown;
        _myState = GetComponent<PlayerState>();
        _myInput = GetComponent<PlayerInput>();
        _myBody = GetComponent<Rigidbody2D>();
        _myAnimator = GetComponent<Animator>();

        _dayConstraints = RigidbodyConstraints2D.FreezeAll;
        _nightConstraints = RigidbodyConstraints2D.FreezeRotation;

        GameController.Instance.OnDay += OnDay;
        GameController.Instance.OnNight += OnNight;
	}

    void OnDay()
    {
        _isDay = true;
        _myBody.velocity = Vector3.zero;
        TreeSprite.SetActive(true);
        HeroSprite.SetActive(false);
        _myBody.constraints = _dayConstraints;
    }

    void OnNight()
    {
        _isDay = false;
        TreeSprite.SetActive(false);
        HeroSprite.SetActive(true);
        _myBody.constraints = _nightConstraints;
    }

    void FixedUpdate()
    {
        if(!_isDay)
        {
            Vector3 movement = _myInput.GetMovementVector();
            _myBody.velocity = movement * Speed;
            _myAnimator.SetBool("isWalking", movement.magnitude > 0.0f);
        }
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

        Vector3 direction = GameController.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        direction.z = 0.0f;
        direction.Normalize();

        GameObject bullet = Instantiate(BulletPrefab, transform.position, transform.rotation) as GameObject;
        bullet.GetComponent<Bullet>().Shoot(direction);
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
