using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class TraectoryRender : MonoBehaviour
{

    [SerializeField] private int _pointCount = 30;

    [SerializeField, Range(0.05f, 1f)] private float _lineWidth = 0.2f;

    [SerializeField] private float _timeStep = 0.1f;

    [SerializeField] private Color lineColor=Color.red;


    private LineRenderer _lineRenderer;

    private void Awake() => InitialiseLineRenderer();


    private void InitialiseLineRenderer()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.startWidth = _lineWidth;
        _lineRenderer.useWorldSpace = true;

        _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineColor.a = 0.5f;
        _lineRenderer.material.color = lineColor;

    }

    public void DrawVacuum(Vector3 startPosition, Vector3 startVelocity)
    {
        if (_pointCount < 2) _pointCount = 2;
        _lineRenderer.positionCount = _pointCount;
        

        for (int i = 0; i < _pointCount; i++)
        {
            float t = i * _timeStep;
            Vector3 newPosition = startPosition + t*startVelocity + 0.5f*t*t*Physics.gravity;

            _lineRenderer.SetPosition(i, newPosition);
        }
        
    }
    
    public void DrawWithAir(Vector3 startPosition, float _airDencity,
        Vector3 _wind, float _dragCoefficient, float radius,  Vector3 startVelocity, float mass)
    {
        Vector3 p = startPosition;
        Vector3 v = startVelocity;
        _lineRenderer.positionCount = _pointCount;
        float _area = radius * radius * Mathf.PI;

        for (int i = 0; i < _pointCount; i++)
        {
            _lineRenderer.SetPosition(i, p);

            // ускорение: g + Fd/m, Fd = -0.5*rho*Cd*A*|v_rel|*v_rel
            Vector3 vReal = v -= _wind;
            float speed = vReal.magnitude;
            Vector3 drag = -0.5f * _airDencity * _dragCoefficient * _area * speed *vReal;
            Vector3 a = Physics.gravity + drag / mass;

            v += a * _timeStep;  // шаг по скорости
            p += v * _timeStep;  // шаг по позиции


        }
    }
    
    public void DrawReal(Vector3 startPosition, float _airDencity,
        Vector3 _wind, float _dragCoefficient, float radius,  Vector3 startVelocity, float mass)
    {
        if (_pointCount < 2) _pointCount = 2;
        _lineRenderer.positionCount = _pointCount;
        
        Vector3 drag = Vector3.zero;
        Vector3 vReal = startVelocity;
        float _area = radius * radius * Mathf.PI;
        Vector3 newPosition = startPosition;

        for (int i = 0; i < _pointCount; i++)
        {
            _lineRenderer.SetPosition(i, newPosition);
            
            float t = i * _timeStep;

            vReal -= (_wind / mass / _pointCount);
            float speed = vReal.magnitude;


            drag = -0.5f * _airDencity * _dragCoefficient * vReal * _area * speed;
            //Debug.Log("A"+_area);
           // Debug.Log("D1"+drag);

  
          //  Debug.Log("D2"+drag);


            newPosition = startPosition + t * startVelocity +
                                  t * t / 2 * Physics.gravity  + t * t*drag/2;

            
        }
        
        
        
    }
    

}
