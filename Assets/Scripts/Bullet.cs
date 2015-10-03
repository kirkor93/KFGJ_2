using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    public AudioClip HitSound;
    public float Speed;
    public float Damage = 5.0f;

    private Rigidbody2D _myBody;
    private Humanoid _playerHumanoidScript;

    void Awake()
    {
        _myBody = GetComponent<Rigidbody2D>();
    }

    public void Shoot(Vector3 InDirection)
    {
        if(_playerHumanoidScript == null)
        {
            _playerHumanoidScript = GameController.Instance.Player.GetComponent<Humanoid>();
        }
        _myBody.velocity = InDirection * Speed;
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Damagable"))
        {
            col.GetComponent<Humanoid>().Hit(_myBody.velocity.normalized, Damage, _playerHumanoidScript);
            AudioSource.PlayClipAtPoint(HitSound, transform.position);
            Destroy(gameObject);
        }
    }
}
