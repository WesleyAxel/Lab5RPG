using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PontosDano pontosDano;                   //objeto de leitura dos dados de quantos danos tem o player
    public Player caractere;                        // recebera o objeto do player
    public Image medidorImage;                      // recebe a barra de medição
    public Text pdTexto;                            // recebe os dados de PD Texto  
    float maxPontosDano;                            // armazena a variavel limitante de pontos dano
    // Start is called before the first frame update
    void Start()        // metodo start que inicia o hp do palyer
    {
        maxPontosDano = caractere.MaxPontosDano;
    }

    // Update is called once per frame
    void Update()           // metodo update que vai atualizando a barra do player conforme o mesmo vai perdendo ou ganhando vida
    {
        if (caractere != null)
        {
            medidorImage.fillAmount = pontosDano.valor / maxPontosDano;
            pdTexto.text = "PD: " + (medidorImage.fillAmount * 100);
        }
    }
}
