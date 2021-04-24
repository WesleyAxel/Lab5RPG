using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item")]

public class Item : ScriptableObject
{
    public string nomeObjeto;
    public Sprite sprite;
    public int quantidade;
    public bool empilhavel;

    public enum TipoItem
    {
        MOEDA,  
        VIDA,
        CHAVE,                      //1
        PAO,                        
        ESTILINGUE1,                //2
        MUNICAO,                     //3
        ANEL                        //4 
    }

    public TipoItem tipoItem;
}
