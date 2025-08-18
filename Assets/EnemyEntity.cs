using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyEntity : MonoBehaviour
{
    NavMeshAgent agent;
    [SerializeField] private Rigidbody2D rbEnemy;
    public Animator anim;
    private Player player;
    private Transform posPlayer; // Vari�vel para armazenar a posi��o do jogador
    private Vector2 initialPositionEnemy;

    [Header("Enemy Settings")]
    public float speedEnemy = 2f;
    public int damage = 10;
    public float stoppingDistance = 0.2f;


    // Sistema de vida do inimigo
    [Header("Health Settings")]
    [SerializeField] private int maxLife = 30;
    [SerializeField] private int currentLife;
    // [SerializeField] private Transform target;

    [Header("States")]
    private bool playerDetected = false;
    [SerializeField] private bool isDead = false;
    public bool playerDead = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerDetection();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!playerDetected && other.CompareTag("Player"))
        {
            playerDetected = true;
        }
    }

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
                var knockbackDirection = (player.transform.position - transform.position).normalized;
                player.TakeDamage(damage, knockbackDirection);

                if (player.isDead)
                {
                    playerDead = true;
                    playerDetected = false;
                    StartCoroutine(ReturnToStart());
                }

            }

        }
    }

    void PlayerDetection()
    {
        playerDead = player.isDead;
        if (playerDetected & !playerDead)
        {
            FollowPlayer();
        }
        else if (playerDead)
        {
            playerDead = true;
            playerDetected = false;
            StartCoroutine(ReturnToStart());
        }
        // teste de dano do inimigo
        if (Input.GetKeyDown(KeyCode.L))
        {
            TakeDamageEnemy(10);
        }
    }

    void Initialize()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; // Desativa a rota����o autom��tica do NavMeshAgent
        agent.updateUpAxis = false;
        agent.stoppingDistance = stoppingDistance; // Define a dist��ncia de parada do inimigo

        agent.speed = speedEnemy; // Define a velocidade do inimigo

        posPlayer = GameObject.FindGameObjectWithTag("Player").transform; // Encontra o jogador na cena e armazena sua posi��o
        player = posPlayer.GetComponent<Player>();

        rbEnemy = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        initialPositionEnemy = rbEnemy.position;
        currentLife = maxLife; // Inicializa a vida do inimigo com o valor m�ximo
    }

    private void FollowPlayer()
    {
        if (posPlayer.gameObject != null && !playerDead)
        {
            agent.SetDestination(posPlayer.position); // Move o inimigo em dire����o ao jogador
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

     private IEnumerator ReturnToStart()
    {
        agent.SetDestination(initialPositionEnemy); // Move o inimigo de volta para a posi����o inicial
        yield return new WaitForFixedUpdate();

        // Enquanto n�o chegou ao ponto inicial, mova o inimigo para l�
        // while ((Vector2)transform.position != initialPositionEnemy)
        // {
        //     Vector2 direction = (initialPositionEnemy - (Vector2)transform.position).normalized;
        //     rbEnemy.MovePosition(rbEnemy.position + direction * speedEnemy * Time.fixedDeltaTime);
        //     yield return new WaitForFixedUpdate();
        // }
        // // Garante que a posi��o final seja exatamente a inicial
        // rbEnemy.MovePosition(initialPositionEnemy);
    }

}
