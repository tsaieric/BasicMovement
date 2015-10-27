using UnityEngine;
using System.Collections;

public class FPSController : MonoBehaviour
{
	float deltaTime = 0.0f;
	GUIStyle style = new GUIStyle ();
	Rect rect;
	int w, h;
	void Start ()
	{
		w = Screen.width;
		h = Screen.height;
		rect = new Rect (0, 0, w, h * 4 / 100);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 4 / 100;
		style.normal.textColor = Color.yellow;
	}
	void Update ()
	{
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
	}

	void OnGUI ()
	{
		float ms = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		string text = string.Format ("{0:0.0} ms ({1:0.} fps)", ms, fps);
		GUI.Label (rect, text, style);
	}
}