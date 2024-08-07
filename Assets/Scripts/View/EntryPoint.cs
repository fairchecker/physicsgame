using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    [SerializeField] private EntryButtonView _kinematicsButtonView;
    [SerializeField] private EntryButtonView _dynamicsButtonView;

    private void Awake()
    {
        _kinematicsButtonView.Initialise(new EntryButtonData(EntryButtonData.Directions.Kinematics));
        _dynamicsButtonView.Initialise(new EntryButtonData(EntryButtonData.Directions.Dynamics));
    }
}