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
    private int _numOfTutorialsLeft;

    private void Start()
    {
        Tutorial[] tutorials = {rollTutorial, wasdTutorial, leftAttackTutorial, rightAttackTutorial, attackingTutorial};

        _numOfTutorialsLeft = 0;
        foreach (Tutorial tutorial in tutorials)
        {
            tutorial.completed.AddListener(TutorialCompleted);
            ++_numOfTutorialsLeft;
        }
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
