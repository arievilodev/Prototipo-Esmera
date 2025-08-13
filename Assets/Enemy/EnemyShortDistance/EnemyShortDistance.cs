using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShortDistance : MonoBehaviour
{
    private Transform posPlayer; // Vari�vel para armazenar a posi��o do jogador
    public float speedEnemy;
    [SerializeField] private Rigidbody2D rbEnemy;
    private bool playerDetected = false;
    private Vector2 initialPositionEnemy;
    public Animator anim;

    // Sistema de vida do inimigo
    [SerializeField] private int maxLife = 30;
    [SerializeField] private int currentLife;
    [SerializeField] private bool isDead = false;

    


    void Start() { 
    
        posPlayer = GameObject.FindGameObjectWithTag("Player").transform; // Encontra o jogador na cena e armazena sua posi��o
        rbEnemy = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        initialPositionEnemy = rbEnemy.position;
        currentLife = maxLife; // Inicializa a vida do inimigo com o valor m�ximo
    }

    void Update()
    {
        if (playerDetected)
        {
            FollowPlayer();
        }

        // teste de dano do inimigo
        if (Input.GetKeyDown(KeyCode.L))
        {
            TakeDamageEnemy(10);
        }
    }

    void FixedUpdate()
    {
        if (playerDetected)
        {
            Vector2 direction = (posPlayer.position - transform.position).normalized;
            rbEnemy.MovePosition(rbEnemy.position + direction * speedEnemy * Time.fixedDeltaTime);
        }

    }

    private void FollowPlayer() {

        
        if (posPlayer.gameObject != null) {
            transform.position = Vector2.MoveTowards(transform.position, posPlayer.position, speedEnemy * Time.deltaTime);
        }
    
    }

    public void TakeDamageEnemy(int amount)
    {
        anim.SetTrigger("hit");
        if (isDead) return;
        currentLife -= amount;
        
        if (currentLife <= 0)
        {
            DieEnemy();
        }
    }

    private void DieEnemy()
    {
        isDead = true;
        // Exemplo: desativa o inimigo
        gameObject.SetActive(false);
        // Ou: anim.SetTrigger("Dead"); se tiver anima��o
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!playerDetected && other.CompareTag("Player"))
        {
            playerDetected = true;
        }
    }


    // Inicializando a colis�o do inimigo
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se o objeto colidido � o jogador a partir da tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Obt�m o componente Player, guarda os resultados no objeto player
            Player player = collision.gameObject.GetComponent<Player>();
            // Se o componente PlayerMov n�o for nulo, ou seja, se tiver sido encontrado, ent�o o m�todo TakeDamage � chamado,
            // tirando 10 pontos de vida do jogador */
            if (player != null)
            {
                player.TakeDamage(10);

                if (player.isDead) {
                    playerDetected = false;
                    StartCoroutine(ReturnToStart());
                }

            }

        }
    }

    private IEnumerator ReturnToStart()
    {
        // Enquanto n�o chegou ao ponto inicial, mova o inimigo para l�
        while ((Vector2)transform.position != initialPositionEnemy)
        {
            Vector2 direction = (initialPositionEnemy - (Vector2)transform.position).normalized;
            rbEnemy.MovePosition(rbEnemy.position + direction * speedEnemy * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
        }
        // Garante que a posi��o final seja exatamente a inicial
        rbEnemy.MovePosition(initialPositionEnemy);
    }
}
