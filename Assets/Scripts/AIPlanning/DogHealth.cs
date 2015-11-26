﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DogHealth : MonoBehaviour
{
    private float currentHealth = 100f;
    private float totalHealth = 100f;
    private Transform healthBar;
    private Animator anim;
    private bool isAlive = true;
    // Use this for initialization
    void Awake()
    {
        anim = this.GetComponent<Animator>();
        healthBar = this.transform.Find("HealthBarCanvas/HealthColor");
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            ReduceHealth(10f);
        }
    }
    public void ReduceHealth(float difference)
    {
        if (isAlive)
        {
            float newHealth = Mathf.Max(0, currentHealth - difference);
            float speed = 1f;
            currentHealth = newHealth;
            StartCoroutine(SetBar(newHealth, speed));
        }
    }

    public void AddHealth(float difference)
    {
        if (isAlive)
        {
            float newHealth = Mathf.Min(totalHealth, currentHealth + difference);
            float speed = 1f;
            currentHealth = newHealth;
            //		StartCoroutine (SetHealth (newHealth, speed));
            StartCoroutine(SetBar(newHealth, speed));
        }
    }

    IEnumerator SetBar(float input, float speed)
    {
        if (isAlive)
        {
            float ratio = input / totalHealth;
            Vector3 dest = new Vector3(ratio, 1f, 1f);
            while (healthBar.transform.localScale != dest)
            {
                Vector3 curScale = healthBar.transform.localScale;
                healthBar.transform.localScale = Vector3.MoveTowards(curScale, dest, speed * Time.deltaTime);
                yield return null;
            }
            if (currentHealth <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HealthPack")
        {
            Debug.Log("reached health pack");
            AddHealth(15f);
            Destroy(other.gameObject);
        }
    }

    public float GetHealth()
    {
        return currentHealth;
    }
}