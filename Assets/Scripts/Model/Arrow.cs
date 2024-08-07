using UnityEngine;

public class Arrow
{
    public static float GetAngle(Vector2 direction)
    {
        var angle = Mathf.Rad2Deg * Mathf.Atan(direction.y / direction.x);
        float deltaAngle = -90.0f;

        if(direction.x < 0 && direction.y < 0)
        {
            deltaAngle = 180;
        }
        else if(direction.x < 0)
        {
            deltaAngle = 90;
        }
        return (angle + deltaAngle);
    }
}
