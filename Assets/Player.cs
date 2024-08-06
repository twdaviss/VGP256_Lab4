using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private TMP_Text text;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip gainPoint;
    [SerializeField] private AudioClip losePoint;
    [SerializeField] private AudioClip hitWall;

    private float soundCooldown = 0.5f;
    private float soundTimer = 0.0f;

    private static int score = 0;
    void Update()
    {
        Vector2 moveDirection;
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");

        transform.position = new Vector3(transform.position.x + (moveDirection.x * Time.deltaTime * moveSpeed), transform.position.y + (moveDirection.y * Time.deltaTime * moveSpeed));
        text.text = score.ToString();
        soundTimer += Time.deltaTime;
    }

    public void IncreaseScore(int scoreAmount)
    {
        score += scoreAmount;
        if (scoreAmount > 0) 
        { 
            audioSource.clip = gainPoint;
        }
        else
        {
            audioSource.clip = losePoint;
        }
        if(score < 0) score = 0;  

        if(soundTimer > soundCooldown)
        {
            audioSource.Play();
            soundTimer = 0.0f;
        }
    }

    public void PlayCollisionSound()
    {
        audioSource.clip = hitWall;
        if (soundTimer > soundCooldown)
        {
            audioSource.Play();
            soundTimer = 0.0f;
        }
    }
}

