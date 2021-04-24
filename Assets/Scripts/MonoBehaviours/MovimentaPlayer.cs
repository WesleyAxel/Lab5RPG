using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimentaPlayer : MonoBehaviour
{
    public float VelocidadeMovimento = 3.0f;            //equivale ao momento a ser dado  ao player
    Vector2 Movimento = new Vector2();                  //detectar movimento pelo teclado


    Animator animator;                                  // guarda a componente de controlador do player
    Rigidbody2D rd2D;                                   // guarda a componete CorpoRigido ao player

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rd2D = GetComponent<Rigidbody2D>();             // pegou o componete rigidbody 2d
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEstado();
    }


    private void FixedUpdate()
    {
        MoveCaractere();
    }

    private void MoveCaractere()
    {
        Movimento.x = Input.GetAxisRaw("Horizontal");
        Movimento.y = Input.GetAxisRaw("Vertical");
        Movimento.Normalize();
        rd2D.velocity = Movimento * VelocidadeMovimento;  // fixou velocidade ao corpo rigido
    }

    void UpdateEstado()
    {
        if (Mathf.Approximately(Movimento.x, 0 ) && Mathf.Approximately(Movimento.y, 0)){
            animator.SetBool("Caminhando", false);
        }
        else
        {
            animator.SetBool("Caminhando", true);
        }
        animator.SetFloat("dirX", Movimento.x);
        animator.SetFloat("dirY", Movimento.y);
    }
}
