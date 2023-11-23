using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameScreen : MonoBehaviour
{
    public TextMeshProUGUI endTextTmp;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameEnded(PlayerId looser)
    {
        gameObject.SetActive(true);
        endTextTmp.text = looser == PlayerId.Opponent ? "You Won!" : "You Lost!";
        endTextTmp.color = looser == PlayerId.Opponent ? Color.green : Color.red;
    }
}
