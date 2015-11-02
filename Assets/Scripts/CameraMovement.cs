using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour
{
    private GameObject player;
    private Vector3 camOffset;
    private float speed;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        camOffset = this.transform.position - player.transform.position;
        speed = player.GetComponent<PlayerMovement>().speed*3/4;
    }

    void Update()
    {
        Vector3 targetPos = player.transform.position + camOffset;
        if(this.transform.position!=targetPos)
        {
            this.transform.position = Vector3.MoveTowards(transform.position, targetPos, speed*Time.deltaTime);
        }
    }
}
