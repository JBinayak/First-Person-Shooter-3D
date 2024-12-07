using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{
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
        Transform hitTransform = other.transform;
        if (hitTransform.CompareTag("Player"))
        {
            Debug.Log("Trap triggered");
            hitTransform.GetComponent<PlayerHealth>().takeDamage(20);
        }
        Destroy(gameObject);
    }
}
