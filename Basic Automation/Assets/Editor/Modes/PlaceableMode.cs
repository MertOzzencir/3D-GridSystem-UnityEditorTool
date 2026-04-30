using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PlaceableMode : IPlacerMode
{
    public string ModeName => "Placeable Mode";
    public Color ModeColor => Color.cyan;

    private Placeable[] placeables;
    private GridDataSO gridSOData;
    private bool isDeleteMode;
    private ReorderableList placeableList;

    public PlaceableMode(GridDataSO gridSOData, ref Placeable[] placeables)
    {
        this.gridSOData = gridSOData;
        this.placeables = placeables;
        BuildList(ref placeables);
    }

    private void BuildList(ref Placeable[] placeables)
    {
        placeableList = new ReorderableList(placeables, typeof(Placeable), true, true, true, true);

        placeableList.drawHeaderCallback = rect =>
            EditorGUI.LabelField(rect, "Placeables");

        placeableList.drawElementCallback = (rect, index, isActive, isFocused) =>
        {
            this.placeables[index] = (Placeable)EditorGUI.ObjectField(
                new Rect(rect.x, rect.y + 2, rect.width, EditorGUIUtility.singleLineHeight),
                this.placeables[index], typeof(Placeable), false
            );
        };

        placeableList.onAddCallback = list =>
        {
            ArrayUtility.Add(ref this.placeables, null);
            placeableList.list = this.placeables;
        };

        placeableList.onRemoveCallback = list =>
        {
            ArrayUtility.RemoveAt(ref this.placeables, list.index);
            placeableList.list = this.placeables;
        };
    }

    public void DrawUI()
    {
        placeableList.DoLayoutList();

        GUILayout.Space(5);

        GUI.color = isDeleteMode ? Color.red : Color.white;
        if (GUILayout.Button(isDeleteMode ? "Delete Mode — Kapat(Tab)" : "Delete Mode Aç(Tab)"))
            isDeleteMode = !isDeleteMode;
        GUI.color = Color.white;

        if (isDeleteMode)
            EditorGUILayout.HelpBox("X ile placeable sil", MessageType.Info);
        else
            EditorGUILayout.HelpBox("Sol tık veya 1-9 ile placeable yerleştir", MessageType.Info);
    }

    public void OnSceneGUI(Event e)
    {
        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Tab)
        {
            isDeleteMode = !isDeleteMode;
            e.Use();
        }

        if (isDeleteMode)
        {
            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.X)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Placeable")))
                {
                    Placeable placeable = hit.transform.GetComponent<Placeable>();

                    for (int z = 0; z < placeable.Size.y; z++)
                    {
                        for (int x = 0; x < placeable.Size.x; x++)
                        {
                            Vector3Int gridPos = new Vector3Int(
Mathf.FloorToInt(placeable.transform.position.x + x),
0,
Mathf.FloorToInt(placeable.transform.position.z + z)
);
                            GridRawData entry = gridSOData.entries.Find(e => e.position == gridPos);
                            if (entry != null)
                            {
                                entry.IsFull = false;
                                entry.OccupiedBy = "";
                            }

                        }
                    }
                    Object.DestroyImmediate(hit.transform.gameObject);
                    e.Use();
                }
            }
            return;
        }

        if (e.type == EventType.KeyDown)
        {
            for (int i = 0; i < placeables.Length; i++)
            {
                if (e.keyCode == KeyCode.Alpha1 + i)
                {
                    RayCastToWorld(e, placeables[i]);
                    e.Use();
                    return;
                }
            }
        }

        if (e.type == EventType.MouseDown && e.button == 0)
            RayCastToWorld(e);
    }

    private void RayCastToWorld(Event e, Placeable placeable = null)
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Grid")))
        {
            Vector3Int hitPos = new Vector3Int(
                Mathf.FloorToInt(hit.transform.position.x),
                Mathf.FloorToInt(hit.transform.localScale.y),
                Mathf.FloorToInt(hit.transform.position.z)
            );
            Vector3Int gridPos = new Vector3Int(
                        Mathf.FloorToInt(hit.transform.position.x),
                        0,
                        Mathf.FloorToInt(hit.transform.position.z)
                    );
            GridRawData entry = gridSOData.entries.Find(e => e.position == gridPos);
            if (placeable == null)
                OpenPlacementMenu(hitPos, hit.transform, entry);
            else
                PlacePlaceable(placeable, hitPos, hit.transform, entry);

            e.Use();
        }
    }

    private void OpenPlacementMenu(Vector3Int pos, Transform parent, GridRawData grid)
    {
        GenericMenu menu = new GenericMenu();
        for (int i = 0; i < placeables.Length; i++)
        {
            if (placeables[i] != null)
            {
                Placeable captured = placeables[i];
                menu.AddItem(new GUIContent(captured.name), false, () => PlacePlaceable(captured, pos, parent, grid));
            }
            else
            {
                menu.AddDisabledItem(new GUIContent($"Placeable {i + 1} atanmamış"));
            }
        }
        menu.ShowAsContext();
    }

    private void PlacePlaceable(Placeable placeable, Vector3Int pos, Transform parent, GridRawData spawnedGrid)
    {
        if (!spawnedGrid.IsFull)
        {
            bool isAvaliable = true;
            List<GridRawData> checkedGrids = new List<GridRawData>();
            for (int z = 0; z < placeable.Size.y; z++)
            {
                for (int x = 0; x < placeable.Size.x; x++)
                {
                    Vector3Int hitPos = new Vector3Int(
               Mathf.FloorToInt(spawnedGrid.position.x + x),
               0,
               Mathf.FloorToInt(spawnedGrid.position.z + z)
           );
                    GridRawData entry = gridSOData.entries.Find(e => e.position == hitPos);
                    if (entry != null)
                    {
                        checkedGrids.Add(entry);
                        if (entry.IsFull)
                            isAvaliable = false;
                    }
                    else
                        isAvaliable = false;
                }
            }
            if (!isAvaliable) return;




            Placeable instance = (Placeable)PrefabUtility.InstantiatePrefab(placeable);
            instance.transform.position = new Vector3(pos.x, pos.y, pos.z);
            instance.transform.SetParent(parent);
            instance.name = $"Placeable_{spawnedGrid.position.x}_{spawnedGrid.position.z}";
            foreach (var a in checkedGrids)
            {
                a.IsFull = true;
                a.OccupiedBy = instance.name;
            }

            checkedGrids.Clear();

            spawnedGrid.IsFull = true;
            Undo.RegisterCreatedObjectUndo(instance, "Place Placeable");
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
    }
}