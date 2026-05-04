using UnityEngine;

public class SourceAnimationManager : MonoBehaviour
{
    private SourceLocalManager baseManager;
    void Awake()
    {
        baseManager = GetComponent<SourceLocalManager>();
        baseManager.OnChildAnimation += PlayTriggerAnimation;
    }

    private void PlayTriggerAnimation(Animator destructable, string animationname)
    {
        destructable.SetTrigger(animationname);
    }
}
