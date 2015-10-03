using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyCircle : MonoBehaviour
{
    private Enemy _enemyScript;
    private List<Humanoid> _targets = new List<Humanoid>();

    void Start()
    {
        _enemyScript = GetComponentInParent<Enemy>();
    }

    void Update()
    {
        Humanoid target = _enemyScript.GetTarget();
        if(target.IsDead)
        {
            _enemyScript.SetTarget(null);
            if(_targets.Count > 0)
            {
                if(_enemyScript.SetTarget(_targets[0]))
                {
                    _targets.Remove(_targets[0]);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Damagable"))
        {
            if (col.gameObject.GetComponent<Enemy>() != null)
            {
                return;
            }

            Humanoid target = col.GetComponent<Humanoid>();
            if (target == null || target.IsPlayer)
            {
                return;
            }

            if (!_enemyScript.SetTarget(target) && target != null)
            {
                _targets.Add(target);
            }
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if (col.gameObject.layer == LayerMask.NameToLayer("Damagable"))
        {
            if (col.gameObject.GetComponent<Enemy>() != null)
            {
                return;
            }

            Humanoid target = col.GetComponent<Humanoid>();
            if (target.IsPlayer)
            {
                return;
            }

            if (_targets.Contains(target))
            {
                _targets.Remove(target);
            }
            
            _enemyScript.SetTarget(null);

            if(_targets.Count > 0)
            {
                if (_enemyScript.SetTarget(_targets[0]))
                {
                    _targets.Remove(_targets[0]);
                }
            }
        }
    }
}
