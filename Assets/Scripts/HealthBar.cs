using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image healthBarFill;
    public float drainSpeed;
    public CharacterController character;
    
    // Update is called once per frame
    void Update()
    {
        var currentHealth = character.currentHealth;
        // Example: Drain the health over time
        if (currentHealth > 0)
        {
            currentHealth -= Time.deltaTime * drainSpeed; // Adjust the speed of draining

            // Ensure that the health doesn't go below 0
            currentHealth = Mathf.Max(currentHealth, 0.0f);

            // Calculate health percentage
            float healthPercentage = currentHealth / character.maxHealth;

            // Lerp the health bar fill amount
            SetHealthBarFill(Mathf.Lerp(healthBarFill.fillAmount, healthPercentage, Time.deltaTime * 5.0f));
        }
        else
        {
            // Lerp the health bar fill amount
            SetHealthBarFill(Mathf.Lerp(healthBarFill.fillAmount, 0, Time.deltaTime * 5.0f));
            
        }
    }
    // Set the fill amount of the health bar based on the health percentage
    void SetHealthBarFill(float percentage)
    {
        healthBarFill.fillAmount = percentage;
    }
}
