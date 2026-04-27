using UnityEngine;

public interface IPlacerMode
{
    string ModeName { get; }
    Color ModeColor { get; }
    void OnSceneGUI(Event e);
    void DrawUI();
}