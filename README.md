# EZTween - Lightweight Tweening Library for Unity  

**EZTween** is a minimalistic yet powerful tweening library designed for Unity, focusing on smooth animations through interpolation (`Lerp`) operations. It provides a fluent API for creating and managing animation sequences with various easing curves.

## Key Features  

- **Simple API**: Chain tweens with a fluent interface.  
- **Multiple Easing Curves**: Quad, Quint, Back, Bounce, and more.  
- **Sequence Control**: Queue tweens, delays, and callbacks.  
- **Looping**: Easily create looping animations.  
- **Lightweight**: Object pooling for efficient memory usage.  

## Installation  

1. **Via Git URL**:  
   Add this to your Unity project's `Packages/manifest.json`:  
```json
"com.zeni.eztween": "https://github.com/zenikode/EZTween.git?path=Assets",
```

2. **Manual Import**:  
   Clone the repository and place the `EZTween` folder in your `Assets` directory.

## Usage  

### Basic Tween  

```csharp
EZ.Spawn()
    .Tween(1f, data => 
    {
        transform.position = Vector3.Lerp(startPos, endPos, data.QuadOut);
    });
```

### Chaining Tweens  

```csharp
EZ.Spawn()
    .Tween(0.5f, data => transform.localScale = Vector3.one * data.QuintIn)
    .Delay(0.2f)
    .Tween(0.5f, data => transform.localScale = Vector3.one * (1 - data.Bounce));
```

### Looping  

```csharp
EZ.Spawn()
    .Loop()
    .Tween(1f, data => transform.Rotate(Vector3.up, 360 * data.Linear));
```

### Custom Easing  

```csharp
EZ.Spawn()
    .Tween(2f, data => 
    {
        float alpha = data.BackInOut;
        spriteRenderer.color = Color.Lerp(Color.red, Color.blue, alpha);
    });
```

### Waiting for Conditions  

```csharp
EZ.Spawn()
    .Tween(1f, data => MoveEnemy(data.QuadOut))
    .Wait(() => playerIsInRange)
    .Tween(0.5f, data => Attack(data.QuintIn));
```

## Available Easing Functions  

| Ease Type       | In       | Out      | InOut     | OutIn     |
|-----------------|----------|----------|-----------|-----------|
| **Linear**      | `Linear` | -        | -         | -         |
| **Quad**        | `QuadIn` | `QuadOut`| `QuadInOut`| `QuadOutIn`|
| **Quint**       | `QuintIn`| `QuintOut`| `QuintInOut`| `QuintOutIn`|
| **Back**        | `BackIn` | `BackOut`| `BackInOut`| `BackOutIn`|
| **Bounce**      | `Bounce` | -        | -         | -         |

## Advanced Usage  

### Object Pooling  
EZTween uses pooling for `EZStep` objects to minimize garbage collection.  

### Manual Control  
```csharp
var queue = EZ.Spawn();
queue.Tween(1f, data => { /* ... */ });

// Force-complete all tweens  
queue.Forward();

// Clear all tweens  
queue.Clear();
```

## Performance Tips  

- Reuse `EZQueue` instances where possible.  
- Avoid frequent allocations inside tween callbacks.  
- Use `Delay()` instead of empty tweens for better readability.  

## License  
MIT License. Free to use in personal and commercial projects.  

---  

**Happy Tweening!** 🚀
