using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class QuadricSpeedController : MonoBehaviour
{

    [SerializeField] private float _mass = 1.5f;
    [SerializeField] private float _maxThrottle = 30f;
    [SerializeField] private float _maxTorque = 5f;

    [SerializeField] private float _maxPitch = 20f;
    [SerializeField] private float _maxRollDeg = 20f;

    [SerializeField] private float _yawDegPerSec = 100f;


    Rigidbody _rigidbody;

    private float _desiredYawDeg;

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.mass = Mathf.Max(0.01f, _mass);

        _desiredYawDeg = transform.eulerAngles.y;

    }


    void Update()
    {
        float yawInput = Mathf.Clamp(Input.GetAxis("Mouse X"), -1f, 1f);
        _desiredYawDeg = yawInput * Time.deltaTime;

      
    }

    void FixedUpdate()
    {
        float pitchInput = Mathf.Clamp(Input.GetAxis("Mouse Y"), -1f, 1f);

  
        float rollInput = Mathf.Clamp(Input.GetAxis("Mouse X"), -1f, 1f);

        float throttleInput = 0;

        if (Input.GetKey("space")) throttleInput = 1f;




        float targetPitch = pitchInput * _maxPitch;
        float targetRoll = rollInput * _maxRollDeg;

        float targetYaw = _desiredYawDeg;


        Quaternion qTarget = Quaternion.Euler(targetPitch, targetYaw, targetRoll);
        Quaternion qCurrent = _rigidbody.rotation;

        Quaternion qError = qTarget * Quaternion.Inverse(qCurrent);

        if (qError.w < 0)
        {
            qError.x *= -1;
            qError.y *= -1;
            qError.z *= -1;
            qError.w *= -1;
        }

        qError.ToAngleAxis(out float angleDeg, out Vector3 axis);

        float angleRrad = Mathf.Deg2Rad * angleDeg;

        Vector3 omega = _rigidbody.angularVelocity;
        Vector3 torque = 8 * angleRrad * axis - 2.5f * omega;


        _rigidbody.AddTorque(torque);

        float g = Physics.gravity.magnitude;
        float hover = g * _rigidbody.mass;

        float centerd = (throttleInput - 0.5f) * 2f;
        float commandForce = hover + centerd * (0.5f * _maxThrottle);
        commandForce = Mathf.Clamp(commandForce, 0, _maxThrottle);

        _rigidbody.AddForce(transform.up* commandForce, ForceMode.Force);
        //transform.position = transform.up * totalThrottle;

    }

}
