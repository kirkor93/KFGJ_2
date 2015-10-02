using UnityEngine;
using System.Collections;

public abstract class Humanoid : MonoBehaviour
{
    protected float _hp = 100.0f;
    protected bool _isDead = false;
    protected SpriteRenderer _myRenderer;
    protected BoxCollider2D[] _myColliders;

    void Start()
    {
        _myRenderer = GetComponent<SpriteRenderer>();
        if(_myRenderer == null)
        {
            _myRenderer = GetComponentInChildren<SpriteRenderer>();
        }
        _myColliders = GetComponents<BoxCollider2D>();
    }

    protected void Update()
    {
        if(_isDead)
        {
            return;
        }

        OnUpdate();
    }

    protected abstract void OnUpdate();

    public void Hit(Vector3 direction, float damageValue)
    {
        if(_isDead)
        {
            return;
        }

        _hp -= damageValue;
        OnHit(direction, damageValue);
        if(_hp <= 0.0f)
        {
            Die();
        }
    }

    protected abstract void OnHit(Vector3 direction, float damageValue);

    protected void Die()
    {
        _isDead = true;
        StartCoroutine(OnDie());
    }

    protected IEnumerator OnDie()
    {
        foreach(BoxCollider2D col in _myColliders)
        {
            col.enabled = false;
        }
        int count = 0;
        while(count < 7)
        {
            Color c = _myRenderer.color;
            c.a = 1.0f;
            _myRenderer.color = c;
            float timer = 0.0f;
            while (timer <= 1.0f)
            {
                timer += Time.deltaTime * (count + 7) * 0.5f;
                yield return null;
            }
            c.a = 0.0f;
            _myRenderer.color = c;
            ++count;
            yield return new WaitForSeconds(0.2f);
        }
        Color c1 = _myRenderer.color;
        c1.a = 0.0f;
        _myRenderer.color = c1;
        foreach (BoxCollider2D col in _myColliders)
        {
            col.enabled = true;
        }
        gameObject.SetActive(false);
    }
}
