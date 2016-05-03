using UnityEngine;
using System.Collections;

public class Elevator : MonoBehaviour
{
    public void Move(Vector2 move_distance)
    {
        /* This line has a Vector2 & Vector3 conflict */
        transform.position += new Vector3(move_distance.x, move_distance.y, 0);

        //GameObject.FindGameObjectWithTag("Player").transform.position += new Vector3(move_distance.x, move_distance.y, 0);

        // keeps the camera centered on the elevator
        Camera.main.transform.position = new Vector3(transform.position.x + 2f, transform.position.y - 2f, Camera.main.transform.position.z);
    }
}
