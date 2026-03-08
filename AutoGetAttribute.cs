using System;

[AttributeUsage(AttributeTargets.Field)]
public class AutoGetAttribute : Attribute
{
    public bool optional;

    public AutoGetAttribute(bool optional = false)
    {
        this.optional = optional;
    }
}

[AttributeUsage(AttributeTargets.Field)]
public class AutoGetInChildrenAttribute : Attribute
{
    public bool optional;

    public AutoGetInChildrenAttribute(bool optional = false)
    {
        this.optional = optional;
    }
}

[AttributeUsage(AttributeTargets.Field)]
public class AutoGetInParentAttribute : Attribute
{
    public bool optional;

    public AutoGetInParentAttribute(bool optional = false)
    {
        this.optional = optional;
    }
}