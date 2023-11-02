using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TitleUIController : MonoBehaviour
{
    public static TitleUIController Instance;

    [SerializeField] GameObject _inTitle;

    [SerializeField] GameObject _loadingData;
    [SerializeField] TMP_Text _loadingText;
    [SerializeField] GameObject _failLoading;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    public IEnumerator LoadingTextCor()
    {
        int count = 0;

        while(true)
        {
            switch(count)
            {
                case 0:
                    _loadingText.text = "Loading";
                    break;
                case 1:
                    _loadingText.text = "Loading .";
                    break;
                case 2:
                    _loadingText.text = "Loading . .";
                    break;
                case 3:
                    _loadingText.text = "Loading . . .";
                    break;
            }

            count++;

            if (count > 3)
                count = 0;

            yield return new WaitForSeconds(0.5f);
        }
    }

    public void StartLoading()
    {
        _loadingData.SetActive(true);

        StartCoroutine(LoadingTextCor());
    }

    public void CompleteLoadingData()
    {
        StopCoroutine(LoadingTextCor());

        _loadingData.SetActive(false);
    }

    public void FailLoadingData()
    {
        StopCoroutine(LoadingTextCor());

        _failLoading.SetActive(true);
    }
}
