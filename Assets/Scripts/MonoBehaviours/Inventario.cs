using UnityEngine;
using UnityEngine.UI;

public class Inventario : MonoBehaviour
{
    public GameObject slotPrefab;                       // objeto que recebe prefab slot
    public const int numSlots = 5;                      // numero fixo de slots
    Image[] itemImages = new Image[numSlots];           // array de imagens
    public Item[] items = new Item[numSlots];                  // array de items
    GameObject[] slots = new GameObject[numSlots];      // array de slots

    // Start is called before the first frame update
    void Start()                                       // metodo que inicia e cria os slots
    {
        CriaSlots();
    }

    public void CriaSlots()                           // cria os slots do invetario
    {   
        if(slotPrefab != null)
        {
            for(int i =0; i<numSlots; i++)
            {
                GameObject novoSlot = Instantiate(slotPrefab);
                novoSlot.name = "ItemSlot_" + i;
                novoSlot.transform.SetParent(gameObject.transform.GetChild(0).transform);
                slots[i] = novoSlot;
                itemImages[i] = novoSlot.transform.GetChild(1).GetComponent<Image>();
            }
        }
    }

    public bool AddItem(Item itemToAdd)                 // adiciona os items no inventario com base em qual item foi pego
    {
        for(int i=0; i<items.Length; i++)
        {
            if(items[i]!=null && items[i].tipoItem == itemToAdd.tipoItem && itemToAdd.empilhavel == true)
            {
                items[i].quantidade = items[i].quantidade + 1;
                Slot slotScript = slots[i].gameObject.GetComponent<Slot>();
                Text quantidadeTexto = slotScript.QtDTexto;
                quantidadeTexto.enabled = true;
                quantidadeTexto.text = items[i].quantidade.ToString();
                return true;
            }
            if(items[i] == null)
            {
                items[i] = Instantiate(itemToAdd);
                items[i].quantidade = 1;
                itemImages[i].sprite = itemToAdd.sprite;
                itemImages[i].enabled = true;
                return true;
            }
        }
        return false;
    }

    public bool removeMunicao()                         // metodo que remove munição do inventario quando a mesma é disparada
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] != null && items[i].tipoItem == Item.TipoItem.MUNICAO)
            {
                if (items[i].quantidade > 0)
                {
                    items[i].quantidade--;
                    Slot slotScript = slots[i].gameObject.GetComponent<Slot>();
                    Text quantidadeTexto = slotScript.QtDTexto;
                    quantidadeTexto.enabled = true;
                    quantidadeTexto.text = items[i].quantidade.ToString();
                    return true;
                }
            }

        }
        return false;
    }

}
