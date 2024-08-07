using UnityEngine;

public class EntryPoint : MonoBehaviour
{
    private void Awake()
    {
        var targetData = new TrackingTargetData(5.0f);
        GetComponent<TrackingTargetView>().Initialise(targetData);
    }
}
