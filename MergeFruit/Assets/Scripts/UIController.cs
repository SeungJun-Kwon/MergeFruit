using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [Header("UI Object")]
    [SerializeField] GameObject _menu;
    [SerializeField] GameObject _gameOver;

    [Header("Upper")]
    [SerializeField] Image _nextFruitImage;
    [SerializeField] TMP_Text _scoreText;
    [SerializeField] TMP_Text _timerText;

    [Header("Lower")]
    [SerializeField] Button _leftButton;
    [SerializeField] Button _rightButton;
    [SerializeField] Button _spawnButton;

    [Space]
    [SerializeField] Transform _arrow;

    bool _leftDown = false, _rightDown = false;

    float _arrowSpeed = 0.1f;
    float _spawnDelay = 0.5f;
    float _xLimit = 2f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    private void Start()
    {
        SetActiveMenu(false);
    }

    private void Update()
    {
#if UNITY_EDITOR
        _arrowSpeed = 0.01f;
#endif

        if (_leftDown && _arrow.position.x > -_xLimit)
            MoveArrow(Vector3.left * _arrowSpeed);

        if(_rightDown && _arrow.position.x < _xLimit)
            MoveArrow(Vector3.right * _arrowSpeed);
    }

    public void ChangeNextFruitImage(Sprite s) => _nextFruitImage.sprite = s;

    public void ChangeScoreText(string s) => _scoreText.text = s;

    public void ChangeTimerText(string s) => _timerText.text = s;

    public void MoveArrow(Vector3 value) => _arrow.position += value;

    public void LeftButtonDown() => _leftDown = true;
    public void LeftButtonUp() => _leftDown = false;

    public void RightButtonDown() => _rightDown = true;
    public void RightButtonUp() => _rightDown = false;

    public void SpawnButton()
    {
        FruitSpawner.Instance.SpawnFruit(_arrow.position.x);

        StartCoroutine(SpawnDelayCor());
    }

    IEnumerator SpawnDelayCor()
    {
        _spawnButton.interactable = false;

        yield return new WaitForSeconds(_spawnDelay);

        _spawnButton.interactable = true;
    }

    public void SetActiveMenu(bool value)
    {
        if(value)
        {
            Time.timeScale = 0;
            _menu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            _menu.SetActive(false);
        }

        _spawnButton.interactable = !value;
    }

    public void SetActiveGameOver(bool value)
    {
        if (value)
        {
            Time.timeScale = 0;
            _gameOver.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            _gameOver.SetActive(false);
        }

        _spawnButton.interactable = !value;
    }

    public void SetVolume(float volume) => GameManager.Instance.SetVolume(volume);

    public void RestartButton()
    {
        GameManager.Instance.Restart();
        _arrow.position = new Vector3(0f, _arrow.position.y, 1f);
        StopCoroutine(SpawnDelayCor());
        _spawnButton.interactable = true;
    }

    public void ExitButton() => GameManager.Instance.ExitGame();
}
