using UnityEngine;

public class TargetSpawner : MonoBehaviour
{

    [SerializeField] private GameObject target;
    [SerializeField] private Transform[] bounds;

    private void Awake()
    {
        Create();
    }


    public void Create()
    {
        
        GameObject newTraget = Instantiate(target, GeneratePosition(), Quaternion.identity);
        Target targetComp = newTraget.GetComponent<Target>();
        targetComp.SetSpawner(this);
    }

    Vector3 GeneratePosition()
    {
        float x = Random.Range(bounds[0].position.x, bounds[1].position.x);
        float y = Random.Range(bounds[0].position.y, bounds[1].position.y);
        float z = Random.Range(bounds[0].position.z, bounds[1].position.z);

        return new Vector3(x, y, z);

    }
    
}
