using UnityEngine;

public class Rectangle : MonoBehaviour
{
    public float width = 2.0f;
    public float height = 1.0f;
    public Vector2 centrePoint;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;

    // Start is called before the first frame update
    void Start()
    {
        centrePoint = transform.position;
        minX = centrePoint.x - width/2;
        maxX = centrePoint.x + width/2;
        minY = centrePoint.y - height/2;
        maxY = centrePoint.y + height/2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
