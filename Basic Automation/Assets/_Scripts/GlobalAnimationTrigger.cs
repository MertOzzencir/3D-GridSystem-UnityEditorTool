using System;
using UnityEngine;

public class GlobalAnimationTrigger : MonoBehaviour
{

    public event Action<string> OnAnimationTrigger;
    public void TriggerAnimation(string animname)
    {
        OnAnimationTrigger?.Invoke(animname);
    }

}
