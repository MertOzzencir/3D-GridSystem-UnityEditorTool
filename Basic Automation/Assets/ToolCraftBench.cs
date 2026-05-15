using UnityEngine;

public class ToolCraftBench : CraftBenchBase<SourcesSO, MachineToolSO>
{
    protected override Sprite GetInputIcon(SourcesSO i) => i.Icon;
    protected override Sprite GetOutputIcon(MachineToolSO o) => o.Icon;

    protected override bool CheckInventory(SourcesSO input)
    {
        var inv = InventoryManager.Instance;
        return inv.CheckSource(input) && inv.SourceAmount(input) > 0;
    }
    protected override void ConsumeFromInventory(SourcesSO input)
        => InventoryManager.Instance.ReduceSource(input);
    protected override void RefundToInventory(SourcesSO input)
        => InventoryManager.Instance.AddSource(input);

    protected override void SpawnOutput(Vector3Int spawnPos)
    {
        var tool = Instantiate(currentRecipt.OutputRecipt.Prefab, spawnPos, Quaternion.identity);
    }
}