using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class FlightStateLight : MonoBehaviour
{
    [SerializeField] private Transform wingChord;
    public float IAS { get; private set; }
    public float AoAdeg { get; private set; }
    public float Nz { get; private set; }

    private Rigidbody _rigidbody;
    private Vector3 _vPrev;
    private float _tPrev;
    private Vector3 g = Physics.gravity;
    
    private const float MinValueForAngleAttack = 1e-3f;
    
    private void Awake() => Initialize();

    private void Initialize()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _vPrev = _rigidbody.linearVelocity;
        _tPrev = Time.time;

    }

    void FixedUpdate()
    {
        Vector3 currentVelocity = _rigidbody.linearVelocity;
        IAS = currentVelocity.magnitude;

        if (IAS > MinValueForAngleAttack)
        {
            Vector3 flow = (-currentVelocity).normalized;
            float flowX = Vector3.Dot(flow, wingChord.forward);
            float flowZ = Vector3.Dot(flow, wingChord.up);

            AoAdeg = Mathf.Deg2Rad * Mathf.Atan2(flowZ, flowX);

        }
        else
        {
            AoAdeg = 0;
        }

        float currentTime = Time.time;
        float dt = Mathf.Max(MinValueForAngleAttack, currentTime - _tPrev);
        Vector3 aWorld = (currentVelocity - _vPrev) / dt;
        float aVert = Vector3.Dot(aWorld + g, transform.up);

        Nz = 1f + (aVert / Mathf.Abs(g.y));
        _vPrev = currentVelocity;
        _tPrev = currentTime;

    }




}
