using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UIController : Singleton<UIController>
{
	public Image[] playerHearts;
	public Image[] dogHearts;
    public Text foodText;
    public Text grenadeText;
    public int grenadesLeft = 15;
    public int foodLeft = 10;
	private PlayerHealth playerHealth;
	private DogHealth dogHealth;
    // Use this for initialization
    void Start()
    {
		dogHealth = GameObject.FindGameObjectWithTag("Dog").GetComponent<DogHealth>();
		playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealth>();
        foodText.text = foodLeft.ToString();
        grenadeText.text = grenadesLeft.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            DecreaseFoodCount();
        }
		SetPlayerHealth();
		SetDogHealth();

    }

	public void SetPlayerHealth(){
		float currentHealth = playerHealth.GetHealth();
		float heartRatio = 100/playerHearts.Length;
		float heartHealth = 0;
		for(int x=0;x<playerHearts.Length;x++) {
			heartHealth = heartRatio*x;
			if(currentHealth > heartHealth) {
				playerHearts[x].enabled=true;
			} else {
				playerHearts[x].enabled=false;
			}
		}
	}

	public void SetDogHealth(){
		float currentHealth = dogHealth.GetHealth();
		float heartRatio = 100/dogHearts.Length;
		float heartHealth = 0;
		for(int x=0;x<dogHearts.Length;x++) {
			heartHealth = heartRatio*x;
			if(currentHealth > heartHealth) {
				dogHearts[x].enabled=true;
			} else {
				dogHearts[x].enabled=false;
			}
		}
	}

    public void DecreaseFoodCount()
    {
        if (foodLeft>0)
        {
            foodLeft--;
            foodText.text = foodLeft.ToString();
        }
    }

    public void DecreaseGrenadeCount()
    {
        if(grenadesLeft>0) {
            grenadesLeft--;
            grenadeText.text = grenadesLeft.ToString();
        }
    }
}
