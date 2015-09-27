using UnityEngine;
using System.Collections;

public class Zombie : MonoBehaviour {
    private MoveController moveController;
    private Animator anim;
    // Use this for initialization
	void Start () {
        moveController = this.GetComponent<MoveController>();
        anim = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
