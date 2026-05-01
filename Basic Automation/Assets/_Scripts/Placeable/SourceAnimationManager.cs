using UnityEngine;

public class SourceAnimationManager : MonoBehaviour
{
    private SourceLocalManager baseManager;
    void Awake()
    {
        baseManager = GetComponent<SourceLocalManager>();
        baseManager.OnChildAnimation += PlayAnimation;
    }

    private void PlayAnimation(Animator destructable, string animationname)
    {
        destructable.SetTrigger(animationname);
    }
}
