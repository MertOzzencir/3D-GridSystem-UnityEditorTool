// Editor/GridPlacerTool.cs
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

public class GridPlacerTool : EditorWindow
{
    private GameObject[] prefabs = new GameObject[0];
    private Placeable[] placeables = new Placeable[0];
    private GridDataSO gridSOData;

    private IPlacerMode[] modes;
    private IPlacerMode activeMode;

    [MenuItem("Tools/Grid Placer")]
    public static void OpenWindow()
    {
        GridPlacerTool window = GetWindow<GridPlacerTool>();
        window.titleContent = new GUIContent("Grid Placer");
        window.Show();
    }

    private void OnEnable()
    {
        LoadPrefs();
        BuildModes();
        SceneView.duringSceneGui += OnSceneGUI;
    }

    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        activeMode = null;
        SavePrefs();
    }

    private void BuildModes()
    {
        modes = new IPlacerMode[]
        {
            new GridPlaceMode(gridSOData, ref prefabs),
            new PlaceableMode(gridSOData, ref placeables)
        };
    }

    private void OnGUI()
    {
        gridSOData = (GridDataSO)EditorGUILayout.ObjectField("Grid Data", gridSOData, typeof(GridDataSO), false);

        GUILayout.Space(10);
        GUILayout.Label("Modlar", EditorStyles.boldLabel);

        foreach (var mode in modes)
        {
            GUI.color = activeMode == mode ? mode.ModeColor : Color.white;
            if (GUILayout.Button(mode.ModeName))
                activeMode = activeMode == mode ? null : mode;
            GUI.color = Color.white;
        }

        GUILayout.Space(10);

        activeMode?.DrawUI();
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (activeMode == null) return;

        Event e = Event.current;

        if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Space)
        {
            activeMode = null;
            Repaint();
            e.Use();
            return;
        }

        int controlID = GUIUtility.GetControlID(FocusType.Passive);
        HandleUtility.AddDefaultControl(controlID);

        activeMode.OnSceneGUI(e);
        Repaint();
    }

    private void SavePrefs()
    {
        EditorPrefs.SetInt("GridPlacer_PrefabCount", prefabs.Length);
        for (int i = 0; i < prefabs.Length; i++)
            EditorPrefs.SetString($"GridPlacer_Prefab{i}", AssetDatabase.GetAssetPath(prefabs[i]));

        EditorPrefs.SetInt("GridPlacer_PlaceableCount", placeables.Length);
        for (int i = 0; i < placeables.Length; i++)
            EditorPrefs.SetString($"GridPlacer_Placeable{i}", AssetDatabase.GetAssetPath(placeables[i]));

        EditorPrefs.SetString("GridPlacer_GridData", AssetDatabase.GetAssetPath(gridSOData));
    }

    private void LoadPrefs()
    {
        int prefabCount = EditorPrefs.GetInt("GridPlacer_PrefabCount", 0);
        prefabs = new GameObject[prefabCount];
        for (int i = 0; i < prefabCount; i++)
            prefabs[i] = AssetDatabase.LoadAssetAtPath<GameObject>(EditorPrefs.GetString($"GridPlacer_Prefab{i}"));

        int placeableCount = EditorPrefs.GetInt("GridPlacer_PlaceableCount", 0);
        placeables = new Placeable[placeableCount];
        for (int i = 0; i < placeableCount; i++)
            placeables[i] = AssetDatabase.LoadAssetAtPath<Placeable>(EditorPrefs.GetString($"GridPlacer_Placeable{i}"));

        gridSOData = AssetDatabase.LoadAssetAtPath<GridDataSO>(EditorPrefs.GetString("GridPlacer_GridData"));
    }
}