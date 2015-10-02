using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour {

    public float Speed;
    public float Damage = 5.0f;

    private Rigidbody2D _myBody;

    void Awake()
    {
        _myBody = GetComponent<Rigidbody2D>();
    }

    public void Shoot(Vector3 InDirection)
    {
        _myBody.velocity = InDirection * Speed;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Damagable"))
        {
            col.GetComponent<Humanoid>().Hit(_myBody.velocity.normalized, Damage);
            Destroy(gameObject);
        }
    }
}
