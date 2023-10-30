using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FruitManager : MonoBehaviour
{
    public static FruitManager Instance;

    public FruitDatas _fruitDatas;

    public Dictionary<string, Sprite> _fruitSprites = new();

    [SerializeField] TextAsset _fruitJson;

    const string _fruitURL = "https://docs.google.com/spreadsheets/d/1bWqXHogVMaGZO2mmRIAXUMd3ihiSycEadH4lG1m89yw/export?format=tsv&range=A2:E";

    private IEnumerator Start()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else if (Instance != this)
            Destroy(this);

        UnityWebRequest www = UnityWebRequest.Get(_fruitURL);
        yield return www.SendWebRequest();

        string data = www.downloadHandler.text;

        string[] row = data.Split('\n');
        int rowSize = row.Length;

        for (int i = 0; i < rowSize; i++)
        {
            string[] column = row[i].Split('\t');

            FruitData fruitData = new();
            fruitData._fruitId = int.Parse(column[0]);
            fruitData._fruitName = column[1];
            fruitData._fruitSize = float.Parse(column[2]);
            fruitData._fruitScore = int.Parse(column[3]);
            fruitData._fruitSprite = column[4].Trim();

            _fruitDatas.fruit.Add(fruitData);
        }

        foreach (var f in _fruitDatas.fruit)
        {
            UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<Sprite>($"Assets/Sprites/Fruits/{f._fruitSprite}.png").Completed += result =>
            {
                _fruitSprites.Add(f._fruitName, result.Result);
            };
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