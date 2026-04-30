using UnityEngine;

public class DestructableAnimationManager : MonoBehaviour
{
    private DestructableLocalManager baseManager;
    void Awake()
    {
        baseManager = GetComponent<DestructableLocalManager>();
        baseManager.OnChildAnimation += PlayAnimation;
    }

    private void PlayAnimation(Animator destructable, string animationname)
    {
        destructable.SetTrigger(animationname);
    }
}
