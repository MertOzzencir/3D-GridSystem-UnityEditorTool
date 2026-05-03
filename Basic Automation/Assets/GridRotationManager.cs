using System;
using UnityEngine;

public class GridRotationManager
{
    public CarryableDirection direction;

    public void Initialize()
    {
        direction = CarryableDirection.Vector3Forward;
    }
    public void HandleRotate()
    {
        direction = (CarryableDirection)(((int)direction + 1) % Enum.GetValues(typeof(CarryableDirection)).Length);
    }
    public Vector3 GetRotation()
    {
        switch (direction)
        {
            case CarryableDirection.Vector3Forward:
                return new Vector3(0, 0, 0);
            case CarryableDirection.Vector3Right:
                return new Vector3(0, 90, 0);
            case CarryableDirection.Vector3Back:
                return new Vector3(0, 180, 0);
            case CarryableDirection.Vector3Left:
                return new Vector3(0, 270, 0);
        }
        return Vector3.zero;
    }
    public Vector3 GetDirection()
    {
        switch (direction)
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
