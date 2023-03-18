using UnityEngine;
using Random = UnityEngine.Random;

public class CrowdMember : MonoBehaviour
{
    private Animator _animator;
    
    void Start()
    {
        _animator = GetComponent<Animator>();

        // Want random start point so the crowd isn't in sync
        float random = Random.Range(0.0f, 1.0f);
        random = Mathf.Round(random * 10.0f) * 0.1f;
        _animator.Play("cheering", -1, random);
    }
}
