using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class Humanoid : MonoBehaviour
{
    [Header("Humanoid things")]
    public float HP = 100.0f;
    public float FloatingSpeed = 5.0f;
    public float FloatingY = 0.3f;
    public Transform _childSpriteObject;
    public bool IsPlayer = false;

    protected bool _isDead = false;
    [SerializeField]
    protected SpriteRenderer _myRenderer;
    protected BoxCollider2D[] _myColliders;

    protected float _timer = 0.0f;
    protected bool _up = true;

    public bool IsDead
    {
        get { return _isDead; }
    }

    void OnEnable()
    {
        if(_myRenderer == null)
        {
            _myRenderer = GetComponent<SpriteRenderer>();
            if (_myRenderer == null)
            {
                _myRenderer = GetComponentInChildren<SpriteRenderer>();
            }
        }
        _myColliders = GetComponents<BoxCollider2D>();
    }

    protected void Update()
    {
        if(_isDead)
        {
            return;
        }
        
        if(!IsPlayer)
        {
            Float();
        }

        OnUpdate();
    }

    protected void Float()
    {
        if (_childSpriteObject != null)
        {
            if (_up)
            {
                _timer += Time.deltaTime * FloatingSpeed;
                if (_timer >= 1.0f)
                {
                    _up = false;
                }
            }
            else
            {
                _timer -= Time.deltaTime * FloatingSpeed;
                if (_timer <= 0.0f)
                {
                    _up = true;
                }
            }
            _childSpriteObject.localPosition = new Vector3(0.0f, Mathf.Lerp(-FloatingY, FloatingY, _timer), 0.0f);
        }
    }

    protected abstract void OnUpdate();

    public void Hit(Vector3 direction, float damageValue, Humanoid predator)
    {
        if(_isDead)
        {
            return;
        }

        HP -= damageValue;
        if(HP < 0.0f)
        {
            HP = 0.0f;
        }
        OnHit(direction, damageValue, predator);
        if(HP <= 0.0f)
        {
            Die();
        }
    }

    protected abstract void OnHit(Vector3 direction, float damageValue, Humanoid predator);

    protected void Die()
    {
        _isDead = true;
        OnDie();
        StartCoroutine(DieEffect());
    }

    protected abstract void OnDie();

    protected IEnumerator DieEffect()
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
