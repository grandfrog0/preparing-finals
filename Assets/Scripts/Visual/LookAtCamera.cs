using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    private Vector3 startPos;

    private void FixedUpdate()
    {
        transform.forward = transform.position - Camera.main.transform.position;
        transform.localPosition = startPos + Vector3.up * Mathf.Sin(Time.time) * 0.25f;
    }

    private void Start()
    {
        startPos = transform.localPosition;
    }
}
