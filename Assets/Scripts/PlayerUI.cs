using UnityEngine;
using System.Collections.Generic;
using System.Collections;

struct ScoreChangedStruct
{
    public UnityEngine.UI.Text ScoreText;
    public Color TextColor;
}

public class PlayerUI : MonoBehaviour {

    public UnityEngine.UI.Text ScoreText;
    public UnityEngine.UI.Text ScoreValueChangedTextPrefab;

    private Queue<ScoreChangedStruct> _scoreValueChangedTexts = new Queue<ScoreChangedStruct>();
    private Vector2 _initPosition;
    private Vector2 _targetPosition;

	void OnDisable()
    {
        PlayerState.Instance.OnScoreChange -= OnScoreChange;
    }

    void OnEnable()
    {
        StartCoroutine(WaitToInitialize());

        _initPosition = ScoreValueChangedTextPrefab.rectTransform.anchoredPosition;
        _targetPosition = _initPosition + new Vector2(0, 50.0f);

        if(_scoreValueChangedTexts.Count < 10)
        {
            for(int i = _scoreValueChangedTexts.Count; i < 10; ++i)
            {
                UnityEngine.UI.Text newText = Instantiate<UnityEngine.UI.Text>(ScoreValueChangedTextPrefab);
                newText.rectTransform.SetParent(ScoreValueChangedTextPrefab.rectTransform.parent, false);
                newText.rectTransform.anchoredPosition = ScoreValueChangedTextPrefab.rectTransform.anchoredPosition;
                newText.gameObject.SetActive(false);
                ScoreChangedStruct newStruct = new ScoreChangedStruct();
                newStruct.ScoreText = newText;
                newStruct.TextColor = Color.gray;
                _scoreValueChangedTexts.Enqueue(newStruct);
            }
        }
    }

    IEnumerator WaitToInitialize()
    {
        while(PlayerState.Instance == null)
        {
            yield return null;
        }

        PlayerState.Instance.OnScoreChange += OnScoreChange;

        ScoreText.text = PlayerState.Instance.Score.ToString();
    }

    protected void OnScoreChange(int valueChanged)
    {
        ScoreChangedStruct scs = default(ScoreChangedStruct);
        if(_scoreValueChangedTexts.Count > 0)
        {
            scs = _scoreValueChangedTexts.Dequeue();
        }
        else
        {
            scs = new ScoreChangedStruct();
            UnityEngine.UI.Text newText = Instantiate<UnityEngine.UI.Text>(ScoreValueChangedTextPrefab);
            newText.rectTransform.SetParent(ScoreValueChangedTextPrefab.rectTransform.parent, false);
            newText.rectTransform.anchoredPosition = ScoreValueChangedTextPrefab.rectTransform.anchoredPosition;
            newText.gameObject.SetActive(false);
            scs.ScoreText = newText;
            scs.TextColor = Color.gray;
        }

        scs.TextColor = Color.gray;
        if(valueChanged < 0.0f)
        {
            scs.TextColor = Color.red;
            scs.ScoreText.text = "";
        }
        else if(valueChanged >= 0.0f)
        {
            scs.TextColor = Color.white;
            scs.ScoreText.text = "+";
        }

        scs.ScoreText.text += valueChanged.ToString();
        ScoreText.text = PlayerState.Instance.Score.ToString();

        StartCoroutine(ScoreChanged(scs));

    }

    IEnumerator ScoreChanged(ScoreChangedStruct currentStruct)
    {
        currentStruct.ScoreText.color = currentStruct.TextColor;
        currentStruct.ScoreText.rectTransform.anchoredPosition = _initPosition;
        yield return new WaitForEndOfFrame();
        currentStruct.ScoreText.gameObject.SetActive(true);
        float timer = 0.0f;
        while (timer <= 1.0f)
        {
            timer += Time.unscaledDeltaTime;
            currentStruct.ScoreText.rectTransform.anchoredPosition = Vector2.Lerp(_initPosition, _targetPosition, timer);
            yield return null;
        }

        yield return new WaitForEndOfFrame();
        currentStruct.ScoreText.gameObject.SetActive(false);
        _scoreValueChangedTexts.Enqueue(currentStruct);
    }
}
