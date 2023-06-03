using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarMenuMovement : MonoBehaviour
{
    public bool DirectionUp;
    public float speed;

    private void Update()
    {
        if (DirectionUp)
        {
            transform.Translate(Vector2.up * speed * Time.deltaTime);
            if (transform.position.y > Screen.height + 2)
            {
                Destroy(gameObject); 
            } 
        }
        else
        {
            transform.Translate(Vector2.down * speed * Time.deltaTime);
            if(transform.position.y < Screen.height - 2)
            {
                Destroy(gameObject); 
            }
        }
    }
}
