using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperJumpCube : MonoBehaviour
{
    float superJumpPower = 200;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ãæµ¹!!");

        if (other.TryGetComponent(out Rigidbody rigidbody))
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z);
            rigidbody.AddForce(Vector3.up * superJumpPower, ForceMode.Impulse);

        }
    }
}
