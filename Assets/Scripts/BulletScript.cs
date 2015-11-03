using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour
{

    private float bulletSpeed = 1000f;
    // Use this for initialization
    void Start()
    {
        Destroy(this.gameObject, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision col)
    {
        //Destroy (col.gameObject);
        if (col.collider.gameObject.name == "Cube")
        {
            Debug.Log("collision");
            Destroy(col.collider.gameObject);
        }
    }
}

