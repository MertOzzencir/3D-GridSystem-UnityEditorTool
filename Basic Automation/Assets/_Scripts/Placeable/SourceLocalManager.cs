using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SourceLocalManager : Placeable, ITriggerOutput
{
    [SerializeField] private ParticleSystem leafParticle;
    List<Destructable> childDestructables;
    private GlobalAnimationManager animManager;
    public override void Start()
    {
        base.Start();
        childDestructables = new List<Destructable>(GetComponentsInChildren<Destructable>());
        animManager = GetComponent<GlobalAnimationManager>();

    }

    public void ProcessSignal()
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
        animManager.SeperateAnimatorTriggerPlay(child.GetAnimator(), child.CurrentAnimationName(0));
        leafParticle.gameObject.SetActive(true);
        leafParticle.Play();
        if (_childHealth <= 0)
        {
            childDestructables.Remove(child);
            SpawnDropFromChild(child.GetData().Prefab.GetComponent<SourceBase>());
            child.OnDeath();
            DestroySelfCheck();
        }
    }

    private void SpawnDropFromChild(SourceBase child)
    {
        GridManager m = GridManager.Instance;
        GameObject gridRef = m.GetGridGameObject(m.GetOneGridInRange(GridPosition, Size));
        if (gridRef != null)
        {
            Vector3Int freeSpawnPosition = new Vector3Int(Mathf.FloorToInt(gridRef.transform.position.x),
                                                          Mathf.FloorToInt(gridRef.transform.localScale.y),
                                                         Mathf.FloorToInt(gridRef.transform.position.z)
            );
            SourceBase spawnedDrop = Instantiate(child, freeSpawnPosition, Quaternion.identity);
            Transform dropVisual = spawnedDrop.ReturnVisual();
            dropVisual.position = transform.position + new Vector3(0.5f, 0.0f, 0.5f);
            spawnedDrop.SpawnAnimation();
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
            leafParticle.transform.parent = null;
            var main = leafParticle.main;
            main.stopAction = ParticleSystemStopAction.Destroy;
            Destroy(gameObject);
        }
    }


}
