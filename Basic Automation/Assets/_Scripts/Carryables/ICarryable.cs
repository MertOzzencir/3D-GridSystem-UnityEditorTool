
using System;
using UnityEngine;

public interface ICarryable
{
    public void Carry();
    public void Drop(out bool sc);
    public Transform GetTransform();
}
