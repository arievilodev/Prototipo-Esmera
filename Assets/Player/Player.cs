using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Movimenta��o do jogador  
    [SerializeField] float speed = 1;
    public Rigidbody2D rb;
    public Vector2 mov;
    public Animator anim;

    // Vida do jogador
    [SerializeField] Image lifeBar;
    [SerializeField] Image redLifeBar;
    [SerializeField] int maxLife;
    [SerializeField] int currentLife;
    [SerializeField] bool isDead;

    // Knockback do jogador ao receber dano
    public float kBForce;
    public float kBCount;
    public float kBTime;

    public bool isKnockRight;

    void Start() { 
        
        currentLife = maxLife;

    }

    void Update()
    {
        KnockLogic();

        // Teste da barra de vida para perder vida
        if (Input.GetKeyDown(KeyCode.J)) {
            TakeDamage(10);
        };

        // Teste da barra de vida para ganhar vida
        if (Input.GetKeyDown(KeyCode.H)) {
            Heal(10);
        };

    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + mov.normalized * speed * Time.fixedDeltaTime);
    }

    void KnockLogic()
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
    }

    public void MoveLogic() {
        mov.x = Input.GetAxis("Horizontal");
        mov.y = Input.GetAxis("Vertical");

        anim.SetFloat("Horizontal", mov.x);
        anim.SetFloat("Vertical", mov.y);
        anim.SetFloat("Speed", mov.sqrMagnitude);
    }

    public void TakeDamage(int amount)
    {
        SetLife(-amount);
        anim.SetTrigger("TakeDamage");
        kBCount = kBTime;
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
            enabled = false;
            
        }
    }
};