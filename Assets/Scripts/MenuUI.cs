using UnityEngine;
using System.Collections;

public class MenuUI : MonoBehaviour
{
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Time.timeScale = 1.0f;
            Application.LoadLevel("MainScene");
        }
    }
}
