using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class NetworkManagerUI : MonoBehaviour
{

    [SerializeField] private Button hostBtn, clientBtn, serverBtn;


    private void Awake()
    {

        serverBtn.onClick.AddListener(() => { NetworkManager.Singleton.StartServer(); });

        hostBtn.onClick.AddListener(() => { NetworkManager.Singleton.StartHost(); });

        clientBtn.onClick.AddListener(() => { NetworkManager.Singleton.StartClient(); });

    }

}
