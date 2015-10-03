using UnityEngine;
using System.Collections;
using System;

public enum State
{
    MOVE_TO_TARGET,
    ATTACK
}

public class Enemy : Humanoid
{
    [Header("Enemy")]
    public float MinSpeed = 4.0f;
    public float MaxSpeed = 7.0f;
    public float HitImpactMultiplier = 0.01f;
    public float Damage = 10.0f;

    //Management variables
    private State _currentState = State.MOVE_TO_TARGET;

    //Hit variables
    private float _hitTimer = 0.0f;
    private Vector3 _targetPosition;
    private Vector3 _myPosition;
    private bool _isHit;
    private float _hitMultiplier = 5.0f;

    //Moving variables
    private Vector3 direction = Vector3.zero;
    private float _speed = 0.0f;
    private bool _lookRight = false;
    [SerializeField]
    private Rigidbody2D _myBody;

    //Attack variables
    private Humanoid _target;

    //Animations
    [SerializeField]
    private Animator _myAnimator;

    void Start()
    {
        _speed = UnityEngine.Random.Range(MinSpeed, MaxSpeed);
        _target = GameController.Instance.Player.GetComponent<Humanoid>();
        InvokeRepeating("Attack", 1.0f, 1.0f);
    }

    void FixedUpdate()
    {
        if(IsDead)
        {
            return;
        }

        direction = (_target.transform.position - transform.position);
        if (direction.x > 0.0f && !_lookRight)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1.0f;
            transform.localScale = scale;
            _lookRight = true;
        }
        else if (direction.x < 0.0f && _lookRight)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1.0f;
            transform.localScale = scale;
            _lookRight = false;
        }
        if (direction.magnitude < 2.5f)
        {
            ChangeState(State.ATTACK);
        }
        else
        {
            ChangeState(State.MOVE_TO_TARGET);
        }

        if (_currentState == State.MOVE_TO_TARGET)
        {
            //_myBody.velocity = direction.normalized * _speed;
            transform.position += direction.normalized * _speed * Time.deltaTime;
        }
    }

    protected override void OnUpdate()
    {
        //Update self
        if (_isHit)
        {
            _hitTimer += (1 + _hitMultiplier) * Time.deltaTime;
            _hitMultiplier = Mathf.Clamp(_hitMultiplier - 10.0f * Time.deltaTime, 0.0f, float.MaxValue);
            transform.position = Vector3.Lerp(_myPosition, _targetPosition, _hitTimer);

            if(_hitTimer >= 1.0f)
            {
                _isHit = false;
            }

            return;
        }

        if(_target == null)
        {
            return;
        }

        _myAnimator.SetBool("isAttacking", _currentState == State.ATTACK);
    }

    protected override void OnHit(Vector3 direction, float damageValue, Humanoid predator)
    {
        _isHit = true;
        _myPosition = transform.position;
        _targetPosition = transform.position + direction * damageValue * HitImpactMultiplier;
        _hitTimer = 0.0f;
        _hitMultiplier = 5.0f;
    }

    public void ChangeState(State newState)
    {
        if(_currentState != newState)
        {
            _currentState = newState;
        }
    }

    protected override void OnDie()
    {
        _myBody.Sleep();
        Spawner.Instance.EnemyDead();
    }

    public bool SetTarget(Humanoid target)
    {
        if(_target != null && !_target.IsDead && _target.gameObject.layer == LayerMask.NameToLayer("Damagable"))
        {
            return false;
        }

        _target = target;
        if(_target == null)
        {
            _target = GameController.Instance.Player.GetComponent<Humanoid>();
            return false;
        }

        return true;
    }

    public Humanoid GetTarget()
    {
        return _target;
    }

    protected void Attack()
    {
        if(_currentState != State.ATTACK)
        {
            return;
        }

        if(_target != null)
        {
            Vector3 direction = _target.transform.position - transform.position;
            direction.Normalize();
            _target.Hit(direction, Damage, this);
        }
    }
}
