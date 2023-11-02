using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FruitManager : MonoBehaviour
{
    public static FruitManager Instance;

    public FruitDatas _fruitDatas;

    public Dictionary<string, Sprite> _fruitSprites = new();

    const string _fruitURL = "https://script.google.com/macros/s/AKfycby-IJILHDTM5CVoxU9d3QWnuvPL5VJmeZx1aTojeNxyG2vhEiQl8c4iMjTlmDDjiouu/exec";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (Instance != this)
            Destroy(this);
    }

    private IEnumerator Start()
    {
        TitleUIController.Instance.StartLoading();

        using (UnityWebRequest w = UnityWebRequest.Get(_fruitURL))
        {
            yield return w.SendWebRequest();

            if (w.isDone)
            {
                string jsonData = w.downloadHandler.text;
                jsonData = "{\"fruit\":" + jsonData + "}";

                _fruitDatas = JsonUtility.FromJson<FruitDatas>(jsonData);

                foreach (var f in _fruitDatas.fruit)
                {
                    var op = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<Sprite>($"Assets/Sprites/Fruits/{f._fruitSprite}.png");
                    yield return op;

                    if (op.IsDone)
                        _fruitSprites.Add(f._fruitName, op.Result);
                }

                TitleUIController.Instance.CompleteLoadingData();
            }
            else
            {
                TitleUIController.Instance.FailLoadingData();
            }
        }
    }
}

[System.Serializable]
public class FruitDatas
{
    public List<FruitData> fruit;
}

[System.Serializable]
public class FruitData
{
    public int _fruitId;
    public string _fruitName;
    public float _fruitSize;
    public int _fruitScore;
    public string _fruitSprite;

    public override string ToString()
    {
        return $"{_fruitId} {_fruitName} {_fruitSize} {_fruitScore} {_fruitSprite}";
    }
}