using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 5f;
    public Vector3 offset;
    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float zoom = 4f;

    private void Start()
    {
        Camera.main.orthographicSize = zoom;
    }
    private void LateUpdate()
    {
        if (target == null) return;

        Vector3 cameraPosition = target.position + offset;

        float batasX = Mathf.Clamp(cameraPosition.x, minX, maxX);
        float batasY = Mathf.Clamp(cameraPosition.y, minY, maxY);

        Vector3 batasPosisi = new Vector3(batasX, batasY, cameraPosition.z);

        transform.position = Vector3.Lerp(transform.position, batasPosisi, smoothSpeed * Time.deltaTime);
    }
}
