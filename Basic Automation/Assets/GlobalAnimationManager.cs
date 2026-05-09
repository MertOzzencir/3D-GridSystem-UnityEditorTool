using UnityEngine;

public class GlobalAnimationManager : MonoBehaviour
{
    [SerializeField] private Animator anim;



    public void PlayTrigger(string name)
    {
        anim.SetTrigger(name);
    }
    public void PlayBool(string name, bool state)
    {
        anim.SetBool(name, state);
    }

    public void SeperateAnimatorTriggerPlay(Animator anim, string name)
    {
        anim.SetTrigger(name);
    }
}
