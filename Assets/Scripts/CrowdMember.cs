using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CrowdMember : MonoBehaviour
{
    private Animator _animator;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        float random = Random.Range(0.0f, 1.0f);
        random = Mathf.Round(random * 10.0f) * 0.1f;
        _animator.Play("cheering", -1, random);

    }

    private void OnEnable()
    {
        
    }
}
