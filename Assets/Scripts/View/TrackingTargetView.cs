using UnityEngine;

public class TrackingTargetView : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;
    private TrackingTargetData _data;

    public void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public void Initialise(TrackingTargetData data)
    {
        _data = data;
    }

    public void Move(Vector2 direction)
    {
        _rigidbody2D.AddForce(direction * _data.Speed);
    }
}

