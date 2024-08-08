using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class RotatingBallView : MonoBehaviour
{
    private bool _isTurning = false;
    private Vector2 _previousPosition = new(0, 0);
    private List<float> _speeds = new();
    private float _previousRadius = 0;
    private float _timeEstablished = 2.0f;

    private void Update()
    {
        if(_isTurning)
        {
            Vector2 currentPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            var radius = Mathf.Sqrt(Mathf.Pow(currentPosition.x, 2) + Mathf.Pow(currentPosition.y, 2));

            var angle = RotatingBall.FindAngle(currentPosition.normalized);
            var previousAngle = RotatingBall.FindAngle(_previousPosition.normalized);

            var angleSpeed = (angle - previousAngle) / Time.fixedDeltaTime;
            

            _previousPosition = currentPosition;
            _previousRadius = radius;

            _speeds.Add(angleSpeed);

            _timeEstablished -= Time.fixedDeltaTime;

            transform.GetChild(1).localPosition = currentPosition.normalized * 3;

            if(_timeEstablished <= 0)
            {
                _timeEstablished = 2.0f;
                float midSpeed = 0;
                foreach(float speed in _speeds)
                {
                    midSpeed += speed;
                }

                midSpeed = midSpeed / _speeds.Count;

                _speeds = new();
                if(midSpeed == float.NaN) GetComponentInChildren<TextMeshProUGUI>().text = "0.0";
                else GetComponentInChildren<TextMeshProUGUI>().text = Mathf.Abs(Mathf.Round(midSpeed)).ToString();
            }
        }
    }

    private void OnMouseDown()
    {
        _isTurning = true;
    }

    private void OnMouseUp()
    {
        _isTurning = false;
    }
}
