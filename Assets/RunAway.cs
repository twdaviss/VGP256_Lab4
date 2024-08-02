using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunAway : MonoBehaviour
{
    [SerializeField] private float minDistance;
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = (transform.position - player.transform.position).normalized;
        float distance = Vector2.Distance(player.transform.position, transform.position);
        if (distance < minDistance)
        {
            transform.position = (Vector2)transform.position + (direction * (minDistance-distance)) ;
        }
    }
}
