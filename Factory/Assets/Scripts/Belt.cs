using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Belt : MonoBehaviour
{
    public Spawn spawn;
    LineRenderer lr;
    // Start is called before the first frame update
    void Awake()
    {
        spawn = GameObject.Find("Main Camera").GetComponent<Spawn>();
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            switch (touch.phase)
            {
                case TouchPhase.Moved:
                {
                    lr.SetPosition(1, spawn.touchPosition);
                    break;
                }
                case TouchPhase.Stationary:
                {
                    lr.SetPosition(1, spawn.touchPosition);
                    break;
                }
                case TouchPhase.Ended:
                    {
                        
                        Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position) + new Vector3(0, 0, 9);
                        Vector2 touch2D = new Vector2(touchPosition.x, touchPosition.y);
                        RaycastHit2D hit = Physics2D.Raycast(touch2D, Camera.main.transform.forward);
                        if (hit.collider.gameObject.CompareTag("Factory"))
                        {
                            GameObject target = hit.collider.gameObject;
                            Vector3 inputPoint = target.transform.position;
                            if (target.GetComponent<Factory>().inputBelt != Vector3.zero) { Destroy(gameObject); return; }
                            

                            lr.SetPosition(1, inputPoint);

                            lr.useWorldSpace = true;
                            
                            target.GetComponent<Factory>().inputBelt = lr.GetPosition(1);
                            target.GetComponent<Factory>().inputBeltObject = gameObject;

                            enabled = false;
                        }
                        else
                        {
                            Destroy(gameObject);
                        }
                        break;
                    }
            }
        }
    }
}
