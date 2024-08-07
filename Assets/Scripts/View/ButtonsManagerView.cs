using System.Collections;
using UnityEngine;

public class ButtonsManagerView : MonoBehaviour
{

    private KinematicsView _kinematicsView;
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
        _kinematicsView = GetComponent<KinematicsView>();
        _buttons = GetComponentsInChildren<EntryButtonView>();
    }

    private void StartKinematicsScene()
    {
        Debug.Log("started");
        StartCoroutine(KinematicsCoroutine());
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
        Debug.Log("prekol");
        foreach(EntryButtonView button in _buttons)
        {
            Debug.Log("buba");
            button.TurnOnButton();
        }
    }

    private IEnumerator KinematicsCoroutine()
    {
        Debug.Log("ggg");
        yield return new WaitForSeconds(4.0f);
        TurnOnButtons();
    }

}