using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPGGameManager : MonoBehaviour
{
    public static RPGGameManager instaciaCompartilhada = null;
    public CameraManager cameraManager;

    public PontoSpawn playerPontoSpawn;

    private void Awake()
    {
        if(instaciaCompartilhada != null && instaciaCompartilhada != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instaciaCompartilhada = this;
        }
    }
    void Start()            // metodo start que inicia a cena
    {
        SetupScene();
    }

    
    void Update()
    {
        
    }

    public void SetupScene()        // metodo que spawna o player
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()       // metodo que spawna o player com base na posição do PlayerPontoSpawn
    {
        if(playerPontoSpawn != null)
        {
            GameObject player = playerPontoSpawn.SpawnO();
            cameraManager.virtualCamera.Follow = player.transform;
        }
    }
}
