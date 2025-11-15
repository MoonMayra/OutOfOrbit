using UnityEngine;

public class GorillaMovement : MonoBehaviour
{
    [SerializeField] private float minY;
    [SerializeField] private float maxY;
    private Rigidbody2D playerRigidBody;
    private Rigidbody2D gorillaRigidBody;
    private float prevY;

    private void Start()
    {
        playerRigidBody= transform.parent.GetComponentInParent<Rigidbody2D>();
        gorillaRigidBody= GetComponent<Rigidbody2D>();
        prevY = playerRigidBody.position.y;
    }
    private float CalculateDeltaPlayer()
    {
        float deltaY = playerRigidBody.position.y - prevY;
        prevY = playerRigidBody.position.y;
        return -deltaY;
    }
    private void Update()
    {
        if (playerRigidBody== null)
        return;
        Vector2 gorillaPos= transform.position;
        float gorillaNewY= gorillaPos.y + CalculateDeltaPlayer();
        gorillaNewY= Mathf.Clamp(gorillaNewY, minY, maxY);
        gorillaRigidBody.transform.position=(new Vector2(gorillaPos.x, gorillaNewY ));

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector2(-10, minY), new Vector2(10, minY));
        Gizmos.DrawLine(new Vector2(-10, maxY), new Vector2(10, maxY));
    }
}

