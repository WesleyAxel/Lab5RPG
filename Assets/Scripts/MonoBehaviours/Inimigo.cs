using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : Caractere
{
    float pontosVida;                   // valor da saude do inimigo
    public int forcaDano;               // valor do dano do inimigo
    Coroutine danoCoroutine;

    void Start()
    {
        
    }

    private void OnEnable()             // reseta o inimgo
    {
        ResetCaractere();
    }

    private void OnCollisionEnter2D(Collision2D collision)          // verifica quando entra em colisão com algum objeto
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if( danoCoroutine == null)
            {
                //starta a coroutine
                danoCoroutine = StartCoroutine(player.DanoCaractere(forcaDano, 1.0f));
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)       // verifica quando sai em colisão de algum objeto
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if( danoCoroutine != null)
            {
                //termina a coroutine
                StopCoroutine(danoCoroutine);
                danoCoroutine = null;
            }
        }
    }

    public override IEnumerator DanoCaractere(int dano, float intervalo)        // calcula o dano inflingido ao player
    {
        while (true)
        {
            StartCoroutine(FlickerCaractere());
            pontosVida = pontosVida - dano;
            if(pontosVida <= float.Epsilon)
            {
                KillCaractere();
                break;
            }
            if( intervalo > float.Epsilon)
            {
                yield return new WaitForSeconds(intervalo);
            }
            else
            {
                break;
            }
        }
    }

    public override void ResetCaractere()               // reseta o inimigo
    {
        pontosVida = inicioPontosDano;
    }

    void Update()
    {
        
    }
}
