using UnityEngine;
using System.Collections;

public abstract class Humanoid : MonoBehaviour
{
    protected float _hp = 100.0f;
    protected bool _isDead = false;

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

    protected virtual void Die()
    {
        _isDead = true;
        StartCoroutine(OnDie(1.0f));
    }

    protected IEnumerator OnDie(float seconds)
    {
        //Die mechanics
        Debug.Log("Dead");
        yield return new WaitForSeconds(seconds);
        this.enabled = false;
    }
}
