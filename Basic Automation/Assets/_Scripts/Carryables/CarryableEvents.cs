using System;
using UnityEngine;

public static class CarryableEvents
{
    public static event Action<ICarryable> OnCarryable;
    public static void Invoke(ICarryable t)
    {
        Debug.Log("event from " + t.GetTransform().name);
        OnCarryable?.Invoke(t);
    }
}
