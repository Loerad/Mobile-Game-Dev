using UnityEngine;
using UnityEngine.UIElements;

public class Belt : MonoBehaviour
{
    public Spawn spawn;
    public LineRenderer lr;
    public Vector3 directionVector;
    public GameObject arrow;
    public GameObject target;
    public GameObject placed = null;
    public GameObject origin; //this is unused but can be used if you need the belt to know where it starts

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
                    lr.SetPosition(1, spawn.touchPosition); //moves with finger
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
                        target = hit.collider.gameObject;
                        Vector3 inputPoint = target.transform.position;

                        if (target.GetComponent<Factory>().inputBelt[0] != Vector3.zero) //is there a belt in the first slot?
                        { 
                            //yes there is because that vector is not zero, check if there is a belt in the second slot
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
                    else if (hit.collider.gameObject.CompareTag("Finish"))
                    {
                        target = hit.collider.gameObject;
                        Vector3 inputPoint = target.transform.position;
                        if (target.GetComponent<Finish>().inputBelt != Vector3.zero) //is there a belt in the finish?
                        {
                            Destroy(gameObject); return; //its full, destroy yourself now!
                        }
                        else
                        {
                            lr.SetPosition(1, inputPoint);

                            lr.useWorldSpace = true;

                            target.GetComponent<Finish>().inputBelt = lr.GetPosition(1);
                            target.GetComponent<Finish>().inputBeltObject = gameObject;

                            enabled = false;
                        }
                    }
                    else //not placed on a factory or finish. Foundries cannot take inputs
                    {
                        Destroy(gameObject); return;
                    }

                    //https://discussions.unity.com/t/look-rotation-2d-equivalent/728105/2
                    directionVector = lr.GetPosition(0) - lr.GetPosition(1); //this is the wrong way around on purpose, switching it makes the arrow spawn off the belt for some reason
                    Vector3 rotatedVectorToEnd = Quaternion.Euler(0,0,90) * -directionVector; //^ that is why this is negative
                    placed = Instantiate(arrow, (lr.GetPosition(1) + directionVector / 2) + new Vector3(0,0,-0.1f),  Quaternion.LookRotation(forward: arrow.transform.forward, upwards: rotatedVectorToEnd), gameObject.transform); 
                    //the odd naming of this variable is because this is the only way to tell if the belt has been successfully placed
                    break;
                }
            }
        }
    }
}
