using UnityEngine;
using System.Collections;

public abstract class Clickable : MonoBehaviour
{
    protected bool _appQuiting = false;

    void Start()
    {
        GameController.Instance.OnNight += OnNight;
    }

    void OnApplicationQuit()
    {
        _appQuiting = true;
    }

    void OnDestroy()
    {
        if(_appQuiting)
        {
            return;
        }

        GameController.Instance.OnNight -= OnNight;
    }

    public abstract void Click();
    
    private void OnNight()
    {
        if(gameObject != null)
        {
            Destroy(gameObject);
        }
    }
}
