using UnityEngine; // Removed System.Numerics and Dataflow

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    // LateUpdate is better for cameras to prevent "jitter"
    void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
        }
    }
}