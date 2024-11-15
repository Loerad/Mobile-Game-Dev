using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public GameObject belt;
    public GameObject target;
    public Vector2 origin;
    public Vector3 destination;
    public int value;
    public TextMeshPro valueGUI;
    private bool collected;
    // Start is called before the first frame update
    void Start()
    {
        valueGUI.text = value.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        float step = 1 * Time.deltaTime; //abrirtary speed, could later have some different types of belts that can move faster
        transform.position = Vector2.MoveTowards(transform.position, destination, step);
        transform.position = new Vector3(transform.position.x, transform.position.y , -1.2f);  
        if (transform.position == target.transform.position + new Vector3(0,0,-0.2f) && !collected)
        {
            if (target.CompareTag("Factory"))
            {
                target.GetComponent<Factory>().Intake(this, belt);
            }
            else //foundries cannot be targeted by belts so I do not need to check for them
            {
                target.GetComponent<Finish>().CheckWin(this);
            }
            collected = true;
        }
    }
}
