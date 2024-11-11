using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
                            if (target.GetComponent<Factory>().inputBelt[0] != Vector3.zero) //is there a belt in the first slot?
                            { 
                                //yes there is, check if there is a belt in the second slot
                                if (target.GetComponent<Factory>().inputBelt[1] != Vector3.zero) //is there a belt in the second slot?
                                { 
                                    Destroy(gameObject); return; //its full, destroy yourself now!
                                }
                                else //no, this one is free
                                {
                                    lr.SetPosition(1, inputPoint);

                                    lr.useWorldSpace = true;

                                    target.GetComponent<Factory>().inputBelt[1] = lr.GetPosition(1);
                                    target.GetComponent<Factory>().inputBeltObject[1] = gameObject;

                                    enabled = false;
                                }
                            }
                            else //no, there isn't, put it here
                            {
                                lr.SetPosition(1, inputPoint);

                                lr.useWorldSpace = true;

                                target.GetComponent<Factory>().inputBelt[0] = lr.GetPosition(1);
                                target.GetComponent<Factory>().inputBeltObject[0] = gameObject;

                                enabled = false;
                            }
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
