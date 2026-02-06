using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public float zoomSpeed = 2f;        
    public float minZoom = 2f;          
    public float maxZoom = 5f;          
    public float recenterSmoothTime = 0.2f; 

    private Camera cam;
    private Vector3 velocity = Vector3.zero; 

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Mathf.Abs(scroll) > 0.01f)
        {
            
            Vector3 mouseWorldBefore = cam.ScreenToWorldPoint(Input.mousePosition);

            
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);

           
            if (scroll > 0)
            {
                Vector3 mouseWorldAfter = cam.ScreenToWorldPoint(Input.mousePosition);
                Vector3 offset = mouseWorldBefore - mouseWorldAfter;
                cam.transform.position += offset;
            }
           
            else if (scroll < 0)
            {
                Vector3 targetPos = new Vector3(0, 0, cam.transform.position.z);
                cam.transform.position = Vector3.SmoothDamp(cam.transform.position, targetPos, ref velocity, recenterSmoothTime);
            }
        }

        
    }


}
