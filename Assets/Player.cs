using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private List<GameObject> worldObjects;

    private float radius = 0.5f;
    void Update()
    {
        Vector2 moveDirection;
        moveDirection.x = Input.GetAxisRaw("Horizontal");
        moveDirection.y = Input.GetAxisRaw("Vertical");

        transform.position = new Vector3(transform.position.x + (moveDirection.x * Time.deltaTime * moveSpeed), transform.position.y + (moveDirection.y * Time.deltaTime * moveSpeed));
        CollisionDetection();
    }

    private void CollisionDetection()
    {
        if (worldObjects.Count == 0) { return; }
        foreach (var obj in worldObjects)
        {
            if (obj.GetComponent<Circle>() != null)
            {
                if (Vector2.Distance(transform.position, obj.transform.position) > 4) { continue; }

                Circle circle = obj.GetComponent<Circle>();

                float distance = Vector2.Distance(this.transform.position, circle.centrePoint);
                if(distance < circle.radius + radius) // intersecting
                {
                    worldObjects.Remove(circle.gameObject);
                    Destroy(circle.gameObject);
                    return;
                }
            }
            else if (obj.GetComponent<Rectangle>() != null)
            {
                if(Vector2.Distance(transform.position, obj.transform.position) > 4) { continue;}
                
                Rectangle rectangle = obj.GetComponent<Rectangle>();
                
                Vector2 edge = Vector2.zero;
                if(rectangle.minX < transform.position.x && transform.position.x < rectangle.maxX)
                {
                    edge.x = transform.position.x;
                }
                else if (transform.position.x > rectangle.maxX)
                {
                    edge.x = rectangle.maxX;
                }
                else if (transform.position.x < rectangle.minX)
                {
                    edge.x = rectangle.minX;
                }
                if (rectangle.minY < transform.position.y && transform.position.y < rectangle.maxY)
                {
                    edge.y = transform.position.y;
                }
                else if (transform.position.y > rectangle.maxY)
                {
                    edge.y = rectangle.maxY;
                }
                else if (transform.position.y < rectangle.minY)
                {
                    edge.y = rectangle.minY;
                }
                float distX = transform.position.x - edge.x;
                float distY = transform.position.y - edge.y;

                float distance = Mathf.Sqrt((distX * distX) + (distY * distY));
                if(distance < radius) // intersecting
                {
                    Vector2 direction = (Vector2)transform.position - edge;
                    float difference = radius - distance;
                    Vector2 offset = direction.normalized * difference;
                    transform.position = transform.position + (Vector3)offset; // moved back by the amount of interection in the opposite direction of the edge
                    return;
                }
            }
        }
    }
}

