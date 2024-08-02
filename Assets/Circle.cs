using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    [HideInInspector] public float radius = 0.5f;
    private Vector2 centrePoint;
    [SerializeField] private GameObject worldObjects;

    void Start()
    {
        centrePoint = transform.position;
        worldObjects = GameObject.Find("WorldObjects");
    }

    private void Update()
    {
        CollisionDetection();
    }
    private void CollisionDetection()
    {
        for (int i = 0; i < worldObjects.transform.childCount; i++)
        {
            if(worldObjects.transform.GetChild(i).gameObject == this.gameObject) { return; }
            if (worldObjects.transform.GetChild(i).GetComponent<Circle>() != null)
            {
                Circle circle = worldObjects.transform.GetChild(i).GetComponent<Circle>();
                if (Vector2.Distance(transform.position, circle.transform.position) > 4 * circle.radius) { continue; }

                float distance = Vector2.Distance(this.transform.position, circle.transform.position);
                if (distance < circle.radius + radius) // intersecting
                {
                    if(this.gameObject.CompareTag("Player"))
                    {
                        Destroy(circle.gameObject);
                    }
                    else
                    {
                        Vector2 direction = (transform.position - circle.transform.position).normalized;
                        
                        transform.position = (Vector2)transform.position + ((direction * (radius * 2 - distance))/2);
                        circle.transform.position = (Vector2)circle.transform.position - ((direction * (radius * 2 - distance)) / 2);
                    }
                }
            }
            else if (worldObjects.transform.GetChild(i).GetComponent<Rectangle>() != null)
            {
                Rectangle rectangle = worldObjects.transform.GetChild(i).GetComponent<Rectangle>();

                if (Vector2.Distance(transform.position, rectangle.transform.position) > 2 * Mathf.Max(rectangle.width, rectangle.height)) { continue; }

                Vector2 edge = Vector2.zero;
                if (rectangle.minX < transform.position.x && transform.position.x < rectangle.maxX)
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
                if (distance < radius) // intersecting
                {
                    Vector2 direction = (Vector2)transform.position - edge;
                    float difference = radius - distance;
                    Vector2 offset = direction.normalized * difference;
                    transform.position = transform.position + (Vector3)offset; // moved back by the amount of interection in the opposite direction of the edge
                }
            }
        }
    }
}
