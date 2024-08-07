using System.Collections;
using UnityEngine;

public class ButtonsManagerView : MonoBehaviour
{

    [SerializeField] private KinematicsView _kinematicsView;
    private EntryButtonView[] _buttons;

    private void OnEnable()
    {
        EntryButtonView.OnKinematicsStarted += StartKinematicsScene;
        EntryButtonView.OnDynamicsStarted += TurnOffButtons;
        EntryButtonView.OnKinematicsStarted += TurnOffButtons;
        
    }

    private void OnDisable()
    {
        EntryButtonView.OnDynamicsStarted -= TurnOffButtons;
        EntryButtonView.OnKinematicsStarted -= TurnOffButtons;
        EntryButtonView.OnKinematicsStarted -= StartKinematicsScene;
    }

    private void Awake()
    {    
        _buttons = GetComponentsInChildren<EntryButtonView>();
    }

    private void StartKinematicsScene()
    {
        StartCoroutine(KinematicsCoroutine());
        _kinematicsView.DoKinematicsScene();
    }

    private void TurnOffButtons()
    {
        foreach(EntryButtonView button in _buttons)
        {
            button.TurnOffButton();
        }
    }

    private void TurnOnButtons()
    {
        foreach(EntryButtonView button in _buttons)
        {
            button.TurnOnButton();
        }
    }

    private IEnumerator KinematicsCoroutine()
    {
        yield return new WaitForSeconds(20.0f);
        TurnOnButtons();
        _kinematicsView.StopMovement();
        _kinematicsView.DestroyObjects();
    }

}