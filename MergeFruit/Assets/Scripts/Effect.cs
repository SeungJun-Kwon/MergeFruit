using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public ParticleSystem _particle;

    private void Awake()
    {
        TryGetComponent(out _particle);
    }

    private void OnParticleSystemStopped()
    {
        EffectSpawner.Instance.HideObject(this);
    }
}