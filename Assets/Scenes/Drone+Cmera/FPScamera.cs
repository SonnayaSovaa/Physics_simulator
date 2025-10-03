using UnityEngine;

public class FPScamera : MonoBehaviour
{
    [SerializeField] private float _yawSensity = 180f;
    [SerializeField] private float _pitchSensity = 180f;
    [SerializeField] private float _maxPitch = 180;
    [SerializeField, Range(0, 1)] private float _rotationDamping = 0.5f;
    private Quaternion _targetRoatation;

    private float _yawDeg;
    private float _pitchDeg;


    void Awake()
    {
        _yawDeg = transform.eulerAngles.y;
        _pitchDeg = transform.eulerAngles.x;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _targetRoatation = transform.rotation;

    }

    void Update()
    {

        float dx = Input.GetAxis("Mouse X");
        float dy = Input.GetAxis("Mouse Y");

        _yawDeg += _yawSensity * dx * Time.deltaTime;
        _pitchDeg -= _pitchSensity * dy * Time.deltaTime;

        _pitchDeg = Mathf.Clamp(_pitchDeg, -_maxPitch, _maxPitch);

        Quaternion yawRot = Quaternion.AngleAxis(_yawDeg, Vector3.up);
        Vector3 rightAxis = yawRot * Vector3.right;

        Quaternion pitchRot = Quaternion.AngleAxis(_pitchDeg, rightAxis);

        _targetRoatation = pitchRot * yawRot;

        float t = 1 - Mathf.Pow(1 - Mathf.Clamp01(_rotationDamping), Time.deltaTime * 60);
        transform.rotation = Quaternion.Slerp(transform.rotation, _targetRoatation, t);

        // transform.localRotation = Quaternion.Euler(_pitchDeg, _yawDeg, 0);
    }


}
