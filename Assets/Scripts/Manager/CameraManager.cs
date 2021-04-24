using Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instanciaCompartilhada = null;

    [HideInInspector]
    public CinemachineVirtualCamera virtualCamera;

    private void Awake()            // metodo que verifica a instancia e determina que a camera virtual deve seguir o player
    {
        if( instanciaCompartilhada != null && instanciaCompartilhada != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instanciaCompartilhada = this;
        }
        GameObject vCamGameObject = GameObject.FindWithTag("Virtual Camera");
        virtualCamera = vCamGameObject.GetComponent<CinemachineVirtualCamera>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
