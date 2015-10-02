using UnityEngine;
using System.Collections;

public class PlayerInput : MonoBehaviour
{
    public Vector3 GetMovementVector()
    {
        return new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0.0f);
    }
}
