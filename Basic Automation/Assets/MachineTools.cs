using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class MachineTools : CarryablePlaceable
{
    [SerializeField] private float setTimer;
    [SerializeField] private string toolMainAnimName;
    private MachineBlueprintBase machineBase;
    private ToolAnimationManager animatorManager;
    public abstract void ToolLogic();
    protected override void Awake()
    {
        base.Awake();
        animatorManager = GetComponent<ToolAnimationManager>();
    }
    public void Setup(MachineBlueprintBase m)
    {
        machineBase = m;
    }
    public void CarryFromMachine()
    {
        c.enabled = false;
        CarryableEvents.Invoke(this);
        OnCarryDrop();
        Visual.localPosition = visualSavedPosition;
    }
    public void UseTool()
    {
        StartCoroutine(ToolLogicExectuer());
    }
    public override void Drop(out GridDictData sc)
    {
        sc = null;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Grid")))
        {
            MachineBlueprintBase goalMachine = GridManager.Instance.GetOneMachineInGrid(hit.point);
            if (goalMachine != null)
            {
                GridPosition = goalMachine.GridPosition;
                sc = GridManager.Instance.GetGridData(goalMachine.GridPosition);
                ResetRotation();
                goalMachine.AddToolOnSlot(this, out bool s);
                if (s)
                    return;
            }
        }
        base.Drop(out sc);

    }
    public MachineBlueprintBase GetBase()
    {
        return machineBase;
    }
    public float CooldownTimer()
    {
        return setTimer;
    }
    public IEnumerator ToolLogicExectuer()
    {
        float timer = 0;
        animatorManager.PlayAnimation(toolMainAnimName, true);
        while (timer < setTimer)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        ToolLogic();
        animatorManager.PlayAnimation(toolMainAnimName, false);
    }
}
