using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField, Range(0.5f, 1f)] private float minRadius;
    [SerializeField, Range(0.6f, 2f)] private float maxRadius;
    
    [SerializeField, Range(0.5f, 1f)] private float minMass;
    [SerializeField, Range(0.6f, 2f)] private float maxMass;
    
    [SerializeField, Range(0.5f, 2f)] private float minVelocity;
    [SerializeField, Range(0.6f, 5f)] private float maxVelocity;

    private Rigidbody _rigidbody;


    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.mass = Random.Range(minMass, maxMass);
        _rigidbody.linearVelocity = new Vector3(Random.Range(minVelocity, maxVelocity),
            0, Random.Range(minVelocity, maxVelocity));
        
        transform.localScale*=Random.Range(minRadius, maxRadius);
    }
}
