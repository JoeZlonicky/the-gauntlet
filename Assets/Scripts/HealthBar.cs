using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private static readonly int AnimNumHearts = Animator.StringToHash("numHearts");

    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetHearts(int numHearts)
    {
        _animator.SetInteger(AnimNumHearts, numHearts);
    }
}
