using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public float Speed = 5.0f;

    private PlayerState _myState;
    private PlayerInput _myInput;

	// Use this for initialization
	void Start ()
    {
        _myState = GetComponent<PlayerState>();
        _myInput = GetComponent<PlayerInput>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.position += _myInput.GetMovementVector() * Time.deltaTime * Speed;
	}
}
