using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Fruit : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    Rigidbody2D _rigidBody;

    public FruitData FruitData
    {
        get { return _fruitData; }
        set
        {
            if(value == null)
            {
                Addressables.Release(this);
                return;
            }

            _fruitData = value;

            transform.localScale = Vector3.one * _fruitData._fruitSize;

            _spriteRenderer.sprite = FruitManager.Instance._fruitSprites[_fruitData._fruitName];
            _spriteRenderer.sortingOrder = FruitManager.Instance._fruitDatas.fruit.Length - _fruitData._fruitId;
        }
    }
    FruitData _fruitData;

    private void Awake()
    {
        TryGetComponent(out _spriteRenderer);
        TryGetComponent(out _rigidBody);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Fruit colFruit))
        {
            MergeFruit(colFruit);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out Fruit colFruit))
        {
            MergeFruit(colFruit);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_rigidBody.velocity.y < 0.0001f && _rigidBody.velocity.y > -0.0001f)
        {
            if (collision.name == "LimitLine")
                GameManager.Instance.GameOver();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_rigidBody.velocity.y < 0.0001f && _rigidBody.velocity.y > -0.0001f)
        {
            if (collision.name == "LimitLine")
                GameManager.Instance.GameOver();
        }
    }

    void MergeFruit(Fruit fruit)
    {
        if (FruitData._fruitId < FruitManager.Instance._fruitDatas.fruit.Length - 1 && (FruitData._fruitId == fruit.FruitData._fruitId))
        {
            Vector3 myPos = transform.position;
            Vector3 otherPos = fruit.transform.position;

            // 아래에 있는 오브젝트를 기준으로 합친다
            if ((myPos.y < otherPos.y) || ((myPos.y == otherPos.y) && (myPos.x < otherPos.x)))
            {
                FruitSpawner.Instance.HideFruit(fruit);

                if (GameManager.Instance != null)
                    GameManager.Instance.Score += FruitData._fruitScore;

                if (FruitData._fruitId < FruitManager.Instance._fruitDatas.fruit.Length - 1)
                    FruitData = FruitManager.Instance._fruitDatas.fruit[FruitData._fruitId + 1];

                GameManager.Instance.PlaySFX("Merge");
            }
        }
    }
}