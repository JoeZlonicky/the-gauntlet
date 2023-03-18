using UnityEngine;
using UnityEngine.Events;

public class Tutorial : MonoBehaviour
{
    public UnityEvent completed;
    [HideInInspector] public bool isCompleted = false;
    
    private Animator _animator;
    private static readonly int CompletedTrigger = Animator.StringToHash("completedTrigger");


    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void MarkCompleted()
    {
        if (isCompleted) {
            return;
        }
        isCompleted = true;
        _animator.SetTrigger(CompletedTrigger);
        completed.Invoke();
    }
}
