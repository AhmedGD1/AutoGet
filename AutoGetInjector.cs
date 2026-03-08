using UnityEngine;
using System.Reflection;

public static class AutoGetInjector
{
    public static void Inject(MonoBehaviour target)
    {
        var fields = AutoGetCache.GetFields(target.GetType());

        foreach (var data in fields)
        {
            if (data.fieldInfo.GetValue(target) != null)
                continue;
            
            Component component;

            component = data.type switch
            {
                AutoGetCache.AutoGetType.Children => target.GetComponentInChildren(data.fieldInfo.FieldType),
                AutoGetCache.AutoGetType.Parent   => target.GetComponentInParent(data.fieldInfo.FieldType),
                _                                 => target.GetComponent(data.fieldInfo.FieldType),
            };

            if (component != null)   data.fieldInfo.SetValue(target, component);
            else if (!data.optional) Debug.LogWarning($"AutoGet could not find {data.fieldInfo.FieldType.Name} on {target.name}");

        }
    }
}

