using System.Collections;
using System.Collections.Generic;
using NajakBoi.Scripts.Systems.Economy;
using TMPro;
using UnityEngine;

namespace NajakBoi.Scripts.UI
{
    public class DropDisplay : MonoBehaviour
    {
        public GameObject displayPrefab;
        public float displayTime = 5f;
        public float fadeSpeed = 1f;

        public void DisplayDrops(List<Drop> drops)
        {
            StartCoroutine(Display(drops));
        }
        private IEnumerator Display(List<Drop> drops)
        {
            var displayers = new List<TextMeshProUGUI>();
            foreach (var item in drops)
            {
                var instance = Instantiate(displayPrefab, transform);
                var tmp = instance.GetComponent<TextMeshProUGUI>();
                tmp.text = $"+{item.Amount} {item.Type}";
                displayers.Add(tmp);
            }

            while (displayTime > 0f)
            {
                displayTime -= Time.deltaTime;
                
                if (displayTime > 3f)
                    yield return null;
                
                foreach (var tmp in displayers)
                {
                    var alpha = tmp.color.a;
                    alpha -= Time.deltaTime * fadeSpeed;
                    tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, alpha);
                }

                yield return null;
            }

            Destroy(gameObject);
        }
        
        
        
    }
}
