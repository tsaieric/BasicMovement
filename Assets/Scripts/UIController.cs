using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UIController : Singleton<UIController>
{
    public Text foodText;
    public Text grenadeText;
    public float playerHealth;
    public float dogHealth;
    public int grenadesLeft = 20;
    public int foodLeft = 10;
    // Use this for initialization
    void Start()
    {
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
