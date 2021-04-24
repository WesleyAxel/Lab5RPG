using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Caractere : MonoBehaviour
{
    // classe abstract
    
    public float MaxPontosDano;               //max valor do pontos dano
    public float inicioPontosDano;            //min valor dos pontos dano

    public abstract void ResetCaractere();

    public virtual IEnumerator FlickerCaractere()           // muda o sprite para "representar" o dano obtido
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    public abstract IEnumerator DanoCaractere(int dano, float intervalo);
    public virtual void KillCaractere()
    {
        Destroy(gameObject);
    }
}
