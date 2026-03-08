using System;
using System.Collections.Generic;
using System.Reflection;

public static class AutoGetCache
{
    public enum AutoGetType
    {
        Self, Parent, Children
    }

    private static readonly Dictionary<Type, FieldData[]> cache = new();

    public static FieldData[] GetFields(Type type)
    {
        if (cache.TryGetValue(type, out var fields))
            return fields;

        var list  = new List<FieldData>();
        var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        foreach (var field in type.GetFields(flags))
        {
            if (field.GetCustomAttribute<AutoGetAttribute>() is AutoGetAttribute attribute)
            {
                FieldData data = new FieldData { fieldInfo = field, type = AutoGetType.Self, optional = attribute.optional };
                list.Add(data);
            }

            else if (field.GetCustomAttribute<AutoGetInChildrenAttribute>() is AutoGetInChildrenAttribute childrenAttribute)
            {
                FieldData data = new FieldData { fieldInfo = field, type = AutoGetType.Children, optional = childrenAttribute.optional };
                list.Add(data);
            }

            else if (field.GetCustomAttribute<AutoGetInParentAttribute>() is AutoGetInParentAttribute parentAttribute)
            {
                FieldData data = new FieldData { fieldInfo = field, type = AutoGetType.Parent, optional = parentAttribute.optional };
                list.Add(data);
            }
        }

        fields      = list.ToArray();
        cache[type] = fields;

        return fields;
    }

    public struct FieldData
    {
        public FieldInfo fieldInfo;
        public AutoGetType type;
        public bool optional;
    }
}

