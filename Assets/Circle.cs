using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    public Vector2 centrePoint;
    public float radius = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        centrePoint = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
