using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    public Tutorial rollTutorial;
    public Tutorial wasdTutorial;
    public Tutorial leftAttackTutorial;
    public Tutorial rightAttackTutorial;
    public Tutorial attackingTutorial;

    public UnityEvent completedAllTutorials;
    private int _numOfTutorialsLeft = 5;

    private void Start()
    {
        rollTutorial.completed.AddListener(TutorialCompleted);
        wasdTutorial.completed.AddListener(TutorialCompleted);
        leftAttackTutorial.completed.AddListener(TutorialCompleted);
        rightAttackTutorial.completed.AddListener(TutorialCompleted);
        attackingTutorial.completed.AddListener(TutorialCompleted);
    }

    private void TutorialCompleted()
    {
        if (leftAttackTutorial.isCompleted && rightAttackTutorial.isCompleted && !attackingTutorial.isCompleted) {
            attackingTutorial.MarkCompleted();
        }

        --_numOfTutorialsLeft;
        if (_numOfTutorialsLeft == 0) {
            completedAllTutorials.Invoke();
        }
    }
}
