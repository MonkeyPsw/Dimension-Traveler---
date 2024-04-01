using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpTest : MonoBehaviour
{
    public float jumpForce = 8.0f; // Á¡ÇÁ Èû
    public float jumpMaxY = 10.0f;
    public float jumpY = 0;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump(jumpForce);
        }
    }

    public void Jump(float force)
    {
        transform.Translate(0, jumpForce * Time.deltaTime, 0);
        if (transform.position.y > jumpMaxY + jumpY)
            transform.Translate(0, -jumpForce * Time.deltaTime, 0);
    }
}
