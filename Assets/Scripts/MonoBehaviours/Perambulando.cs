using System.Collections;
using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]

[RequireComponent(typeof(CircleCollider2D))]

[RequireComponent(typeof(Animator))]
public class Perambulando : MonoBehaviour
{
    public float velocidadePerseguicao;             //velocidade do inimigo na area de spot
    public float velocidadePerambular;              //velocidade do inimigo andando normal
    float velocidadeCorrente;                       //velocidade do inimigo atribuida

    public float invervaloMudancaDirecao;           //tempo para alterar direcao
    public bool perseguePlayer;                     //indica se persegue ou não;

    Coroutine moverCoroutine;

    Rigidbody2D rb2D;                               // armazena o componente rigidBody2D
    Animator animator;                              // armazena o componente Animator
    Transform alvoTransform = null;                 // armazena o componente Transform do alvo
    Vector3 posicaoFinal;
    float anguloAtual = 0;                          // Angulo atribuido
    CircleCollider2D circleCollider;                // armazena o componente de spot
    void Start()                                    // metodo start que inicia os parametros
    {
        animator = GetComponent<Animator>();
        velocidadeCorrente = velocidadePerambular;
        rb2D = GetComponent<Rigidbody2D>();
        StartCoroutine(rotinaPerambular());
        circleCollider = GetComponent<CircleCollider2D>();

    }

    private void OnDrawGizmos()                 // metodo que demonstra o raio de perambular
    {
        if (circleCollider != null)
        {
            Gizmos.DrawWireSphere(transform.position, circleCollider.radius);
        }
    }
    public IEnumerator rotinaPerambular()       // courotine da rotina de perambular
    {
        while (true)
        {
            EscolheNovoPontoFinal();
            if(moverCoroutine != null)
            {
                StopCoroutine(moverCoroutine);
            }
            moverCoroutine = StartCoroutine(Mover(rb2D, velocidadeCorrente));
            yield return new WaitForSeconds(invervaloMudancaDirecao);
        }
    }

    void EscolheNovoPontoFinal()            // calcula um novo ponto final para perambular
    {
        anguloAtual += Random.Range(0, 360);
        anguloAtual = Mathf.Repeat(anguloAtual, 360);
        posicaoFinal += Vector3ParaAngulo(anguloAtual);
    }
    Vector3 Vector3ParaAngulo (float anguloEntradaGraus)            //calcula um novo vetor3 para perambular
    {
        float anguloEntradaRadianos = anguloEntradaGraus * Mathf.Deg2Rad;
        return new Vector3(Mathf.Cos(anguloEntradaRadianos), Mathf.Sin(anguloEntradaRadianos), 0);
    }

    public IEnumerator Mover(Rigidbody2D rbParaMover, float velocidade)      // metodo que movimenta até o ponto final aonde vai perambular
    {
        float distanciaFaltante = (transform.position - posicaoFinal).sqrMagnitude;
        while (distanciaFaltante > float.Epsilon)
        {
            if( alvoTransform != null)
            {
                posicaoFinal = alvoTransform.position;
            }
            if( rbParaMover != null)
            {
                animator.SetBool("Caminhando", true);
                Vector3 novaPosicao = Vector3.MoveTowards(rbParaMover.position, posicaoFinal, velocidade * Time.deltaTime);
                rb2D.MovePosition(novaPosicao);
                distanciaFaltante = (transform.position - posicaoFinal).sqrMagnitude;
            }
            yield return new WaitForFixedUpdate();
        }
        animator.SetBool("Caminhando", false);
    }

    private void OnTriggerEnter2D(Collider2D collision)                 // metodo que verifica quando o raio atinge o player para iniciar perseguição
    {
        if (collision.gameObject.CompareTag("Player") && perseguePlayer)
        {
            velocidadeCorrente = velocidadePerseguicao;
            alvoTransform = collision.gameObject.transform;
            if (moverCoroutine != null)
            {
                StopCoroutine(moverCoroutine);
            }
            moverCoroutine = StartCoroutine(Mover(rb2D, velocidadeCorrente));
        }
    }

    private void OnTriggerExit2D(Collider2D collision)              // metodo que verifica quando o player sai do raio de perseguição para o inimigo voltar a perambualr
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animator.SetBool("Caminhando", false);
            velocidadeCorrente = velocidadePerambular;
            if(moverCoroutine != null)
            {
                StopCoroutine(moverCoroutine);
            }
            alvoTransform = null;
        }
    }

    void Update()
    {
        Debug.DrawLine(rb2D.position, posicaoFinal, Color.red);
    }
}
