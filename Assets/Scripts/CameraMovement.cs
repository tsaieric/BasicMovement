using UnityEngine;
using System.Collections;

public enum CameraBehavior
{
    Seek
}
public class CameraMovement : MonoBehaviour
{
    private GameObject player;
    private CameraBehavior behavior;
    private MoveController moveController;
    private Vector3 camOffset;
    private float speed;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        camOffset = this.transform.position - player.transform.position;
        speed = player.GetComponent<PlayerMovement>().speed*3/4;
        //behavior = CameraBehavior.Seek;
        //moveController = this.GetComponent<MoveController>();
        //moveController.rotatable = false;
        //moveController.maxSpeed = player.GetComponent<PlayerMovement>().speed*1.5f;
    }

    void Update()
    {
        Vector3 targetPos = player.transform.position + camOffset;
        if(this.transform.position!=targetPos)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, targetPos, speed*Time.deltaTime);
        }
        //moveController.ResetSteering();
        //moveController.Arrive(player.transform.position+Vector3.forward*-20f, 10f);
        //moveController.UpdateEverything();
    }
}
