using UnityEngine;

public class PruebaScript : MonoBehaviour
{
    [SerializeField] private float xSpeed = 5f;
    [SerializeField] private float ySpeed = 5f;
    private void Update()
    {
      
        transform.position += new Vector3(xSpeed * Time.deltaTime, 0,0);
        transform.position += new Vector3(0, ySpeed * Time.deltaTime, 0);

    }

}
