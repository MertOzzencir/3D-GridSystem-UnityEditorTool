
using System;
using UnityEngine;

public interface ICarryable
{
    public void Carry();
    public void Drop(out GridDictData sc);
    public Transform Visual { get; }
    public void Rotate();
    public Transform GetTransform();
    public GridRotationManager RotationManager { get; set; }
    public void UpdateMain();
}

public enum CarryableDirection
{
    Vector3Forward,
    Vector3Right,
    Vector3Back,
    Vector3Left
}
