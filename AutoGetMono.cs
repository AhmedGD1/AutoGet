using UnityEngine;

public class AutoGetMono : MonoBehaviour
{
    protected virtual void Awake()
    {
        AutoGetInjector.Inject(this);
    }

#if UNITY_EDITOR
    void OnValidate()
    {
        AutoGetInjector.Inject(this);
    }
#endif

    protected virtual void Reset()
    {
        AutoGetInjector.Inject(this);
    }
}

