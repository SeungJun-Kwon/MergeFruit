using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class FruitSpawner : MonoBehaviour
{
    public static FruitSpawner Instance;

    public AssetReference _spawnObject;
    public GameObject _spawnGO;

    List<Fruit> _fruits = new();

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

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    private void Start()
    {
        _nextIndex = 0;
    }

    private void Update()
    {

    }

    public async void SpawnFruit(float xPos)
    {
        int index = FindDisabledObject();
        if (index >= 0)
        {
            _fruits[index].gameObject.SetActive(true);
            _fruits[index].FruitData = FruitManager.Instance._fruitDatas.fruit[_nextIndex];
            _fruits[index].transform.position = new Vector3(xPos, 3.25f, 0);
        }
        else
        {
            var operation = _spawnObject.InstantiateAsync(new Vector3(xPos, 3.25f, 0), Quaternion.identity);
            await operation.Task; // 비동기 작업이 완료될 때까지 대기하기 위해 .Task를 사용

            if (operation.Result.gameObject.TryGetComponent(out Fruit fruit))
            {
                fruit.FruitData = FruitManager.Instance._fruitDatas.fruit[_nextIndex];
                fruit.transform.parent = transform;
                _fruits.Add(fruit);
            }
        }

        NextIndex = Random.Range(0, FruitManager.Instance._fruitDatas.fruit.Length / 2);
    }

    public void HideFruit(Fruit fruit)
    {
        foreach(var f in _fruits)
        {
            if (f == fruit)
                f.gameObject.SetActive(false);
        }
    }

    public void HideAllFruit()
    {
        foreach (var f in _fruits)
            f.gameObject.SetActive(false);
    }

    private int FindDisabledObject()
    {
        int result = -1;

        if (_fruits.Count < 1)
            return result;

        for(int i = 0; i < _fruits.Count; i++)
        {
            if(!_fruits[i].gameObject.activeSelf)
            {
                result = i;
                break;
            }
        }

        return result;
    }
}
