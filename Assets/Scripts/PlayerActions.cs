using UnityEngine;
using System.Collections;


public class PlayerActions : MonoBehaviour
{
    public float radarSpeed = 10f;
    private PlayerMovement movement;
    private GameObject radar;
    // Use this for initialization
    void Start()
    {
        movement = this.GetComponent<PlayerMovement>();
        radar = GameObject.FindGameObjectWithTag("Radar");
        radar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        RadarControl();
    }

    void RadarControl()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            radar.SetActive(true);
            movement.disabled = true;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Vector3 curScale = radar.transform.localScale;
            radar.transform.localScale = Vector3.MoveTowards(curScale, curScale * 100, Time.deltaTime * radarSpeed);
            radar.transform.Rotate(Vector3.up, 1f);
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            StartCoroutine(ShrinkRadar());
        }
    }
    IEnumerator ShrinkRadar()
    {
        while(radar.transform.localScale!=Vector3.one)
        {
            radar.transform.Rotate(Vector3.up, -1f);
            Vector3 curScale = radar.transform.localScale;
            radar.transform.localScale = Vector3.MoveTowards(curScale, Vector3.one, Time.deltaTime * 100f);
            yield return null;
        }
        radar.SetActive(false);
        movement.disabled = false;
    }
}
