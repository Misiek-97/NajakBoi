using UnityEngine;
using UnityEngine.UI;

namespace NajakBoi.Scripts.UI.HUD
{
    public class ChargeBar : MonoBehaviour
    {
        public Image fillImage;
        public float fillSpeed = 0.1f;

        private float currentFill = 0f;

   

        void ChargeOverTime()
        {
            // Increase the fill amount over time
            currentFill += fillSpeed * Time.deltaTime;

            // Ensure the fill amount stays within the range [0, 1]
            currentFill = Mathf.Clamp01(currentFill);

            // Update the fill amount of the UI Image
            fillImage.fillAmount = currentFill;
        }

        // You can call this method with a passed-in float to set the fill amount directly
        public void SetFillAmount(float fillAmount)
        {
            gameObject.SetActive(true);
            // Ensure the fill amount stays within the range [0, 1]
            currentFill = Mathf.Clamp01(fillAmount);

            // Update the fill amount of the UI Image
            fillImage.fillAmount = currentFill;
        }
    }
}