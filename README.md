# AutoGet

A lightweight Unity utility that automatically resolves and injects `Component` references using attributes — no more boilerplate `GetComponent` calls in `Awake`.

## Overview

AutoGet lets you declare component dependencies directly on fields using attributes. References are resolved automatically at runtime (and in the editor), keeping your `Awake` methods clean and your intent explicit.

```csharp
public class PlayerController : AutoGetMono
{
    [AutoGet] Rigidbody rb;
    [AutoGetInChildren] Animator animator;
    [AutoGetInParent] AudioSource audioSource;
}
```

## Installation

Copy the following files into your Unity project:

| File | Description |
|---|---|
| `AutoGetAttribute.cs` | Attribute definitions |
| `AutoGetCache.cs` | Reflection cache for performance |
| `AutoGetInjector.cs` | Injection logic |
| `AutoGetMono.cs` | Base `MonoBehaviour` to inherit from |

## Usage

### 1. Inherit from `AutoGetMono`

```csharp
public class MyComponent : AutoGetMono
{
    // ...
}
```

### 2. Annotate your fields

Use one of three attributes on any `Component` field:

| Attribute | Equivalent to |
|---|---|
| `[AutoGet]` | `GetComponent<T>()` |
| `[AutoGetInChildren]` | `GetComponentInChildren<T>()` |
| `[AutoGetInParent]` | `GetComponentInParent<T>()` |

Fields can be `private`, `protected`, or `public`.

### 3. Optional references

By default, a warning is logged if a component cannot be found. Pass `optional: true` to suppress it:

```csharp
[AutoGet(optional: true)] Collider optionalCollider;
```

## How It Works

Injection runs in three lifecycle events:

- **`Awake`** — at runtime when the object is instantiated
- **`Reset`** — when the component is reset in the editor
- **`OnValidate`** — whenever values change in the Inspector (editor only)

Fields that are already assigned are skipped, so manually set references are never overwritten.

Reflection results are cached per type in `AutoGetCache`, so the cost of field discovery is only paid once per type regardless of how many instances exist.

## Overriding Awake

If you need your own `Awake`, call `base.Awake()` to ensure injection still runs:

```csharp
public class MyComponent : AutoGetMono
{
    [AutoGet] Rigidbody rb;

    protected override void Awake()
    {
        base.Awake(); // injection happens here
        rb.mass = 5f;
    }
}
```

## Limitations

- Only `Component` fields are supported — plain C# object types are not resolved.
- Each field resolves to a single component. Use `GetComponents` manually for arrays.
- Does not support injecting into nested base classes beyond what Unity's reflection flags cover (`Instance | Public | NonPublic`).
