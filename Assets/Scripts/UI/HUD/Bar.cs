using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public Image bar;
    public float drainSpeed;
    public float amount;
    public float maxAmount;
    
    // Update is called once per frame
    void Update()
    {

        var currentValue = amount;
        if (currentValue > 0)
        {
            currentValue -= Time.deltaTime * drainSpeed; 

            currentValue = Mathf.Max(currentValue, 0.0f);

            float percentage = currentValue / maxAmount;

            DrainBar(Mathf.Lerp(bar.fillAmount, percentage, Time.deltaTime * 5.0f));
        }
        else
        {
            DrainBar(Mathf.Lerp(bar.fillAmount, 0, Time.deltaTime * 5.0f));            
        }
    }

    // Set the fill amount of the bar based on the percentage
    private void DrainBar(float percentage)
    {
        bar.fillAmount = percentage;
    }
}
