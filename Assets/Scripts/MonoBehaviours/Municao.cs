using UnityEngine;

public class Municao : MonoBehaviour
{
    public int danoCausado;                     //poder de dano

    private void OnTriggerEnter2D(Collider2D collision)             // metodo que verifica quando a munição entra em contato com a colisão do inimigo
    {
        if (collision is BoxCollider2D)
        {
            Inimigo inimigo = collision.gameObject.GetComponent<Inimigo>();
            StartCoroutine(inimigo.DanoCaractere(danoCausado,0.0f));
            gameObject.SetActive(false);
        }
    }
}
