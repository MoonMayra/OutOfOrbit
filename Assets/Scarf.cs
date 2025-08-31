using UnityEngine;

public class Scarf : MonoBehaviour
{
    [Header("Configuración general")]
    public Transform target;           // Punto de ancla (ScarfAnchor)
    public int segments = 10;          // Cantidad de nodos
    public float segmentLength = 0.1f; // Largo de cada segmento
    public int iterations = 6;         // Correcciones de distancia
    public float damping = 0.05f;      // Amortiguación de movimiento
    public float gravity = 0.1f;       // Caída de la bufanda

    [Header("Viento")]
    public Vector2 wind = new Vector2(0.05f, 0f); // Fuerza y dirección del viento

    private Vector3[] pos;            // Posición actual de cada nodo
    private Vector3[] oldPos;         // Posición anterior para inercia
    private LineRenderer lr;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = segments;

        pos = new Vector3[segments];
        oldPos = new Vector3[segments];

        Vector3 start = target.position;
        for (int i = 0; i < segments; i++)
        {
            pos[i] = start - Vector3.right * segmentLength * i;
            oldPos[i] = pos[i];
        }
    }

    void FixedUpdate()
    {
        // 1) Verlet Integration (inercia + gravedad)
        for (int i = 1; i < segments; i++)
        {
            Vector3 velocity = pos[i] - oldPos[i];
            oldPos[i] = pos[i];
            pos[i] += velocity * (1f - damping);
            pos[i] += Vector3.down * gravity * Time.fixedDeltaTime;

            // 1b) Aplicar viento
            pos[i] += (Vector3)wind * Time.fixedDeltaTime;
        }

        // 2) Primer nodo anclado al target
        pos[0] = target.position;

        // 3) Constraints: mantener la distancia entre nodos
        for (int k = 0; k < iterations; k++)
        {
            pos[0] = target.position;
            for (int i = 0; i < segments - 1; i++)
            {
                Vector3 delta = pos[i + 1] - pos[i];
                float dist = delta.magnitude;
                float error = dist - segmentLength;
                Vector3 change = delta.normalized * error * 0.5f;
                if (i != 0) pos[i] += change;
                pos[i + 1] -= change;
            }
        }

        // 4) Forzar Z = 0 para 2D
        for (int i = 0; i < segments; i++)
        {
            pos[i].z = 0f;
        }

        // 5) Dibujar la línea
        lr.positionCount = segments;
        lr.SetPositions(pos);
    }
}