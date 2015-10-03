﻿using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class Worm : Clickable
{
    public float Speed = 8.0f;

    private bool _isHold = false;
    private Vector3 _swipeDistance;
    private Vector3 _prevPosition;
    private bool _dieBySwipe = false;

    private Rigidbody2D _myBody;

    void Awake()
    {
        _myBody = GetComponent<Rigidbody2D>();
    }

    public override void Click()
    {
        if(!Input.GetMouseButtonUp(0))
        {
            _isHold = true;
            _swipeDistance = Vector3.zero;
            _prevPosition = transform.position;
            _dieBySwipe = false;
        }
    }

    void Update()
    {
        if(_isHold)
        {
            if(Input.GetMouseButtonUp(0))
            {
                _isHold = false;
                if(_swipeDistance.magnitude > 4.0f)
                {
                    _dieBySwipe = true;
                    if(_swipeDistance.magnitude < 15.0f)
                    {
                        _swipeDistance *= 15.0f / _swipeDistance.magnitude;
                    }
                    _myBody.velocity = _swipeDistance;
                    Destroy(gameObject, 3.0f);
                }
                return;
            }

            Vector3 position = GameController.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
            position.z = 0.0f;
            transform.position = position;
            _swipeDistance += (transform.position - _prevPosition);
            _prevPosition = position;
        }
        else if(!_dieBySwipe)
        {
            Vector3 direction = GameController.Instance.Player.transform.position - transform.position;
            if(direction.magnitude < 1.5f)
            {
                GameController.Instance.Player.GetComponent<Player>().Hit(direction, 5.0f, null);
                Destroy(gameObject);
            }
            direction.Normalize();
            transform.position += direction * Time.deltaTime * Speed;
        }

        if(_dieBySwipe)
        {
            _myBody.rotation += 10.0f;
        }
    }
}
