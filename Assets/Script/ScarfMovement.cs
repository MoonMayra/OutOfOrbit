using UnityEngine;

public class ScarfMovement : MonoBehaviour
{
    [Header("Anchor")]
    public Transform target;                 // ScarfAnchor en el personaje
    public Vector2 anchorLocalOffset = new Vector2(0f, 0.1f);

    [Header("Geometry")]
    [Min(2)] public int segments = 10;       // Cantidad de segmentos
    [Min(1)] public int segmentLengthPixels = 8; // Largo de cada segmento en píxeles
    [Min(1)] public int pixelsPerUnit = 16;  // PPU de tu proyecto (16/32)
    public bool pixelSnap = true;

    [Header("Physics (Verlet)")]
    [Range(0f, 0.3f)] public float damping = 0.06f;   // amortiguación
    [Range(0f, 1f)] public float gravityScale = 0.15f;
    [Min(1)] public int constraintIterations = 6;
    public float maxTeleportSnap = 2f; // si el ancla se aleja de golpe, reinicia

    [Header("Wind / Oscillation (opcional)")]
    public float windStrength = 0f;            // 0 = sin viento
    public Vector2 windDirection = new Vector2(1f, 0f);
    public float windFrequency = 1.2f;         // Hz

    [Header("Rendering")]
    public Sprite segmentSprite;
    public Sprite tipSprite;
    public string sortingLayerName = "Default";
    public int orderInLayer = 0;

    // Internals
    private Vector2[] pos;
    private Vector2[] prev;
    private Transform[] segRenderers;
    private float segLenWorld => segmentLengthPixels / (float)pixelsPerUnit;
    private bool initialized;

    void OnEnable() { EnsureInit(); }
    void Start() { EnsureInit(); }

    void EnsureInit()
    {
        if (target == null || segmentSprite == null || segments < 2 || pixelsPerUnit <= 0)
            return;

        if (pos == null || pos.Length != segments)
        {
            pos = new Vector2[segments];
            prev = new Vector2[segments];
        }

        // Crear/actualizar renderers
        if (segRenderers == null || segRenderers.Length != segments)
        {
            // Limpia hijos viejos
            for (int i = transform.childCount - 1; i >= 0; i--)
                if (Application.isEditor) DestroyImmediate(transform.GetChild(i).gameObject);
                else Destroy(transform.GetChild(i).gameObject);

            segRenderers = new Transform[segments];

            for (int i = 0; i < segments; i++)
            {
                var go = new GameObject(i == segments - 1 ? "Tip" : $"Seg{i}");
                go.transform.SetParent(transform, false);
                var sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = (i == segments - 1 && tipSprite != null) ? tipSprite : segmentSprite;
                sr.sortingLayerName = sortingLayerName;
                sr.sortingOrder = orderInLayer + i; // o fijo, como prefieras
                segRenderers[i] = go.transform;
            }
        }

        // Inicializar posiciones en el ancla
        Vector2 a = AnchorWorld();
        for (int i = 0; i < segments; i++)
        {
            pos[i] = a - (Vector2.right * segLenWorld * i);
            prev[i] = pos[i];
        }

        initialized = true;
    }

    void FixedUpdate()
    {
        if (!initialized) { EnsureInit(); if (!initialized) return; }

        float dt = Time.fixedDeltaTime;

        // Teleport snap si el ancla saltó muy lejos
        if (Vector2.Distance(pos[0], AnchorWorld()) > maxTeleportSnap)
        {
            Vector2 a = AnchorWorld();
            for (int i = 0; i < segments; i++)
            {
                pos[i] = a - (Vector2.right * segLenWorld * i);
                prev[i] = pos[i];
            }
        }

        // 1) Integración Verlet
        Vector2 gravity = Physics2D.gravity * gravityScale * dt * dt;
        Vector2 wind = Vector2.zero;
        if (windStrength > 0f)
        {
            float t = Time.time;
            Vector2 dir = windDirection.normalized;
            float osc = Mathf.Sin(t * Mathf.PI * 2f * windFrequency);
            wind = dir * windStrength * osc * dt * dt;
        }

        for (int i = 1; i < segments; i++) // i=0 es el anclado
        {
            Vector2 current = pos[i];
            Vector2 velocity = (pos[i] - prev[i]) * (1f - damping);
            prev[i] = current;
            pos[i] = current + velocity + gravity + wind;
        }

        // 2) Anclar el primer punto al target
        pos[0] = AnchorWorld();

        // 3) Constraints de distancia
        for (int k = 0; k < constraintIterations; k++)
        {
            // Mantener el primer punto fijo
            pos[0] = AnchorWorld();

            for (int i = 0; i < segments - 1; i++)
            {
                Vector2 p1 = pos[i];
                Vector2 p2 = pos[i + 1];
                Vector2 delta = p2 - p1;
                float d = delta.magnitude;
                if (d == 0f) { delta = Vector2.right * segLenWorld; d = segLenWorld; }

                float diff = (d - segLenWorld) / d;
                // Repartimos la corrección (p1 fijo si i==0)
                if (i == 0)
                {
                    // mover solo p2
                    p2 -= delta * diff;
                }
                else
                {
                    p1 += delta * diff * 0.5f;
                    p2 -= delta * diff * 0.5f;
                }
                pos[i] = p1;
                pos[i + 1] = p2;
            }
        }

        // 4) Pixel snap (opcional) para evitar subpíxel
        if (pixelSnap)
        {
            for (int i = 0; i < segments; i++)
                pos[i] = SnapToPixel(pos[i]);
        }
    }

    void LateUpdate()
    {
        if (!initialized) return;

        // Render: posicionar y rotar cada sprite hacia el siguiente
        for (int i = 0; i < segments; i++)
        {
            Vector2 p = pos[i];
            segRenderers[i].position = new Vector3(p.x, p.y, transform.position.z);

            // Rotación hacia el siguiente (excepto el último, que mira al anterior)
            Vector2 dir = Vector2.right;
            if (i < segments - 1) dir = (pos[i + 1] - pos[i]).normalized;
            else dir = (pos[i] - pos[i - 1]).normalized;

            float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            segRenderers[i].rotation = Quaternion.Euler(0f, 0f, ang);
        }
    }

    Vector2 AnchorWorld()
    {
        return target != null ? (Vector2)target.TransformPoint(anchorLocalOffset) : (Vector2)transform.position;
    }

    Vector2 SnapToPixel(Vector2 worldPos)
    {
        float ppu = pixelsPerUnit;
        worldPos.x = Mathf.Round(worldPos.x * ppu) / ppu;
        worldPos.y = Mathf.Round(worldPos.y * ppu) / ppu;
        return worldPos;
    }
}
