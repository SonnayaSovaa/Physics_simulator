using UnityEngine;

[RequireComponent(typeof(ForceVisualizer))]

public class SimplePhysicsEngine : MonoBehaviour
{

    [SerializeField] private float _mass;
    [SerializeField] private bool _isGravity;

    [SerializeField] private float _dragCoefficient = 0.1f;

    private ForceVisualizer _forceVisualizer;
    private Vector3 _netForce;


    private void Start()
    {
        _forceVisualizer = GetComponent<ForceVisualizer>();
    }

    void FixedUpdate()
    {
        _netForce = Vector3.zero;
        _forceVisualizer.ClearForces();

        if (_isGravity )
        {
            Vector3 gravity = Physics.gravity * _mass;
            ApplyForce(gravity, Color.red, "Gravity");
        }
    }

    private void ApplyForce(Vector3 vector, Color colorForce, string name)
    {
        _netForce += vector;
        _forceVisualizer.AddForce(_netForce, colorForce, name);
    }

}
