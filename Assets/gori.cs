using UnityEngine;

public class Stun : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            Debug.Log("Coco golpeó al gorila");

            Gorilla gorilla = collision.collider.GetComponent<Gorilla>();
            if (gorilla != null)
            {
                gorilla.Stun();
            }
        }

        Destroy(gameObject);
    }
}
