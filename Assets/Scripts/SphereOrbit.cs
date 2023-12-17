using UnityEngine;

public class SphereOrbit : MonoBehaviour
{
    public Transform center; 
    public float orbitSpeed = 20f;
    public Vector3 orbitAxis = Vector3.up; 

    private Vector3 initialPosition;

    void Start()
    {
        
        initialPosition = transform.position;
    }

    void Update()
    {
        if (center != null)
        {
          
            Quaternion rotation = Quaternion.AngleAxis(orbitSpeed * Time.time, orbitAxis);
            Vector3 orbitPosition = center.position + rotation * (initialPosition - center.position);

           
            transform.position = orbitPosition;
        }
    }
}
