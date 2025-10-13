using UnityEngine;

public class CameraMov : MonoBehaviour
{
    [SerializeField] private Transform targetPos;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask player;
    [SerializeField] private float speed = 2f;

    private bool moveCamera = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((1<< collision.gameObject.layer & player) != 0)
        {
           
            moveCamera = true;
        }
    }

    private void Update()
    {
        if(moveCamera && targetPos != null)
        {
            mainCamera.transform.position = Vector2.Lerp(mainCamera.transform.position, new Vector2(targetPos.position.x, targetPos.position.y), speed * Time.deltaTime);
            if (Vector2.Distance(mainCamera.transform.position, new Vector2(targetPos.position.x, targetPos.position.y)) < 0.1f)
            {
                moveCamera = false;
            }
        }
    }
}
