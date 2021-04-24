using UnityEngine;
using UnityEngine.SceneManagement;

public class MudaScene : MonoBehaviour
{
    public int indexScene;
    private void OnTriggerEnter2D(Collider2D collision)         // muda de cena com o player entrando em colisão com o gameobject com base no index da cena
    {
        if(collision.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(indexScene);
        }
       
    }
}
