using System;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GridRotationManager
{
    public CarryableDirection Direction;
    public Vector3 defaultRotation;
    public void Initialize(Transform defaultRot)
    {
        Direction = CarryableDirection.Vector3Forward;
        defaultRotation = defaultRot.eulerAngles;
    }
    public void HandleRotate()
    {
        Direction = (CarryableDirection)(((int)Direction + 1) % Enum.GetValues(typeof(CarryableDirection)).Length);
    }
    public Vector3 GetRotation(Transform t)
    {
        switch (Direction)
        {
            case CarryableDirection.Vector3Forward:
                return new Vector3(0, 0, 0) + defaultRotation;
            case CarryableDirection.Vector3Right:
                return new Vector3(0, 90, 0) + defaultRotation;
            case CarryableDirection.Vector3Back:
                return new Vector3(0, 180, 0) + defaultRotation;
            case CarryableDirection.Vector3Left:
                return new Vector3(0, 270, 0) + defaultRotation;
        }
        return Vector3.zero;
    }
    public Vector3 GetDirection()
    {
        switch (Direction)
        {
            case CarryableDirection.Vector3Forward:
                return Vector3.forward;
            case CarryableDirection.Vector3Right:
                return Vector3.right;
            case CarryableDirection.Vector3Back:
                return Vector3.back;
            case CarryableDirection.Vector3Left:
                return Vector3.left;
        }
        return Vector3.zero;
    }
}
