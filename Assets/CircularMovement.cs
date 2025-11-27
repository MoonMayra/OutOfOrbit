using UnityEngine;

public class CircularMovement : MonoBehaviour
{
    [SerializeField] private Transform center;
    [SerializeField] private float radius = 5f;
    [SerializeField] private float angularSpeed = 30f;

    private Rigidbody2D rigidBody;
    private float angle;
    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if(center==null) return;
        angle += angularSpeed * Time.fixedDeltaTime;

        Vector2 centerPosition = center.position;
        Vector2 newPosition= new Vector2(Mathf.Cos(angle),Mathf.Sin(angle))*radius + centerPosition;
        rigidBody.MovePosition(newPosition);
    }
    private void OnDrawGizmosSelected()
    {
        if (center == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(center.position, radius);
    }
}
