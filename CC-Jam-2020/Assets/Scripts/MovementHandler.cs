using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    private Vector2 _deltaMovement;
    private Rigidbody2D _rb;
    
    public void GetInputVector(Vector2 input)
    {
        
    }

    public void MoveHorizontally()
    {
        var isGoingRight = _deltaMovement.x > 0;
    }
}