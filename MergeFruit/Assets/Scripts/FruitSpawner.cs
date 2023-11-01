using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class FruitSpawner : ObjectPooling<Fruit>
{
    public static FruitSpawner Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    int NextIndex
    {
        get { return _nextIndex; }
        set
        {
            _nextIndex = value;

            if (UIController.Instance != null)
            {
                string name = FruitManager.Instance._fruitDatas.fruit[_nextIndex]._fruitName;
                if (FruitManager.Instance._fruitSprites.TryGetValue(name, out var s))
                    UIController.Instance.ChangeNextFruitImage(s);
            }
        }
    }
    int _nextIndex;

    private void Start()
    {
        _nextIndex = 0;
    }

    private void Update()
    {

    }

    public async void SpawnFruit()
    {
        var result = await base.SpawnObject();

        if (result == null)
            return;

        float xPos = UIController.Instance._arrow.position.x;

        result.transform.position = new Vector3(xPos, 3.25f, 0);
        result.FruitData = FruitManager.Instance._fruitDatas.fruit[_nextIndex];
        result._available = true;

        NextIndex = Random.Range(0, FruitManager.Instance._fruitDatas.fruit.Count / 2);
    }

    public void MergeFruit(Fruit a, Fruit b)
    {
        if (GameManager.Instance != null)
            GameManager.Instance.Score += a.FruitData._fruitScore;

        if (a.FruitData._fruitId < FruitManager.Instance._fruitDatas.fruit.Count - 1)
            a.FruitData = FruitManager.Instance._fruitDatas.fruit[a.FruitData._fruitId + 1];

        HideObject(a);
        HideObject(b);

        a.gameObject.SetActive(true);
        a.StartMergeAnim();

        GameManager.Instance.PlaySFX("Merge");

        EffectSpawner.Instance.MergeEffect(a.transform.position, a.FruitData._fruitSize);
    }
}
