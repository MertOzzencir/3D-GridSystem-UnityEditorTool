using System.Collections.Generic;
using UnityEngine;

public class DistanceComparer : IComparer<Collider>
{
    public Vector3 origin;
    public int Compare(Collider a, Collider b)
    {
        float dA = (a.transform.position - origin).sqrMagnitude;
        float dB = (b.transform.position - origin).sqrMagnitude;
        return dA.CompareTo(dB);
    }
}