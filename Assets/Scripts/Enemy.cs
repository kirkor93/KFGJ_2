using UnityEngine;
using System.Collections;
using System;

public class Enemy : Humanoid
{
    private float _hitTimer = 0.0f;
    private Vector3 _targetPosition;
    private Vector3 _myPosition;
    private bool _isHit;
    private float _hitMultiplier = 5.0f;

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
    }

    protected override void OnHit(Vector3 direction, float damageValue)
    {
        _isHit = true;
        _myPosition = transform.position;
        _targetPosition = transform.position + direction * damageValue * 0.1f;
        _hitTimer = 0.0f;
        _hitMultiplier = 5.0f;
    }

}
