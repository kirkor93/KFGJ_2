using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PropsGenerator : MonoBehaviour
{
    public List<GameObject> Props;
    public Transform MinPoint;
    public Transform MaxPoint;

    private List<GameObject> _generated = new List<GameObject>();

    [ContextMenu("Generate")]
    void Generate()
    {
        if(_generated.Count > 0)
        {
            for(int i = 0; i < _generated.Count; ++i)
            {
                Destroy(_generated[i]);
            }

            _generated.Clear();
        }

        if(Props != null && Props.Count > 0)
        {
            int propsNumber = Random.Range(20, 50);
            for(int k = 0; k < propsNumber; ++k)
            {
                float x = Random.Range(MinPoint.position.x, MaxPoint.position.x);
                float y = Random.Range(MinPoint.position.y, MaxPoint.position.y);
                int propsIndex = Random.Range(0, Props.Count);
                GameObject go = Instantiate(Props[propsIndex], new Vector3(x, y, 0.0f), Quaternion.identity) as GameObject;
                go.transform.parent = transform;
                _generated.Add(go);
            }
        }
    }
}
