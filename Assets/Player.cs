using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private float radius = 0.5f;
    private static int score;

    void Update()
    {
        Vector2 moveDirection;
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");

        transform.position = new Vector3(transform.position.x + (moveDirection.x * Time.deltaTime * moveSpeed), transform.position.y + (moveDirection.y * Time.deltaTime * moveSpeed));
    }

    public void IncreaseScore(int scoreType)
    {
        if(scoreType == 0)
        {
        }
        else if (scoreType == 1)
        {
        }
    }
}

