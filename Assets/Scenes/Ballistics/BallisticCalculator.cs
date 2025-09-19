using UnityEngine;
using System.Collections;


[RequireComponent(typeof(TraectoryRender))]


public class BallisticCalculator : MonoBehaviour
{
    private Cannon_control _control;

    [SerializeField] private Transform _launchPoint;
    [SerializeField] private float _muzzleVelocity=20f;

    [SerializeField, Range(5, 85)] private float _muzzleAngle=20f;

    private TraectoryRender _traectoryRender;
    [SerializeField] private QuadricDrag _shootRound;


    private float _mass =1;
    private float _radius =0.1f;
    private float _dragCoefficient = 0.47f;
    private float _airDencity=1.255f;
    private Rigidbody _rigidbody;
    private Vector3 _wind= Vector3.zero;
    private Vector3 v0;

    [SerializeField] private ParticleSystem boom;

    private void Awake()
    {
        _control = new Cannon_control();
        _control.Cannon.Fire.started += ctx => Fire(v0);
        
        _traectoryRender = GetComponent<TraectoryRender>();

    }

    

    void Update()
    {
        if (_launchPoint == null) return;

        v0 = CalculateVelocity(_muzzleAngle);
        _traectoryRender.DrawVacuum(_launchPoint.position, v0);

    }

    void Fire(Vector3 initialVelocity)
    {
        if (_shootRound == null) return;
        GameObject newShootRound = Instantiate(_shootRound.gameObject, _launchPoint.position, Quaternion.identity);
        
        boom.Play();
        QuadricDrag quadricDrag = newShootRound.GetComponent<QuadricDrag>();
        quadricDrag.SetPhysicalParams(_mass, _radius, _dragCoefficient, _airDencity, _wind, initialVelocity);
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
