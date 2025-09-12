using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Movimentação do jogador  
    [SerializeField] float speed = 1;
    public Rigidbody2D rb;
    public Collider2D playerCollider;
    public Vector2 mov;
    public Animator anim;

    // Vida do jogador
    [SerializeField] Image lifeBar;
    [SerializeField] Image redLifeBar;
    [SerializeField] int maxLife;
    [SerializeField] int currentLife;
    public bool isDead;

    // Knockback do jogador ao receber dano
    public KnockbackComponent knockbackComponent;
    public float kBForce;
    public float kBCount;
    public float kBTime;

    //ATAQUE DO JOGADOR
    [SerializeField] private float attackRange = 1f; // alcance do ataque
    [SerializeField] private int attackDamage = 1;  // dano do ataque
    [SerializeField] private LayerMask enemyLayer;   // layer dos inimigos


    // Sistema de diálogo:
    /*[SerializeField] Transform npcRobot;
    dialogueSystem dialogueSystem;
    public dialogueUI dialogueUI;*/

    //public bool isKnockRight;

    // Ataque do jogador
    //private bool isAttack = false;

    /*void Awake()
    {
        dialogueSystem = FindFirstObjectByType<dialogueSystem>();
    }*/

    void Start() { 
        knockbackComponent = GetComponent<KnockbackComponent>();
        currentLife = maxLife;

    }

    void Update()
    {
        MoveLogic();
        //OnAttack();

        // Teste da barra de vida para perder vida
        if (Input.GetKeyDown(KeyCode.J)) {
            TakeDamage(10, new Vector2(0, 0));
        };

        // Teste da barra de vida para ganhar vida
        if (Input.GetKeyDown(KeyCode.H)) {
            Heal(10);
        };
        Atacar();
        //EnteringDialogue();

    }

    private void FixedUpdate()
    {
        if (knockbackComponent.isKnockbackActive)
        {
            knockbackComponent.ApplyKnockback();
        }
        else
        {
            rb.linearVelocity = mov.normalized * speed;
        }
        

        // if (mov == Vector2.zero) {
        //     rb.linearVelocity = Vector2.zero;
        // }
            
    }

    /*void KnockLogic()
    {
        if (kBCount < 0){
            MoveLogic();
        } else{
            if (isKnockRight == true){
                rb.linearVelocity = new Vector2(-kBForce, -kBForce);
            } 
            if (isKnockRight == false){
                rb.linearVelocity = new Vector2(kBForce, kBForce);
            }
        }
        kBCount -= Time.deltaTime;
    }*/

    public void MoveLogic() {
        mov.x = Input.GetAxis("Horizontal");
        mov.y = Input.GetAxis("Vertical");

        anim.SetFloat("Horizontal", mov.x);
        anim.SetFloat("Vertical", mov.y);
        anim.SetFloat("Speed", mov.sqrMagnitude);
    }

    public void TakeDamage(int amount, Vector2 knockbackDirection)
    {
        SetLife(-amount);
        anim.SetTrigger("TakeDamage");
        knockbackComponent.Knockbacked();
        knockbackComponent.knockbackDirection = knockbackDirection;
        //kBCount = kBTime;
    }

    public void Heal(int amount)
    {
        SetLife(amount);
    }

    public void SetLife(int amount) { 

        currentLife = Mathf.Clamp(currentLife + amount, 0, maxLife);

        Vector3 lifeBarScale = lifeBar.transform.localScale;
        lifeBarScale.x = (float)currentLife / maxLife;
        lifeBar.rectTransform.localScale = lifeBarScale;
        StartCoroutine(DecreasingRedLifeBar(lifeBarScale));

        DeadState();

    }

    private void Atacar() {
        if (Input.GetKeyDown(KeyCode.Z) ){
            anim.SetTrigger("Attack");
            // Detecta inimigos no alcance
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, enemyLayer);

            foreach (Collider2D enemy in hitEnemies)
            {
                enemy.GetComponent<EnemyShortDistance>()?.TakeDamageEnemy(attackDamage);
            }
        }
        ;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


    /*void OnAttack() {

        EnemyShortDistance enemy = gameObject.GetComponent<EnemyShortDistance>();
        
        if (Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.LeftControl)) {  
            isAttack = true;
            anim.SetBool("isAttack", true);
            enemy.TakeDamageEnemy(10);

        }
        else
        {
            // Se o botão não estiver mais pressionado, define o parâmetro para false
            anim.SetBool("isAttack", false);
        }
        //EndAttack();

    }

    public void EndAttack()
    {
        isAttack = false;
        anim.SetBool("isAttack", false);
    }*/

    IEnumerator DecreasingRedLifeBar(Vector3 Scale)
    {
        yield return new WaitForSeconds(0.5f);
        Vector3 redLifeBarScale = redLifeBar.transform.localScale;


        while (redLifeBar.transform.localScale.x > Scale.x) { 
            redLifeBarScale.x -= Time.deltaTime * 0.25f;
            redLifeBar.transform.localScale = redLifeBarScale;

            yield return null;
        }

        redLifeBar.transform.localScale = Scale;
    }

    private void DeadState() {

        if (currentLife <= 0) { 

            isDead = true;
            anim.SetBool("IsDead", isDead);
            //enabled = false;

            // --- NOVA LÓGICA ---
            // Desativa a simulação do Rigidbody
            if (rb != null)
            {
                // rb.simulated = false; é a forma mais limpa de desativar a física.
                rb.linearVelocity = Vector2.zero;
                rb.angularVelocity = 0;
                rb.constraints = RigidbodyConstraints2D.FreezeAll;
            }

            // Desativa o Collider
            if (playerCollider != null)
            {
                playerCollider.enabled = false;
            }

        }
    }

    /*private void EnteringDialogue() { 
        if(Mathf.Abs(transform.position.x - npcRobot.position.x) < 2.0f)
        {
            if (Input.GetKeyDown(KeyCode.E))
                dialogueUI.Enable();
                dialogueSystem.StartDialogue();
        }
    }*/
};