using System.Collections.Generic;
using UnityEngine;

public class DestructableLocalManager : Placeable
{
    List<Destructable> childDestructables;
    private void Awake()
    {
        childDestructables = new List<Destructable>(GetComponentsInChildren<Destructable>());
        foreach (var a in childDestructables)
        {
            a.OnHit += OnChildHit;
        }
    }

    private void OnChildHit(Destructable child)
    {
        //Debug.Log("Destructable Manager handles child");
        int _childHealth = child.GetHealth();
        _childHealth -= 1;
        child.SetHealth(_childHealth);
        if (_childHealth <= 0)
        {

            if (IsChildWeakPoint(child)) return;

            child.OnHit -= OnChildHit;
            childDestructables.Remove(child);
            SpawnDropFromChild(child.GetData().DropGameObject);
            Destroy(child.gameObject);
            DestroySelfCheck();
        }
    }

    private bool IsChildWeakPoint(Destructable child)
    {
        if (child.GetData().IsWeakPoint)
        {
            for (int i = 0; i < childDestructables.Count; i++)
            {
                Destructable currentChild = childDestructables[i];
                currentChild.OnHit -= OnChildHit;
                SpawnDropFromChild(child.GetData().DropGameObject);
                Destroy(currentChild.gameObject);
            }
            childDestructables.Clear();
            DestroySelfCheck();
            return true;
        }

        return false;
    }
    private void SpawnDropFromChild(DropBase child)
    {
        GridManager m = GridManager.Instance;
        GameObject gridRef = m.GetGridGameObject(m.GetFreeGrid(transform.position, Size));
        Vector3Int freeSpawnPosition = new Vector3Int(Mathf.FloorToInt(gridRef.transform.position.x),
                                                      Mathf.FloorToInt(gridRef.transform.localScale.y),
                                                     Mathf.FloorToInt(gridRef.transform.position.z)
        );
        Placeable spawnedDrop = Instantiate(child, freeSpawnPosition, Quaternion.identity);
    }

    private void DestroySelfCheck()
    {
        if (childDestructables.Count <= 0)
        {
            DeleteOnGrid();
            Destroy(gameObject);
        }
    }

}
