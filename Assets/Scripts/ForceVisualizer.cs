using System;
using System.Collections.Generic;
using UnityEngine;

public class ForceVisualizer : MonoBehaviour
{ 
    [SerializeField] private List<Force> _forces = new();

    public void OnDrawGizmos()
    {
        foreach (Force force in _forces)
        {
            Gizmos.color = force.ColorForce;
            Vector3 start = transform.position;
            Vector3 end = transform.position + force.Vector;

            Gizmos.DrawLine(start, end);
        }
    }

    public void AddForce(Vector3 vector, Color colorForce, string name) => _forces.Add(new Force(vector, colorForce, name));

    public void ClearForces() => _forces.Clear();
}


[Serializable]
public class Force
{

    public Vector3 Vector;
    public Color ColorForce;
    public string Name;

    public Force (Vector3 vector, Color colorForce, string name)
    {
        Vector = vector;
        ColorForce = colorForce;
        Name = name;
    }
}