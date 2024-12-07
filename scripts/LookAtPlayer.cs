using System;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    public Transform target;

    private void Start()
    {
        target = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        if (target != null)
        {
            transform.LookAt(target);
        }
    }
}
