using UnityEngine;

public class RotatingBall
{

    public static float FindAngle(Vector2 position)
    {
        var angle = Mathf.Rad2Deg * Mathf.Atan(position.y / position.x);

        if(position.y < 0 && position.x < 0)
        {
            angle += 180;
        }
        else if(position.x < 0)
        {
            angle += 90;
        }

        return angle;
    }

}