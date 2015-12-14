using UnityEngine;
using System.Collections;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance;
	
	void Awake ()
	{
		if (_instance == null) {
			_instance = GameObject.FindObjectOfType<T> ();
		}
	}
	
	public static T Instance {
		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType<T> ();
				DontDestroyOnLoad (_instance);
			}
			if (_instance == null) {
				GameObject singleton = new GameObject ();
				_instance = singleton.AddComponent<T> ();
				singleton.name = typeof(T).ToString ();
				DontDestroyOnLoad (singleton);
			}
			return _instance;
		}
	}
}
