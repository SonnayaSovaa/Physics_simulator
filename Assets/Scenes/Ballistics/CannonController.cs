
using System;
using UnityEngine;

public class CannonController : MonoBehaviour
{
    private Cannon_control _control;
    private Vector2 _inputDir_XZ;
    private Vector2 _inputDir_Y;
    
    private float _yPos;

    private Vector3 _movingDir;

    [SerializeField] private float rotSpeed;
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject  muzzle;


    private void Awake()
    {
        _control = new Cannon_control();
        _control.Cannon.Moving.started += ctx => OnMovement();
        _control.Cannon.Yrotation.started += ctx => MuzzleRotation();

    }


    private void FixedUpdate()
    {
         OnMovement();
         MuzzleRotation();
    }


    void MuzzleRotation()
    {
        _inputDir_Y = _control.Cannon.Yrotation.ReadValue<Vector2>();
        
        muzzle.transform.eulerAngles+= new Vector3(_inputDir_Y.y*rotSpeed, 0, 0);
    }
    
    void OnMovement()
    {
        _inputDir_XZ = _control.Cannon.Moving.ReadValue<Vector2>();
        
        _movingDir = new Vector3(_inputDir_XZ.y*transform.forward.x, _yPos, _inputDir_XZ.y*transform.forward.z);


        transform.eulerAngles+= new Vector3(0, _inputDir_XZ.x*rotSpeed, 0);

        Vector3 deltaPos = _movingDir * Time.deltaTime * moveSpeed;

        transform.position += deltaPos;
        
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
