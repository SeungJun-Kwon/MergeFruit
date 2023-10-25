using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitManager : MonoBehaviour
{
    public static FruitManager Instance;

    public FruitDatas _fruitDatas;

    public Dictionary<string, Sprite> _fruitSprites = new();

    [SerializeField] TextAsset _fruitJson;

    private async void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (Instance != this)
            Destroy(this);

        var operation = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<TextAsset>("Assets/Jsons/fruit.json");
        await operation.Task;

        operation.Completed += result =>
        {
            _fruitJson = result.Result;
            _fruitDatas = JsonUtility.FromJson<FruitDatas>(_fruitJson.text);

            foreach (var f in _fruitDatas.fruit)
            {
                string path = $"Assets/Sprites/Fruits/{f._fruitSprite}.png";
                UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<Sprite>(path).Completed += result =>
                {
                    _fruitSprites.Add(f._fruitName, result.Result);
                };
            }
        };
    }
}

[System.Serializable]
public class FruitDatas
{
    public FruitData[] fruit;
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