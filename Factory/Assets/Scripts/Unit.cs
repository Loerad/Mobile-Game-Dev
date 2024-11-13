using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public GameObject belt;
    public Vector2 origin;
    public Vector3 destination;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float step = 1 * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, destination, step);
        transform.position = new Vector3(transform.position.x, transform.position.y , -1.2f);
        if (transform.position == destination)
        {
            Destroy(gameObject);
            //belt.GetComponent<Belt>().target.GetComponent<Factory>()
        }
    }
}
