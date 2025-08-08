using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShortDistance : MonoBehaviour
{
    // Inicializando a colisão do inimigo
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verifica se o objeto colidido é o jogador a partir da tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // Obtém o componente PlayerMov, guarda os resultados no objeto player
            Player player = collision.gameObject.GetComponent<Player>();
            // Se o componente PlayerMov não for nulo, ou seja, se tiver sido encontrado, então o método TakeDamage é chamado,
            // tirando 10 pontos de vida do jogador */
            if (player != null)
            {
                player.TakeDamage(10);
                player.kBCount = player.kBTime;
                if (collision.transform.position.x <= transform.position.x)
                {
                    player.isKnockRight = true;
                }
                if (collision.transform.position.x > transform.position.x)
                {
                    player.isKnockRight = false;
                }

            }


        }
    }
}
