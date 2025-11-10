using Unity.VisualScripting;
using UnityEngine;

public class HandcarSensors : MonoBehaviour
{
    private Handcar handcar;
    public enum Side { Left, Right }
    public Side sensorSide;

    private void Awake()
    {
        handcar = GetComponentInParent<Handcar>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Voids"))
        {
            if (sensorSide == Side.Left)
            {
                handcar.UpdateHandcar(Vector2.left);
            }
            else if (sensorSide == Side.Right)
            {
                handcar.UpdateHandcar(Vector2.right);
            }
        }
    }
}