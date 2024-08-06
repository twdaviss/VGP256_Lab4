using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    [HideInInspector] public float radius;
    private GameObject worldObjects;

    void Start()
    {
        radius = transform.localScale.x/2;
        worldObjects = GameObject.Find("WorldObjects"); //all scene objects with collision
    }

    private void Update()
    {
        CollisionDetection();
    }
    private void CollisionDetection()
    {
        for (int i = 0; i < worldObjects.transform.childCount; i++) // check for collision with any other world objects (circle or rectangle)
        {
            if(worldObjects.transform.GetChild(i).gameObject == this.gameObject) { continue; } // cant collide with itself
            if (worldObjects.transform.GetChild(i).GetComponent<Circle>() != null || worldObjects.transform.GetChild(i).transform.childCount > 0)
            {
                //Check type of circle for point value
                int pointValue;
                Circle circle;
                if (worldObjects.transform.GetChild(i).transform.childCount == 0) // some circles are children of rectangles for transformation purposes
                {
                    circle = worldObjects.transform.GetChild(i).GetComponent<Circle>(); // means it must be green circle
                    if (Vector2.Distance(transform.position, circle.transform.position) > 4 * circle.radius) { continue; } // too far away to bother checking
                    pointValue = 1;
                }
                else
                {
                    circle = worldObjects.transform.GetChild(i).transform.GetChild(0).GetComponent<Circle>(); // means it must be red circle
                    pointValue = -2;
                }
                if(circle.gameObject.GetComponent<RunAway>() != null) { pointValue = 5; } // means it must be pink circle

                //-----------------------------------------------------------------------------------------------------------------------------------------
                //Circle-Circle Collision
                float distance = Vector2.Distance(this.transform.position, circle.transform.position);
                if (distance < circle.radius + radius) // intersecting
                {
                    if(this.gameObject.CompareTag("Player"))
                    {
                        Destroy(circle.gameObject);
                        this.GetComponent<Player>().IncreaseScore(pointValue);// adds score depending on which circle
                    }
                    else
                    {
                        Vector2 direction = (transform.position - circle.transform.position).normalized;
                        
                        transform.position = (Vector2)transform.position + ((direction * (radius * 2 - distance))/2);
                        circle.transform.position = (Vector2)circle.transform.position - ((direction * (radius * 2 - distance)) / 2);
                    }
                }
            }
            //-----------------------------------------------------------------------------------------------------------------------------------------
            //Circle-Rectangle Collision
            if (worldObjects.transform.GetChild(i).GetComponent<Rectangle>() != null)
            {
                Rectangle rectangle = worldObjects.transform.GetChild(i).GetComponent<Rectangle>();

                if (Vector2.Distance(transform.position, rectangle.transform.position) > 2 * Mathf.Max(rectangle.width, rectangle.height)) { continue; } // too far away to bother checking

                // find closest x and y to get nearest edge or corner
                Vector2 edge = Vector2.zero;
                if (rectangle.minX < transform.position.x && transform.position.x < rectangle.maxX) // 'walks' in y value 
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
                if (rectangle.minY < transform.position.y && transform.position.y < rectangle.maxY) // 'walks' in y value
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
                float distX = transform.position.x - edge.x; // distance components to nearest edge/corner
                float distY = transform.position.y - edge.y;

                float distance = Mathf.Sqrt((distX * distX) + (distY * distY)); // distance from closest edge
                if (distance < radius) // intersecting
                {
                    Vector2 direction = (Vector2)transform.position - edge; // direction of overlap
                    float difference = radius - distance; // exact amount of overlap needed to be resolved
                    Vector2 offset = direction.normalized * difference;

                    transform.position = transform.position + (Vector3)offset; // moved back by the amount of interection in the opposite direction of the edge

                    if (this.CompareTag("Player"))
                    {
                        this.GetComponent<Player>().PlayCollisionSound();
                    }
                }
            }
        }
    }
}
