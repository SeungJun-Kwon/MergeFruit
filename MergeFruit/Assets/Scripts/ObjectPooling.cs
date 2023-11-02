using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class ObjectPooling<T> : MonoBehaviour where T : Component
{
    [SerializeField] string _assetAddress;

    protected List<T> _objects = new();

    private void Update()
    {

    }

    public virtual Task<T> SpawnObject()
    {
        int index = FindDisabledObject();

        if (index >= 0)
        {
            _objects[index].gameObject.SetActive(true);
            return Task.FromResult(_objects[index]);
        }
        else
        {
            // WaitForCompletion : ���� �߰��� ��巹���� API
            // ���� �񵿱������� ����Ǵ� ��巹������ ���������� �� �� �ְ� ���ִ� �޼ҵ�
            // �ε��� �Ϸ�� ������ �ش� �ڵ带 ����
            var result = Addressables.InstantiateAsync(_assetAddress, transform).WaitForCompletion();

            if (result == null || !result.TryGetComponent(out T value))
                return null;

            _objects.Add(value);

            return Task.FromResult(value);
        }
    }

    public void HideObject(T target)
    {
        foreach (var obj in _objects)
        {
            if (obj == target)
                obj.gameObject.SetActive(false);
        }
    }

    public void HideAllObjects()
    {
        foreach (var obj in _objects)
            obj.gameObject.SetActive(false);
    }

    protected int FindDisabledObject()
    {
        int result = -1;

        if (_objects.Count < 1)
            return result;

        for (int i = 0; i < _objects.Count; i++)
        {
            if (!_objects[i].gameObject.activeSelf)
            {
                result = i;
                break;
            }
        }

        return result;
    }
}
