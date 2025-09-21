using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class QuadricDrag : MonoBehaviour
{

    
    private float _radius;
    private float _dragCoefficient;
    private float _airDencity;
    private Vector3 _wind= Vector3.zero;
    private Rigidbody _rigidbody;
    private float _area;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        
        
    }



    void FixedUpdate()
    {
        Vector3 vRel = _rigidbody.linearVelocity - _wind;
        float speed = vRel.magnitude;
        if (speed < 1e-6f) return;

        Vector3 drag = -0.5f * _airDencity * _dragCoefficient * _area * speed * vRel;
        _rigidbody.AddForce(drag, ForceMode.Force);
    }


    public void SetPhysicalParams(float mass, float radius, float dragCoefficient,
                                    float airDencity, Vector3 wind, Vector3 initialVelocity)
    {
        _rigidbody.mass = mass;
        _rigidbody.useGravity = true;
        _rigidbody.linearDamping = 0f;
        _rigidbody.linearVelocity = initialVelocity;


        _radius = radius;
        _dragCoefficient = dragCoefficient;
        _airDencity = airDencity;
        _wind = wind;

        _area = _radius * _radius * Mathf.PI;

    }


}
