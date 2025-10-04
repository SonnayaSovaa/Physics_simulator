using System;
using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class JetEngine : MonoBehaviour
{

    [SerializeField] private Transform _nozzle;
    [SerializeField] private float _thrustDrySL = 79000f;
    [SerializeField] private float _thrustABSL = 129000f;


    [SerializeField] private float _throttleRate = 1.0f;
    [SerializeField] private float _throttleStep = 0.05f;

    [SerializeField] private InputActionAsset actions;



    private InputAction _throttleUpHold;
    private InputAction _throttleDownHold;
    private InputAction _throttleStepUp;
    private InputAction _throttleStepDown;
    private InputAction _toggleAB;




    private Rigidbody _rigidBody;

    private float _throttle01;
    private bool _afterBurener;

    private float _speedMS;
    private float _lastAppliedThrus;
    
    
    private float  _startPos;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag=="Floor")
            _startPos = transform.position.y;
    }

    void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();

        _throttle01 = 0f;
        _afterBurener = false;
        
        

        InitializeActions();
    }

    void InitializeActions()
    {
        var map = actions.FindActionMap("Jet");

        _throttleUpHold = map.FindAction("ThrottleUp");
        _throttleDownHold = map.FindAction("ThrottleDown");
        _throttleStepUp = map.FindAction("ThrottleStepUp");
        _throttleStepDown = map.FindAction("ThrottleStepDown");
        _toggleAB = map.FindAction("ToggleAB");


        _throttleStepUp.performed += _ => AdjustThrottle(+_throttleStep);
        _throttleStepDown.performed+= _ => AdjustThrottle(-_throttleStep);

        _toggleAB.performed += _ => { _afterBurener = !_afterBurener; };



    }

    private void OnEnable()
    {
        _throttleUpHold.Enable();
        _throttleDownHold.Enable();

    }



    private void AdjustThrottle(float delta)
    {
        _throttle01 = Mathf.Clamp01(_throttle01 * delta);
    }

    private void FixedUpdate()
    {

        _speedMS = _rigidBody.linearVelocity.magnitude;

        float dt = Time.fixedDeltaTime;

        if (transform.position.y-0.1f > _startPos) _nozzle.localEulerAngles = Vector3.zero;
        


        if (_throttleUpHold.IsPressed())
            _throttle01 = Mathf.Clamp01(_throttle01 + _throttleRate * dt);


        if (_throttleDownHold.IsPressed())
            _throttle01 = Mathf.Clamp01(_throttle01 - _throttleRate * dt);


        float throttle = _throttle01 * (_afterBurener ? _thrustABSL : _thrustDrySL);
        _lastAppliedThrus = throttle;

        if (_nozzle != null && throttle > 0)
        {
            Vector3 force = _nozzle.forward * throttle;
            _rigidBody.AddForceAtPosition(force, _nozzle.position, ForceMode.Force);


        }

    }
    
    void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 35;
        GUI.color = Color.black;
        GUILayout.Label($" ", style);
        GUILayout.Label($" ", style);

        GUILayout.Label($"Burning: {_afterBurener}", style);
        GUILayout.Label($"Throttle 01: {_throttle01:0.0}", style);
        


    }



}
