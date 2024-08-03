using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotationAxis
{ 
    None,
    X,
    Y,
    Z
};

public class Transformation : MonoBehaviour
{
    [Header("World Scaling")]
    [SerializeField] public Vector3 worldScale = Vector3.one;

    [Header("Local Scaling")]
    [SerializeField] public Vector3 localScale = Vector3.one;

    [Header("World Rotation")]
    //[SerializeField] Vector3 worldRotationAxis = Vector3.zero;
    [SerializeField] RotationAxis worldRotationAxis = RotationAxis.None;

    [Range(0.0f, 360.0f)]
    [SerializeField] float worldRotationDegrees = 0.0f;

    [Header("Local Rotation")]
    //[SerializeField] Vector3 localRotationAxis = Vector3.zero;
    [SerializeField] RotationAxis localRotationAxis = RotationAxis.None;

    [Range(0.0f, 360.0f)]
    [SerializeField] float localRotationDegrees = 0.0f;

    [Header ("World Translation")]
    [SerializeField] private Vector3 worldTranslationVector = Vector3.zero;

    [Header("Local Translation")]
    [SerializeField] private Vector3 localTranslationVector = Vector3.zero;

    private Vector3 parentPosition;
    private Quaternion parentRotation;
    private Vector3 parentScale;
    
    private Vector3 defaultScale;
    private Vector3 scale;
    private Vector3 scaleRelativeToParent;

    private Quaternion defaultRotation;
    private Quaternion rotation;
    private Quaternion rotationRelativeToParent;
    
    private Vector3 defaultPosition;
    private Vector3 position;
    private Vector3 positionRelativeToParent;

    public bool keepRotating;
    public float rotationSpeed;
    public int reverseDirectionMultiplier;

    void Start()
    {
        parentPosition = transform.parent.position;
        parentRotation = transform.parent.rotation;
        parentScale = transform.parent.lossyScale;

        defaultRotation = transform.rotation;
        defaultScale = transform.lossyScale;
        defaultPosition = transform.position;

        scale = defaultScale;
        rotation = defaultRotation;
        position = defaultPosition;

        //position = parentPosition + (scale * positionRelativeToParent)
        //positionRelativeToParent = (position - parentPosition) / scale
    }

    // Update is called once per frame
    void Update()
    {
        if (keepRotating)
        {
            worldRotationDegrees +=  reverseDirectionMultiplier * Time.deltaTime * rotationSpeed;
        }
        ScalingWorld(worldScale);
        ScalingParent(localScale);
        RotationWorld(worldRotationDegrees, worldRotationAxis);
        RotationParent(localRotationDegrees, localRotationAxis);
        TranslationWorld(worldTranslationVector);
        TranslationParent(localTranslationVector);

        transform.localScale = scaleRelativeToParent;
        transform.localRotation = rotationRelativeToParent;
        transform.localPosition = positionRelativeToParent;
    }
    private void ScalingWorld(Vector3 scaleFactor)
    {
        scale = Vector3.Scale(defaultScale, scaleFactor);
        scaleRelativeToParent = new Vector3(scale.x / parentScale.x, scale.y / parentScale.y, scale.z / parentScale.z);
    }

    private void ScalingParent(Vector3 scaleFactor)
    {
        scaleRelativeToParent = new Vector3(scale.x / parentScale.x * scaleFactor.x, scale.y / parentScale.y * scaleFactor.y, scale.z / parentScale.z * scaleFactor.z);
    }
    private void TranslationWorld(Vector3 translation)
    {
        position += translation;
        positionRelativeToParent.x = (position.x - parentPosition.x) / parentScale.x;
        positionRelativeToParent.y = (position.y - parentPosition.y) / parentScale.y;
        positionRelativeToParent.z = (position.z - parentPosition.z) / parentScale.z;
    }

    private void TranslationParent(Vector3 translation)
    {
        positionRelativeToParent += translation;
    }

    private void RotationWorld(float angle, RotationAxis rotationAxis)
    {
        Vector3 axis;
        switch (rotationAxis)
        {
            case RotationAxis.None:
                return;
            case RotationAxis.X:
                axis = new Vector3(1, 0, 0);
                break;
            case RotationAxis.Y:
                axis = new Vector3(0, 1, 0);
                break;
            case RotationAxis.Z:
                axis = new Vector3(0, 0, 1);
                break;
            default:
                return;
        }

        angle *= Mathf.Deg2Rad; //to radians
        Quaternion rotateQ = new Quaternion(Mathf.Sin(angle / 2) * axis.x, Mathf.Sin(angle / 2) * axis.y, Mathf.Sin(angle / 2) * axis.z, Mathf.Cos(angle/2));
        rotation = defaultRotation * rotateQ;

        rotationRelativeToParent = Quaternion.Inverse(parentRotation) * rotation;
        UpdateRotatedPosition();
    }

    private void RotationParent(float angle, RotationAxis rotationAxis)
    {
        Vector3 axis;
        switch (rotationAxis)
        {
            case RotationAxis.None:
                return;
            case RotationAxis.X:
                axis = new Vector3(1, 0, 0);
                break;
            case RotationAxis.Y:
                axis = new Vector3(0, 1, 0);
                break;
            case RotationAxis.Z:
                axis = new Vector3(0, 0, 1);
                break;
            default:
                return;
        }
        angle *= Mathf.Deg2Rad; //to radians
        //rotationRelativeToParent = Quaternion.Inverse(parentRotation) * rotation;

        Quaternion q = new Quaternion(Mathf.Sin(angle / 2) * axis.x, Mathf.Sin(angle / 2) * axis.y, Mathf.Sin(angle / 2) * axis.z, Mathf.Cos(angle / 2));
        rotationRelativeToParent = q;
    }

    private void UpdateRotatedPosition()
    {
        positionRelativeToParent.x = (defaultPosition.x - parentPosition.x) / parentScale.x;
        positionRelativeToParent.y = (defaultPosition.y - parentPosition.y) / parentScale.y;
        positionRelativeToParent.z = (defaultPosition.z - parentPosition.z) / parentScale.z;

        Quaternion positionQ = new Quaternion(positionRelativeToParent.x, positionRelativeToParent.y, positionRelativeToParent.z, 0);
        positionQ = rotationRelativeToParent * positionQ * Quaternion.Inverse(rotationRelativeToParent);

        positionRelativeToParent.x = positionQ.x;
        positionRelativeToParent.y = positionQ.y;
        positionRelativeToParent.z = positionQ.z;

        position = Vector3.Scale(positionRelativeToParent, parentScale) + parentPosition;
    }
}
