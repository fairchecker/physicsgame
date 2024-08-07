using System;
using UnityEngine;

public class EntryButtonView : MonoBehaviour
{
    private EntryButtonData _data;
    
    public static event Action OnKinematicsStarted;
    public static event Action OnDynamicsStarted;
    
    public void Initialise(EntryButtonData data)
    {
        _data = data;
    }

    public void OnClick()
    {
        switch(_data.Direction)
        {
            case EntryButtonData.Directions.Kinematics:
                OnKinematicsStarted?.Invoke();
                break;
            case EntryButtonData.Directions.Dynamics:
                OnDynamicsStarted?.Invoke();
                break;
            case EntryButtonData.Directions.Electrodynamics:
                //doo
                break;
        }
    }

    public void TurnOffButton()
    {
        this.gameObject.SetActive(false);
    }
    public void TurnOnButton()
    {
        this.gameObject.SetActive(true);
    }
}
