using UnityEngine;
using System.Collections;

public abstract class Clickable : MonoBehaviour
{
    void Start()
    {
        GameController.Instance.OnNight += OnNight;
    }

    void OnDestroy()
    {
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
