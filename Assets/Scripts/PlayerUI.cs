using UnityEngine;
using System.Collections.Generic;
using System.Collections;

struct ScoreChangedStruct
{
    public UnityEngine.UI.Text ScoreText;
    public Color TextColor;
}

public class PlayerUI : MonoBehaviour
{
    public float GameOverTextSpeed = 5.0f;

    public Sprite DayClockSprite;
    public Sprite NightClockSprite;

    public UnityEngine.UI.Image BloodImage;
    public UnityEngine.UI.Image BackgroundClockImage;
    public UnityEngine.UI.Image ForegroundClockImage;
    public UnityEngine.UI.Image ClockImage;
    public UnityEngine.UI.Image HPBarImage;

    public UnityEngine.UI.Text HPText;
    public UnityEngine.UI.Text GameOverText;
    public UnityEngine.UI.Text YourScoreText;
    public UnityEngine.UI.Text RestartText;
    public UnityEngine.UI.Text ScoreText;
    public UnityEngine.UI.Text ScoreValueChangedTextPrefab;

    private Queue<ScoreChangedStruct> _scoreValueChangedTexts = new Queue<ScoreChangedStruct>();
    private Vector2 _initPosition;
    private Vector2 _targetPosition;

    void Awake()
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

    void Update()
    {
        if(ForegroundClockImage != null)
        {
            ForegroundClockImage.fillAmount = 1.0f - GameController.Instance.Timer / GameController.Instance.PeriodTime;
        }
    }

    void OnDay()
    {
        ForegroundClockImage.sprite = DayClockSprite;
        BackgroundClockImage.sprite = NightClockSprite;
        ForegroundClockImage.fillAmount = 1.0f;
    }

    void OnNight()
    {
        ForegroundClockImage.sprite = NightClockSprite;
        BackgroundClockImage.sprite = DayClockSprite;
        ForegroundClockImage.fillAmount = 1.0f;
    }

    IEnumerator WaitToInitialize()
    {
        while(PlayerState.Instance == null)
        {
            yield return null;
        }

        PlayerState.Instance.OnScoreChange += OnScoreChange;

        ScoreText.text = PlayerState.Instance.Score.ToString();

        while(GameController.Instance == null)
        {
            yield return null;
        }

        GameController.Instance.OnGameOver += ShowGameOver;
        GameController.Instance.OnPlayerHPChanged += UpdateHPInfo;
        GameController.Instance.OnDay += OnDay;
        GameController.Instance.OnNight += OnNight;
        UpdateHPInfo();
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

    void UpdateHPInfo()
    {
        Player player = GameController.Instance.Player.GetComponent<Player>();
        HPText.text = player.HP + " / " + player.MaxHP;
        HPBarImage.fillAmount = player.HP / player.MaxHP;

        float x = player.HP / player.MaxHP;
        if (x < 0.5f)
        {
            Color c = BloodImage.color;
            c.a = -2.0f * x + 1.0f;
            BloodImage.color = c;
        }
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

    void ShowGameOver()
    {
        BackgroundClockImage.gameObject.SetActive(false);
        ForegroundClockImage.gameObject.SetActive(false);
        ClockImage.gameObject.SetActive(false);
        HPBarImage.gameObject.SetActive(false);
        HPText.gameObject.SetActive(false);
        ScoreText.gameObject.SetActive(false);
        YourScoreText.text = "YOUR SCORE: ";
        YourScoreText.text += PlayerState.Instance.Score.ToString();
        YourScoreText.gameObject.SetActive(true);
        RestartText.gameObject.SetActive(true);
        foreach(ScoreChangedStruct scs in _scoreValueChangedTexts)
        {
            scs.ScoreText.gameObject.SetActive(false);
        }
        StartCoroutine(AnimateGameOver());
    }

    IEnumerator AnimateGameOver()
    {
        GameOverText.fontSize = 20;
        GameOverText.gameObject.SetActive(true);

        float timer = 0.0f;
        bool enlarge = true;
        while(true)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Application.LoadLevel("MainMenu");
            }

            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }

            if(enlarge)
            {
                timer += Time.unscaledDeltaTime * GameOverTextSpeed;
            }
            else
            {
                timer -= Time.unscaledDeltaTime * GameOverTextSpeed;
            }

            GameOverText.fontSize = (int)Mathf.Lerp(20, 80, timer);

            if(enlarge && timer > 1.0f)
            {
                enlarge = false;
            }

            if (!enlarge && timer < 0.0f)
            {
                enlarge = true;
            }

            yield return null;
        }
    }
}
