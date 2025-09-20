using UnityEngine;

[RequireComponent(typeof(LineRenderer))]

public class TraectoryRender : MonoBehaviour
{

    [SerializeField] private int _pointCount = 30;

    [SerializeField] private float _lineWidth = 0.2f;
    [SerializeField] private float _lineLength = 0.2f;

    [SerializeField] private float _timeStep = 0.1f;


    private LineRenderer _lineRenderer;

    private void Awake() => InitialiseLineRenderer();


    private void InitialiseLineRenderer()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.startWidth = _lineWidth;
        _lineRenderer.useWorldSpace = true;

        _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        _lineRenderer.material.color = Color.red;
        
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
    
    public void DrawReal(Vector3 startPosition, float _airDencity, Rigidbody _rigidbody,
        Vector3 _wind, float _dragCoefficient, float _area,  Vector3 startVelocity)
    {
        if (_pointCount < 2) _pointCount = 2;
        _lineRenderer.positionCount = _pointCount;
        

        for (int i = 0; i < _pointCount; i++)
        {
            float t = i * _timeStep;
            
            Vector3 vReal = startVelocity - _wind;
            float speed = vReal.magnitude;

            Vector3 drag = -0.5f * _airDencity * _area* _dragCoefficient * speed*  Vector3.one;
            Debug.Log(drag);
            
            Vector3 newPosition = startPosition + t*startVelocity + 0.5f*t*t*Physics.gravity + drag;

            _lineRenderer.SetPosition(i, newPosition);
        }
        
    }
    

}
