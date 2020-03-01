﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    Rigidbody[] rigidbodies;
    Animator animator;
    public bool ragdoll = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        SetRagdoll(ragdoll);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SetRagdoll(!ragdoll);
        }
    }

    void SetRagdoll(bool on)
    {
        ragdoll = on;

        animator.enabled = !on;
        foreach (Rigidbody rb in rigidbodies)
            rb.isKinematic = !on;
    }
}