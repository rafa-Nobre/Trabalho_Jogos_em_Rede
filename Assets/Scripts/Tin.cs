using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tin : MonoBehaviour, IShotHit
{
    new Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void IShotHit.Hit(Vector3 direction){
        rigidbody.AddForce(Vector3.Scale(direction, new Vector3(50,100,50)));
    }
}
