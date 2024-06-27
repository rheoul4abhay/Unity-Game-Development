using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    private RectTransform bar; //To access scale property of Bar object

    public Image barImage; //To accesss the Image property of the Bar Object so that we can change its color to red if it
    //falls down below a certain limit

    void Start()
    {
        bar = GetComponent<RectTransform>();
        barImage = GetComponent<Image>();
        bar.localScale = new Vector3(Health.totalHealth, 1f);
        if(Health.totalHealth < 0.3f)
        {
            barImage.color = Color.red;//So that at the start of next lvl,if we had red healthBar in previous lvl,it will
            //stay red and wont reset back to green.
        }
    }
     
    public void Damage(float damage) //Will be called in playerController to keep updating players health if it takes dmg
    {
        if ((Health.totalHealth -= damage) >= 0) //To neglect negative value for totalHealth,as totalHealth will be refering
            //to the scale of the bar which will be updated if player recieved health,so the scale cannot be below 0 or fked up
        {
            Health.totalHealth -= damage;
        }
        else
        {
            Health.totalHealth = 0;
        }
        if(Health.totalHealth < 0.3f)
        {
            barImage.color = Color.red;
        }
        setScale(Health.totalHealth); //To update the health bar by calling the setScale function everytime player takes dmg
    }

    public void setScale(float size) //To take dmg from spikes and reduces the healthbar by decreasing the scale of bar.
    //we will pass totalHealth(remaining) here as the arguement. 
    {
        bar.localScale = new Vector3(size, 1f);//This will change the scale of the health bar based on the remaining health
    }
    
}
