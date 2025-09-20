using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Target : MonoBehaviour
{
    [SerializeField, Range(0.5f, 1f)] private float minRadius;
    [SerializeField, Range(0.6f, 2f)] private float maxRadius;
    
    [SerializeField, Range(0.5f, 1f)] private float minMass;
    [SerializeField, Range(0.6f, 2f)] private float maxMass;
    
    [SerializeField, Range(0.5f, 2f)] private float minVelocity;
    [SerializeField, Range(0.6f, 5f)] private float maxVelocity;

    private Rigidbody _rigidbody;

    public Statistic statistic;

    [SerializeField] private GameObject child;
    private Material mat;

    public void SetStats(Statistic statistic)
    {
        statistic = statistic;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<QuadricDrag>())
        {
            statistic.NewHit();

            mat.color = Color.black;
        }
        
    }


    private void OnEnable()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.mass = Random.Range(minMass, maxMass);
        _rigidbody.linearVelocity = new Vector3(Random.Range(minVelocity, maxVelocity),
            0, Random.Range(minVelocity, maxVelocity));
        
        transform.localScale*=Random.Range(minRadius, maxRadius);

        mat = child.GetComponent<MeshRenderer>().material;

    }
}
