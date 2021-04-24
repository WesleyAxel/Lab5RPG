using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : Caractere
{
    public HealthBar healthBarPrefab;                   //referencia ao objeto prefav da healthbar
    HealthBar healthbar;
    public Inventario inventarioPrefab;                 //referencia ao prefab do inventario
    Inventario inventario;
    public PontosDano pontosDano;                       //valor da vida do objeto
    [HideInInspector]
    public bool temArma = false;
    public bool temChave = false;

    private void Start()                                                                        // metodo start que instancia o inventario e barra de hp
    {
        //healthbar.caractere = this;
        inventario = Instantiate(inventarioPrefab);
        pontosDano.valor = inicioPontosDano;
        healthbar = Instantiate(healthBarPrefab);
    }

    public override IEnumerator DanoCaractere(int dano, float intervalo)                      // metodo que calcula o hp do player e o dano 
    {
        while (true)
        {
            StartCoroutine(FlickerCaractere());
            pontosDano.valor = pontosDano.valor - dano;
            if(pontosDano.valor <= float.Epsilon)
            {
                KillCaractere();
                break;
            }
            if(intervalo > float.Epsilon)
            {
                yield return new WaitForSeconds(intervalo);
                break;
            }
            else
            {
                break;
            }
        }
    }

    public override void KillCaractere()            //metodo que deleta o player e carrega a cena de game over
    {
        base.KillCaractere();
        Destroy(healthbar.gameObject);
        Destroy(inventario.gameObject);
        SceneManager.LoadScene("Lab4RPG_GameOver");
    }

    public override void ResetCaractere()       // metodo que reinicia o player
    {
        inventario = Instantiate(inventarioPrefab);
        healthbar = Instantiate(healthBarPrefab);
        healthbar.caractere = this;
        pontosDano.valor = inicioPontosDano;
    }

    private void OnTriggerEnter2D(Collider2D collision)             // metodo que verifica quando o player entra em contato com algum objeto de colisão
    {
        if (collision.gameObject.CompareTag("Coletavel"))
        {
            Item DanoObjeto = collision.gameObject.GetComponent<Consumable>().item;
            if(DanoObjeto != null)
            {
                bool DeveDesaparecer = false;
                print("Acertou: " + DanoObjeto.nomeObjeto);
                switch (DanoObjeto.tipoItem)
                {
                    case Item.TipoItem.ESTILINGUE1:
                        DeveDesaparecer = inventario.AddItem(DanoObjeto);
                        temArma = true;
                        break;
                    case Item.TipoItem.MUNICAO:
                        for(int i=0;  i < DanoObjeto.quantidade; i++)
                        {
                            DeveDesaparecer = inventario.AddItem(DanoObjeto);
                        }
                        break;
                    case Item.TipoItem.CHAVE:
                        DeveDesaparecer = inventario.AddItem(DanoObjeto);
                        temChave = true;
                        break;
                    case Item.TipoItem.MOEDA:
                        DeveDesaparecer = inventario.AddItem(DanoObjeto);
                        break;
                    case Item.TipoItem.ANEL:
                        DeveDesaparecer = inventario.AddItem(DanoObjeto);
                        SceneManager.LoadScene("Lab4RPG_Fimdejogo");
                        break;
                    case Item.TipoItem.VIDA:
                        DeveDesaparecer = AjustePontosDano(DanoObjeto.quantidade);
                        break;
                    case Item.TipoItem.PAO:
                        DeveDesaparecer = AjustePontosDano(DanoObjeto.quantidade);
                        break;
                    default:
                        break;
                }
                if (DeveDesaparecer)
                {
                    collision.gameObject.SetActive(false);
                }
            }
            
        }
        else if (collision.gameObject.CompareTag("Porta")){
            if(temChave == true)
            {
                collision.gameObject.SetActive(false);
            }
        }
    }

    public bool AjustePontosDano(int quantidade)                // metodo que ajusta a vida do player
    {
        if(pontosDano.valor < MaxPontosDano)
        {
            pontosDano.valor = pontosDano.valor + quantidade;
            print("Ajustado PD por: " + quantidade + ". Novo Valor = " + pontosDano.valor);
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool verificaMunicao()                   // metodo que verifica se o player possui munição
    {
        if (inventario.removeMunicao() == true)
        {
            return true;
        }
        return false;
    }

}


