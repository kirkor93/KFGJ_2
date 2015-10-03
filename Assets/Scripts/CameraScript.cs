using UnityEngine;
using System.Collections;

public class CameraScript : MonoBehaviour
{
    public Transform CameraRestrictionMin;
    public Transform CameraRestrictionMax;

    private Transform _target;

    void Start()
    {
        _target = GameController.Instance.Player.transform;
    }

	// Update is called once per frame
	void LateUpdate ()
    {
        Vector3 position = _target.position;
        position.z = -10;
        position.x = Mathf.Clamp(position.x, CameraRestrictionMin.position.x, CameraRestrictionMax.position.x);
        position.y = Mathf.Clamp(position.y, CameraRestrictionMin.position.y, CameraRestrictionMax.position.y);
        transform.position = position;
    }
}
