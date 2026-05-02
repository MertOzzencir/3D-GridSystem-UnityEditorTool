using System;
using System.Collections.Generic;
using UnityEngine;

public class SourceLocalManager : Placeable, ITriggerInput
{
    public event Action<Animator, string> OnChildAnimation;
    List<Destructable> childDestructables;
    private void Awake()
    {
        childDestructables = new List<Destructable>(GetComponentsInChildren<Destructable>());

    }

    public void GetSignal()
    {
        if (childDestructables.Count > 0)
        {
            OnChildHit(childDestructables[0]);
        }
    }
    private void OnChildHit(Destructable child)
    {
        //Debug.Log("Destructable Manager handles child");
        int _childHealth = child.GetHealth();
        _childHealth -= 1;
        child.SetHealth(_childHealth);
        OnChildAnimation?.Invoke(child.GetAnimator(), child.CurrentAnimationName(0));
        if (_childHealth <= 0)
        {
            childDestructables.Remove(child);
            SpawnDropFromChild(child.GetData().DropGameObject);
            Destroy(child.gameObject);
            DestroySelfCheck();
        }
    }

    private void SpawnDropFromChild(SourceBase child)
    {
        GridManager m = GridManager.Instance;
        GameObject gridRef = m.GetGridGameObject(m.GetOneFreeGrid(GridPosition, Size));
        if (gridRef != null)
        {
            Vector3Int freeSpawnPosition = new Vector3Int(Mathf.FloorToInt(gridRef.transform.position.x),
                                                          Mathf.FloorToInt(gridRef.transform.localScale.y),
                                                         Mathf.FloorToInt(gridRef.transform.position.z)
            );
            Placeable spawnedDrop = Instantiate(child, freeSpawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.Log("There is no place to spawn drop object");
        }
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
