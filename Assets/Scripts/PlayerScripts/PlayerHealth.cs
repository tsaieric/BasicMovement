using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public Image blackOverlay;
    private float currentHealth = 100f;
    private float totalHealth = 100f;
    private Transform healthBar;
    private Animator anim;
    private bool isAlive = true;
    // Use this for initialization
    void Awake()
    {
        blackOverlay.gameObject.SetActive(true);
        blackOverlay.canvasRenderer.SetAlpha(0);
        anim = this.GetComponent<Animator>();
        healthBar = this.transform.Find("HealthBarCanvas/HealthColor");
    }

    public void ReduceHealth(float difference)
    {
        if (isAlive)
        {
            float newHealth = Mathf.Max(0, currentHealth - difference);
            float speed = 1f;
            currentHealth = newHealth;
            StartCoroutine(FlashRed(.2f));
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

    //IEnumerator SetHealth(float input, float speed)
    //{
    //    while (currentHealth != input)
    //    {
    //        currentHealth = Mathf.MoveTowards(currentHealth, input, speed * Time.deltaTime);
    //        yield return null;
    //    }
    //}

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
                StartCoroutine(EndGame());
            }
        }
    }

    IEnumerator FlashRed(float time)
    {
        blackOverlay.color = Color.red;
        blackOverlay.CrossFadeAlpha(.4f, time, false);
        yield return new WaitForSeconds(time);
        blackOverlay.canvasRenderer.SetAlpha(0f);
    }
    IEnumerator EndGame()
    {
        Time.timeScale = .2f;
        anim.SetTrigger("Die");
        isAlive = false;
        yield return new WaitForSeconds(.2f);
        blackOverlay.color = Color.black;
        blackOverlay.CrossFadeAlpha(1f, 1f, false);
        yield return new WaitForSeconds(1f);
        Application.LoadLevel("EndScene");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "HealthPack")
        {
            Destroy(other.gameObject);
            AddHealth(15f);
        }
    }
}