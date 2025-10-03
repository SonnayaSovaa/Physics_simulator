using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


[RequireComponent(typeof(TraectoryRender))]


public class BallisticCalculator : MonoBehaviour
{
    private ActionMaps _control;

    [SerializeField] private Transform _launchPoint;
    [SerializeField] private float _muzzleVelocity=20f;

    [SerializeField, Range(5, 85)] private float _muzzleAngle=20f;

    private TraectoryRender _traectoryRender;
    
    [SerializeField] private GameObject _shootRound;
    
    [SerializeField, Range(0.1f, 0.5f)] private float minRadius;
    [SerializeField, Range(0.1f, 0.5f)] private float maxRadius;
    
    [SerializeField, Range(0.1f, 1f)] private float minMass;
    [SerializeField, Range(0.1f, 1f)] private float maxMass;

    private QuadricDrag quadricDrag;

    [SerializeField] private Statistic statistic;

    
    private float _mass ;
    private float _radius;
    [Header("--Air parameters--")]
    [SerializeField] private float _dragCoefficient = 0.47f;
    [SerializeField] private float _airDencity=1.255f;
    [SerializeField] private Vector3 _wind= Vector3.zero;
    private Vector3 v0;
    

    private bool _instantiated = false;
   

    [SerializeField] private ParticleSystem boom;

    private void Awake()
    {
        ShootInstance();
        
        _control = new ActionMaps();
        _control.Cannon.Fire.started += ctx => Fire(v0);
        _control.Cannon.Inst.started += ctx => ShootInstance();

        
        _traectoryRender = GetComponent<TraectoryRender>();

        ShootInstance();
    }

    public void SetVelocity(Slider slider)
    {
        _muzzleVelocity = slider.value;
    }

    

    void Update()
    {
        /*

        _muzzleAngle = Vector3.Angle(transform.forward,
            Vector3.ProjectOnPlane(transform.forward, Vector3.up));
*/
        v0 = CalculateVelocity(_muzzleAngle);
        //_traectoryRender.DrawVacuum(_launchPoint.position, v0);
        
        
        
        _traectoryRender.DrawWithAir(_launchPoint.position, _airDencity, _wind, _dragCoefficient, _radius , v0, _mass);
           
       //  _traectoryRender.DrawNewAir(_launchPoint, _airDencity, _wind, _dragCoefficient, _radius , v0, _mass);
        

    }

    void ShootInstance()
    {
        if (_instantiated) return;
        GameObject newShootRound = Instantiate(_shootRound, _launchPoint.position, _launchPoint.rotation);

        newShootRound.transform.parent = _launchPoint;
        
        
        quadricDrag = newShootRound.GetComponent<QuadricDrag>();
        quadricDrag.SetStats(statistic);
        
        _radius = Random.Range(minRadius, maxRadius);
        _mass = Random.Range(minMass, maxMass);
        
        _instantiated = true;
    }
    


    void Fire(Vector3 initialVelocity)
    {
        

        if (!_instantiated) return;
        quadricDrag.transform.parent = null;
        
        quadricDrag.SetPhysicalParams(_mass, _radius, _dragCoefficient, _airDencity, _wind, initialVelocity);
        //Debug.Log(_rigidbody.mass);
        boom.Play();

        _instantiated = false;
        
        boom.Play();

        quadricDrag.Delete();
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
