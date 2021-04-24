using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]

public class Armas : MonoBehaviour
{
    public GameObject municaoPrefab;                // armazena o prefab da munição
    static List<GameObject> municaoPiscina;         // pool de munição
    public int tamanhoPiscina;                      // tamanho do pool
    public float velocidadeArma;                    // velocidade da municao

    bool atirando;
    [HideInInspector]
    public Animator animator;

    Camera cameraLocal;

    float slopePositivo;
    float slopeNegativo;

    enum Quadrante
    {
        Leste,
        Sul,
        Oeste,
        Norte
    }


    private void Start()                                                                                                            // metodo de start que carrega as diretrizes e os calculos de quadrantes
    {
        animator = GetComponent<Animator>();
        atirando = false;
        cameraLocal = Camera.main;
        Vector2 abaixoEsquerda = cameraLocal.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 acimaDireita = cameraLocal.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        Vector2 acimaEsquerda = cameraLocal.ScreenToWorldPoint(new Vector2(0, Screen.height));
        Vector2 abaixoDireita = cameraLocal.ScreenToWorldPoint(new Vector2(Screen.width, 0));
        slopePositivo = PegaSlope(abaixoEsquerda, acimaDireita);
        slopeNegativo = PegaSlope(acimaEsquerda, abaixoDireita);
    }

    bool AcimaSlopePositivo(Vector2 posicaoEntrada)                                                                                 // metodo que calcula o slope positivo
    {
        Vector2 posicaoPlayer = gameObject.transform.position;
        Vector2 posicaoMouse = cameraLocal.ScreenToWorldPoint(posicaoEntrada);
        float interseccaoY = posicaoPlayer.y - (slopePositivo * posicaoPlayer.x);
        float entradaInterseccao = posicaoMouse.y - (slopePositivo * posicaoMouse.x);
        return entradaInterseccao > interseccaoY;
    }

    bool AcimaSlopeNegativo(Vector2 posicaoEntrada)                                                                                 // metodo que calcula o slope negativo
    {
        Vector2 posicaoPlayer = gameObject.transform.position;
        Vector2 posicaoMouse = cameraLocal.ScreenToWorldPoint(posicaoEntrada);
        float interseccaoY = posicaoPlayer.y - (slopeNegativo * posicaoPlayer.x);
        float entradaInterseccao = posicaoMouse.y - (slopeNegativo * posicaoMouse.x);
        return entradaInterseccao > interseccaoY;
    }

    Quadrante PegaQuadrante()                                                                                                       // metodo que pega o quadrante da posição do mouse
    {
        Vector2 posicaoMouse = Input.mousePosition;
        Vector2 posicaoPlayer = transform.position;
        bool acimaSlopePositivo = AcimaSlopePositivo(Input.mousePosition);
        bool acimaSlopeNegativo = AcimaSlopeNegativo(Input.mousePosition);
        if ( !acimaSlopePositivo && acimaSlopeNegativo)
        {
            return Quadrante.Leste;
        }
        else if (!acimaSlopePositivo && !acimaSlopeNegativo)
        {
            return Quadrante.Sul;
        }
        else if (acimaSlopePositivo && !acimaSlopeNegativo)
        {
            return Quadrante.Oeste;
        }
        else
        {
            return Quadrante.Norte;
        }
    }

    void UpdateEstado()                                                                                                             // metodo que atualiza o estado do player com base no quadrante para update da animação
    {
        if (atirando)
        {
            Vector2 vetorQuadrante;
            Quadrante quadranteEnum = PegaQuadrante();
            switch (quadranteEnum)
            {
                case Quadrante.Leste:
                    vetorQuadrante = new Vector2(1.0f, 0.0f);
                    break;
                case Quadrante.Sul:
                    vetorQuadrante = new Vector2(0.0f, -1.0f);
                    break;
                case Quadrante.Oeste:
                    vetorQuadrante = new Vector2(-1.0f, 0.0f);
                    break;
                case Quadrante.Norte:
                    vetorQuadrante = new Vector2(0.0f, 1.0f);
                    break;
                default:
                    vetorQuadrante = new Vector2(0.0f, 0.0f);
                    break;
            }
            animator.SetBool("Atirando", true);
            animator.SetFloat("AtirX", vetorQuadrante.x);
            animator.SetFloat("AtirY", vetorQuadrante.y);
            atirando = false;
        }
        else
        {
            animator.SetBool("Atirando", false);
        }
    }

    public void Awake()                                                                                                             // metodo que calcula o pool de munição quando iniciado
    {
        if (municaoPiscina == null)
        {
            municaoPiscina = new List<GameObject>();
        }
        for( int i=0; i < tamanhoPiscina; i++)
        {
            GameObject municaoO = Instantiate(municaoPrefab);
            municaoO.SetActive(false);
            municaoPiscina.Add(municaoO);
        }
    }

    private void Update()                                                                                                         // metodo update que verifica se o botão é pressionado para atirar
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(GetComponent<Player>().temArma == true)
            {
                if (GetComponent<Player>().verificaMunicao() == true)
                {
                    atirando = true;
                    DisparaMunicao();
                }
                
            }
        }
        UpdateEstado();
    }
        
    float PegaSlope(Vector2 ponto1, Vector2 ponto2)                                                                               // metodo que verifica qual o slope da posição de tiro
    {
        return (ponto2.y - ponto1.y) / (ponto2.x - ponto1.x);
    }

    public GameObject SpawnMunicao(Vector3 posicao)                                                                                 // metodo que inicia a munição no pool de munição
    {
        foreach (GameObject municao in municaoPiscina)
        {
            if (municao.activeSelf == false)
            {
                municao.SetActive(true);
                municao.transform.position = posicao;
                return municao;
            }
        }
        return null;
    }

    void DisparaMunicao()                                                                                                       // metodo que cria a munição e movimenta a mesma através de courotine
    {
        Vector3 posicaoMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        GameObject municao = SpawnMunicao(transform.position);
        if (municao != null)
        {
            Arco arcoScript = municao.GetComponent<Arco>();
            float duracaoTragetoria = 1.0f / velocidadeArma;
            StartCoroutine(arcoScript.arcoTragetoria(posicaoMouse, duracaoTragetoria));
        }
    }

    private void OnDestroy()                                                                                                    // metodo que destroi a munição no contato
    {
        municaoPiscina = null;
    }

}
