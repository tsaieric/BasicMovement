using UnityEngine;
using System.Collections;

public class StartController : MonoBehaviour
{
	public void LoadStart ()
	{
		Application.LoadLevel ("Main");
	}

	public void Quit ()
	{
		Application.Quit ();
	}
}
