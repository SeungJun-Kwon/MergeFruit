using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSpawner : ObjectPooling<Effect>
{
    public static EffectSpawner Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    public async void MergeEffect(Vector3 pos, float scale)
    {
        var result = await base.SpawnObject();

        if (result == null)
            return;

        result.transform.position = pos;
        result.transform.localScale = Vector3.one * scale * 0.1f;
        result._particle.Play();
    }
}
