using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameCounter : MonoBehaviour
{
    private float deltatime = 0f;

    [SerializeField, Range(1, 100)]
    private int size = 25;

    [SerializeField]
    private Color color = Color.green;

    public bool isShow;


    // Update is called once per frame
    void Update()
    {
        deltatime += (Time.unscaledDeltaTime - deltatime) * 0.1f;


        if (Input.GetKeyDown(KeyCode.F1))
        {
            isShow = !isShow;
        }

        if (Input.GetKeyDown(KeyCode.F2))
            Application.targetFrameRate = 30;
        if (Input.GetKeyDown(KeyCode.F3))
            Application.targetFrameRate = 60;
        if (Input.GetKeyDown(KeyCode.F4))
            Application.targetFrameRate = 144;
        if (Input.GetKeyDown(KeyCode.F5))
            Application.targetFrameRate = -1;
    }

    private void OnGUI()
    {
        if (isShow)
        {
            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(30, 30, Screen.width, Screen.height);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = size;
            style.normal.textColor = color;

            float ms = deltatime * 1000f;
            float fps = 1.0f / deltatime;
            string text = string.Format("{0:0.} FPS ({1:0.0} ms)", fps, ms);

            GUI.Label(rect, text, style);
        }
    }
}
