using UnityEngine;

public class MovementPlat : MonoBehaviour
{
    [SerializeField] float speed = 2f;
    [SerializeField] float moveDistance = 5f;

    private Vector3 startPos;
    private Rigidbody2D playerRb;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        // Calcula el nuevo movimiento de la plataforma
        float newX = transform.position.x + speed * Time.deltaTime;
        transform.position = new Vector3(newX, transform.position.y, transform.position.z);

        // Cambia dirección si llega a los límites
        if (transform.position.x >= startPos.x + moveDistance || transform.position.x <= startPos.x - moveDistance)
        {
            speed *= -1;
        }

        // Mueve al jugador si está sobre la plataforma
        if (playerRb != null)
        {
            playerRb.MovePosition(playerRb.position + new Vector2(speed * 1.5f * Time.deltaTime, 0f));
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerRb = null;
        }
    }
}
