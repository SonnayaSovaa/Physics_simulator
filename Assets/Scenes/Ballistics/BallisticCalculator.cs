using UnityEngine;

[RequireComponent(typeof(TraectoryRender))]


public class BallisticCalculator : MonoBehaviour
{
    
    [SerializeField] private Transform _launchPoint;
    [SerializeField] private float _muzzleVelocity=20f;

    [SerializeField, Range(5, 85)] private float _muzzleAngle=20f;

    private TraectoryRender _traectoryRender;

    void Start()
    {
        _traectoryRender = GetComponent<TraectoryRender>();
    }

    void Update()
    {
        if (_launchPoint==null) return;

        Vector3 v0 = CalculateVelocity(_muzzleAngle);
        _traectoryRender.DrawVacuum(_launchPoint.position, v0);

    }

    Vector3 CalculateVelocity(float angle)
    {
        float vx = _muzzleVelocity*Mathf.Cos(angle*Mathf.Deg2Rad);
        float vy = _muzzleVelocity*Mathf.Sin(angle*Mathf.Deg2Rad);

        return _launchPoint.forward*vx + _launchPoint.up*vy;


    }

}
