using UnityEngine;
using System.Collections;

public class FPSController : MonoBehaviour
{
	GUIStyle style = new GUIStyle ();
	Rect rect;
	void Start ()
	{
		rect = new Rect (0, 0, Screen.width, Screen.height * 4 / 100);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = 30;
		style.normal.textColor = Color.yellow;
	}

	void OnGUI ()
	{
		float ms = Time.deltaTime * 1000.0f;
		float fps = 1.0f / Time.deltaTime;
		string text = string.Format ("{0:0.0} ms ({1:0.} fps)", ms, fps);
		GUI.Label (rect, text, style);
	}
}