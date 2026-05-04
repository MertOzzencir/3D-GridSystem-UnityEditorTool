using UnityEngine;

public class SpawnerAnimationManager : MonoBehaviour
{
    [SerializeField] private Animator anim;

    public void HitAnimation()
    {
        anim.SetTrigger("hit");
    }
}
