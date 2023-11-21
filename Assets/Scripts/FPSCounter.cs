using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    private float deltaTime = 0.0f;
    private float fps = 0.0f;
    private GUIStyle style;
    private Rect rect;

    private void Start()
    {
        style = new GUIStyle();
        style.alignment = TextAnchor.UpperLeft;
        style.fontSize = 24;
        style.normal.textColor = Color.white; // Задаємо білий колір шрифту
        rect = new Rect(10, 10, 200, 50);
    }


    private void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        fps = 1.0f / deltaTime;
    }

    private void OnGUI()
    {
        GUI.Label(rect, "FPS: " + fps.ToString("F2"), style);
    }
}
