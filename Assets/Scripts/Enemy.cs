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
    public float Speed = 5.0f;
    public float HitImpactMultiplier = 0.01f;

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

        direction = (GameController.Instance.Player.transform.position - transform.position);
        if(direction.magnitude < 1.5f)
        {
            ChangeState(State.ATTACK);
        }
        else
        {
            ChangeState(State.MOVE_TO_TARGET);
        }

        if(_currentState == State.MOVE_TO_TARGET)
        {
            transform.position += direction.normalized * Speed * Time.deltaTime;
        }
        else
        {
            //Attack target
        }
    }

    protected override void OnHit(Vector3 direction, float damageValue)
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
}
