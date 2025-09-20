using UnityEngine;
using System.Collections;
using NUnit.Framework.Constraints;


[RequireComponent(typeof(TraectoryRender))]


public class BallisticCalculator : MonoBehaviour
{
    private Cannon_control _control;

    [SerializeField] private Transform _launchPoint;
    [SerializeField] private float _muzzleVelocity=20f;

    [SerializeField, Range(5, 85)] private float _muzzleAngle=20f;

    private TraectoryRender _traectoryRender;
    
    [SerializeField] private GameObject _shootRound;
    
    [SerializeField, Range(0.1f, 1f)] private float minRadius;
    [SerializeField, Range(0.1f, 1.5f)] private float maxRadius;
    
    [SerializeField, Range(0.5f, 1f)] private float minMass;
    [SerializeField, Range(0.6f, 2f)] private float maxMass;

    private QuadricDrag quadricDrag;


    private float _mass ;
    private float _radius;
    private float _dragCoefficient = 0.47f;
    private float _airDencity=1.255f;
    private Vector3 _wind= Vector3.zero;
    private Vector3 v0;
    
    private Rigidbody _rigidbody;

   

    [SerializeField] private ParticleSystem boom;

    private void Awake()
    {
        ShootInstance();
        
        _control = new Cannon_control();
        _control.Cannon.Fire.started += ctx => Fire(v0);
        
        _traectoryRender = GetComponent<TraectoryRender>();


    }

    

    void Update()
    {
        if (_launchPoint == null) return;

        v0 = CalculateVelocity(_muzzleAngle);
        //_traectoryRender.DrawVacuum(_launchPoint.position, v0);
        
        

        _traectoryRender.DrawReal(_launchPoint.position, _airDencity, _rigidbody,
            _wind, _dragCoefficient, _radius * _radius * Mathf.PI, v0);

    }

    void ShootInstance()
    {
        //if (_shootRound == null) return;
        GameObject newShootRound = Instantiate(_shootRound, _launchPoint.position, Quaternion.identity);
        newShootRound.transform.parent = _launchPoint;
        _rigidbody = newShootRound.GetComponent<Rigidbody>();
        
        
        quadricDrag = newShootRound.GetComponent<QuadricDrag>();
        
        _radius = Random.Range(minRadius, maxRadius);
        _mass = Random.Range(minMass, maxMass);
    }
    

    void Fire(Vector3 initialVelocity)
    {

        _rigidbody.transform.parent = null;
        _rigidbody.isKinematic = false;
        quadricDrag.SetPhysicalParams(_mass, _radius, _dragCoefficient, _airDencity, _wind, initialVelocity);
        //Debug.Log(_rigidbody.mass);
        boom.Play();
        Destroy(quadricDrag.gameObject, 5f);
        
        ShootInstance();
    }

    Vector3 CalculateVelocity(float angle)
    {
        float vx = _muzzleVelocity*Mathf.Cos(angle*Mathf.Deg2Rad);
        float vy = _muzzleVelocity*Mathf.Sin(angle*Mathf.Deg2Rad);

        return _launchPoint.forward*vx + _launchPoint.up*vy;
        
    }
    
    private void OnEnable()
    {
        _control.Enable();
    }

    private void OnDisable()
    {
        _control.Disable();
    }

}
