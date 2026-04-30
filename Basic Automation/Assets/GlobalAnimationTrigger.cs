using System;
using UnityEngine;

public class GlobalAnimationTrigger : MonoBehaviour
{

    public event Action OnAnimationTrigger;
    public void TriggerAnimation()
    {
        OnAnimationTrigger?.Invoke();
    }

}
