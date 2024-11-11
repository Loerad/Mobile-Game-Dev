using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SpawnFactory : MonoBehaviour
{
    public int factoryCount = 5;
    public GameObject factory;
    public GameObject belt;
    private Vector3 touchPosition;

    void Update()
    {
        foreach(Touch touch in Input.touches)
        {
            Debug.Log("Touching at: " + touch.position);
            //https://discussions.unity.com/t/best-way-to-detect-touch-on-a-gameobject/157075/2
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                touchPosition = Camera.main.ScreenToWorldPoint(touch.position) + new Vector3(0, 0, 9);
                Vector2 touch2D = new Vector2(touchPosition.x, touchPosition.y);
                RaycastHit2D hit = Physics2D.Raycast(touch2D, Camera.main.transform.forward);
                // Construct a ray from the current touch coordinates
                if (hit.collider.gameObject.CompareTag("Factory"))
                {
                    Debug.Log("Touched the factory");
                    Destroy(hit.collider.gameObject);
                    factoryCount++;
                    
                }
                else
                {
                    if (factoryCount <= 0)
                    {
                        
                        return;
                    }
                    factoryCount--;
                    Instantiate(factory, touchPosition, Quaternion.identity);
                }
                
            }
        }
    }
}