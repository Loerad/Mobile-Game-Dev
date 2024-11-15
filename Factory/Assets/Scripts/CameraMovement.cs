using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script is not being used. It needs a rework before it can be implemented. It attaches to the camera
public class CameraMovement : MonoBehaviour
{
    private const float MIN_X = -4f;
    private const float MAX_X = 4f;
    private const float MIN_Y = -12f;
    private const float MAX_Y = 1f;   
    public Vector3 releasePosition;
    public bool released;
    
    // Update is called once per frame
    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            //https://discussions.unity.com/t/best-way-to-detect-touch-on-a-gameobject/157075/2
            if (Input.touchCount > 0)
            {
                switch (State.mode)
                {
                    case Mode.Delete:
                    {
                        if (touch.phase == TouchPhase.Moved)
                        {
                            Vector3 touchPosition;
                            if (released)
                            {
                                touchPosition = Camera.main.ScreenToViewportPoint(touch.position) + releasePosition + new Vector3(0, 0, 9);
                            }
                            else
                            {
                                touchPosition = Camera.main.ScreenToViewportPoint(touch.position) + new Vector3(0, 0, 9);
                            }
                            transform.position = new Vector3(-touchPosition.x, -touchPosition.y, transform.position.z);
                            transform.position = new Vector3(
                              Mathf.Clamp(-transform.position.x, MIN_X, MAX_X),
                              Mathf.Clamp(-transform.position.y, MIN_Y, MAX_Y),
                              transform.position.z);
                        }
                        if (touch.phase == TouchPhase.Ended)
                        {
                            releasePosition = Camera.main.ScreenToWorldPoint(touch.position) + new Vector3(0, 0, 9);
                            released = true;
                        }
                        break;
                    }
                }
            }
        }
    }
}
