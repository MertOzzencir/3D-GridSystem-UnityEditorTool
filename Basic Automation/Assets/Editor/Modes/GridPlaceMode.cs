// Editor/Modes/GridPlaceMode.cs
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using UnityEditorInternal;

public class GridPlaceMode : IPlacerMode
{
    public string ModeName => "Grid Place Mode";
    public Color ModeColor => Color.green;

    private GameObject[] prefabs;
    private GridDataSO gridSOData;
    private bool isDeleteMode;
    private ReorderableList prefabList;

    public GridPlaceMode(GridDataSO gridSOData, ref GameObject[] prefabs)
    {
        this.gridSOData = gridSOData;
        this.prefabs = prefabs;
        BuildList(ref prefabs);
    }

    private void BuildList(ref GameObject[] prefabs)
    {
        prefabList = new ReorderableList(prefabs, typeof(GameObject), true, true, true, true);

        prefabList.drawHeaderCallback = rect =>
            EditorGUI.LabelField(rect, "Grid Prefabları");

        prefabList.drawElementCallback = (rect, index, isActive, isFocused) =>
    {
        this.prefabs[index] = (GameObject)EditorGUI.ObjectField(
            new Rect(rect.x, rect.y + 2, rect.width, EditorGUIUtility.singleLineHeight),
            this.prefabs[index], typeof(GameObject), false
        );
    };

        prefabList.onAddCallback = list =>
     {
         ArrayUtility.Add(ref this.prefabs, null);
         prefabList.list = this.prefabs;
     };


        prefabList.onRemoveCallback = list =>
    {
        ArrayUtility.RemoveAt(ref this.prefabs, list.index);
        prefabList.list = this.prefabs;
    };
    }

    public void DrawUI()
    {
        prefabList.DoLayoutList();

        GUILayout.Space(5);

        GUI.color = isDeleteMode ? Color.red : Color.white;
        if (GUILayout.Button(isDeleteMode ? "Delete Mode — Kapat(Tab)" : "Delete Mode Aç(Tab)"))
            isDeleteMode = !isDeleteMode;
        GUI.color = Color.white;

        if (isDeleteMode)
            EditorGUILayout.HelpBox("X ile grid sil", MessageType.Info);
        else
            EditorGUILayout.HelpBox("Sol tık veya 1-9 ile grid yerleştir", MessageType.Info);

        GUILayout.Space(5);

        if (GUILayout.Button("Clear Data"))
        {
            gridSOData.entries.Clear();
            EditorUtility.SetDirty(gridSOData);
        }
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
                if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Grid")))
                {
                    Vector3Int hitPos = new Vector3Int(
                        Mathf.FloorToInt(hit.transform.position.x),
                        0,
                        Mathf.FloorToInt(hit.transform.position.z)
                    );
                    gridSOData.entries.RemoveAll(entry => entry.position == hitPos);
                    EditorUtility.SetDirty(gridSOData);
                    Object.DestroyImmediate(hit.transform.gameObject);
                    e.Use();
                }
            }
            return;
        }

        if (e.type == EventType.KeyDown)
        {
            for (int i = 0; i < prefabs.Length; i++)
            {
                if (e.keyCode == KeyCode.Alpha1 + i)
                {
                    RayCastToWorld(e, prefabs[i]);
                    e.Use();
                    return;
                }
            }
        }

        if (e.type == EventType.MouseDown && e.button == 0)
            RayCastToWorld(e);
    }

    private void RayCastToWorld(Event e, GameObject prefab = null)
    {
        Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
        Plane ground = new Plane(Vector3.up, Vector3.zero);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, LayerMask.GetMask("Grid")))
            return;
        if (ground.Raycast(ray, out float dist))
        {
            Vector3 worldPos = ray.GetPoint(dist);
            Vector3Int snappedPos = new Vector3Int(
                Mathf.FloorToInt(worldPos.x),
                0,
                Mathf.FloorToInt(worldPos.z)
            );

            if (prefab == null)
                OpenPlacementMenu(snappedPos);
            else
                PlaceGrid(prefab, snappedPos);

            e.Use();
        }
    }

    private void OpenPlacementMenu(Vector3Int pos)
    {
        GenericMenu menu = new GenericMenu();
        for (int i = 0; i < prefabs.Length; i++)
        {
            if (prefabs[i] != null)
            {
                GameObject captured = prefabs[i];
                menu.AddItem(new GUIContent(captured.name), false, () => PlaceGrid(captured, pos));
            }
            else
            {
                menu.AddDisabledItem(new GUIContent($"Prefab {i + 1} atanmamış"));
            }
        }
        menu.ShowAsContext();
    }

    private void PlaceGrid(GameObject prefab, Vector3Int pos)
    {
        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        instance.name = $"Grid_{pos.x}_{pos.y}_{pos.z}";
        gridSOData.entries.Add(new GridRawData { position = pos });
        EditorUtility.SetDirty(gridSOData);
        instance.transform.position = new Vector3(pos.x, 0f, pos.z);
        Undo.RegisterCreatedObjectUndo(instance, "Place Grid Tile");
        EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
    }
}