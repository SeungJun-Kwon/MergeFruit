using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] AudioSource _bgmPlayer;
    [SerializeField] List<AudioSource> _sfxPlayer = new();
    [SerializeField] Dictionary<string, AudioClip> _sfxClip = new();

    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;

            if (UIController.Instance != null)
                UIController.Instance.ChangeScoreText(_score.ToString());
        }
    }
    int _score = 0;

    float _totalSeconds = 0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (Instance != this)
            Destroy(this);

        Addressables.LoadAssetsAsync("Audio", (AudioClip result) =>
        {
            _sfxClip.Add(result.name, result);
        });
    }

    private void Start()
    {
        SetVolume(0.5f);
        _bgmPlayer.Play();
        _totalSeconds = 0f;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            UIController.Instance.SetActiveMenu(true);

        if (Time.timeScale != 0 && UIController.Instance != null)
            UIController.Instance.ChangeTimerText(StopwatchTimer());
    }

    string StopwatchTimer()
    {
        _totalSeconds += Time.deltaTime;
        TimeSpan timespan = TimeSpan.FromSeconds(_totalSeconds);
        string timer = string.Format("{0:D2}:{1:D2}.{2:D2}", timespan.Minutes, timespan.Seconds, timespan.Milliseconds / 10);

        return timer;
    }

    public void GameOver()
    {
        PlaySFX("GameOver");
        UIController.Instance.SetActiveGameOver(true);
    }

    public void PlaySFX(AudioClip clip)
    {
        foreach (var p in _sfxPlayer)
        {
            if (!p.isPlaying)
            {
                p.clip = clip;
                p.Play();
            }
        }
    }

    public void PlaySFX(string name)
    {
        if (_sfxClip.TryGetValue(name, out var clip))
            PlaySFX(clip);
    }

    public void SetVolume(float volume)
    {
        _bgmPlayer.volume = volume;

        foreach (var p in _sfxPlayer)
            p.volume = volume;
    }

    public void Restart()
    {
        FruitSpawner.Instance.HideAllObjects();
        Score = 0;
        _totalSeconds = 0f;
    }

    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");

        Score = 0;
        _totalSeconds = 0f;
    }

    public void ExitGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("TitleScene");

        Score = 0;
        _totalSeconds = 0f;
    }

    public void ExitApp()

    {

#if UNITY_EDITOR

        UnityEditor.EditorApplication.isPlaying = false;

#else

        Application.Quit();

#endif

    }
}