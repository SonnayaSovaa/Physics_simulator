using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Target : MonoBehaviour
{
    [SerializeField, Range(0.5f, 2f)] private float minRadius;
    [SerializeField, Range(0.5f, 2f)] private float maxRadius;
    
    [SerializeField, Range(0.01f, 0.1f)] private float minMass;
    [SerializeField, Range(0.01f, 0.1f)] private float maxMass;
    
    [SerializeField, Range(0.5f, 5f)] private float minVelocity;
    [SerializeField, Range(0.5f, 5f)] private float maxVelocity;

    [SerializeField] private Collider[] colliders;

    private Rigidbody _rigidbody;

    [SerializeField] private GameObject child;
    private Material mat;

    private TargetSpawner _spawner;

    [SerializeField] private ParticleSystem flares;
    
    
    public void SetSpawner(TargetSpawner spawner)
    {
        _spawner = spawner;
    }

    private void OnTriggerEnter(Collider other)
    {
        QuadricDrag quadricDrag = other.GetComponent<QuadricDrag>();
        
        if (quadricDrag)
        {
            Destroy(colliders[0]);
            Destroy(colliders[1]);
            
            mat.color = Color.black;
            _rigidbody.isKinematic = true;
            flares.Play();
            _spawner.Create();

            _rigidbody.isKinematic = true;
            
            quadricDrag.Hit();

        }
        
    }


    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.mass = Random.Range(minMass, maxMass);
        
        _rigidbody.linearVelocity = new Vector3(RandomVelocity(), 0, RandomVelocity());
        
        transform.localScale*=Random.Range(minRadius, maxRadius);

        mat = child.GetComponent<MeshRenderer>().material;

    }

    float RandomVelocity()
    {
        if (Random.value <= 0.5) return 1*Random.Range(minVelocity, maxVelocity);
        
        return -1*Random.Range(minVelocity, maxVelocity);
    }
}
