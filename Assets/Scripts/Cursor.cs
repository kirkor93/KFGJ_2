using UnityEngine;
using System.Collections;

public class Cursor : MonoBehaviour
{
    public Sprite DaySprite;
    public Sprite NightSprite;

    private SpriteRenderer _myRenderer;

    void Start()
    {
        _myRenderer = GetComponent<SpriteRenderer>();
        GameController.Instance.OnDay += OnDay;
        GameController.Instance.OnNight += OnNight;
    }

    void OnDay()
    {
        _myRenderer.sprite = DaySprite;
    }

    void OnNight()
    {
        _myRenderer.sprite = NightSprite;
    }

	void Update ()
    {
        Vector3 position = GameController.Instance.MainCamera.ScreenToWorldPoint(Input.mousePosition);
        position.z = 0.0f;
        transform.position = position;
	}
}
