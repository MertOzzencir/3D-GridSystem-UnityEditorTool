using UnityEngine;

public class ToolAnimationManager : MonoBehaviour
{

    private MachineTools currentTool;
    private Animator anim;

    void Awake()
    {
        currentTool = GetComponent<MachineTools>();
        anim = GetComponentInChildren<Animator>();
    }

    public void PlayAnimation(string booleanName, bool state)
    {
        anim.SetBool(booleanName, state);
    }
}
