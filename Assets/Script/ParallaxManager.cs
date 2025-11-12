using UnityEngine;

public class ParallaxManager : MonoBehaviour
{
    public Transform cameraTransform;
    public float[] parallaxSpeeds; // isi sesuai urutan child

    private Vector3 lastCameraPosition;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 delta = cameraTransform.position - lastCameraPosition;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            float speed = (i < parallaxSpeeds.Length) ? parallaxSpeeds[i] : 0.5f;
            child.position += new Vector3(delta.x * speed, 0f, 0f);
        }
        lastCameraPosition = cameraTransform.position;
    }
}
