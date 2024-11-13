using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Foundry : MonoBehaviour
{

    public Vector3 outputBelt;
    public GameObject outputBeltObject;
    public GameObject unit;
    private bool active;
    public float productionAmount = 1;
    public float productionSpeed = 4;
    public int value = 1;

    void LateUpdate()
    {
        if (outputBeltObject == null)
        {
            //if (active) { StopCoroutine(Production()); active = false;}
            
            outputBelt = Vector3.zero;
        }
        else
        {
            //if (!active) { StartCoroutine(Production()); active = true;}
        }
    }
    void Start()
    {
        StartCoroutine(Production());
    }

    IEnumerator Production()
    {
        while(true)
        {
            yield return new WaitForSecondsRealtime(productionSpeed);
            if (outputBeltObject == null ||outputBeltObject.GetComponent<Belt>().a == null) { continue; } //when the arrow is placed
            Unit u = Instantiate(unit, outputBelt + new Vector3(0,0,-1.2f), Quaternion.identity, outputBeltObject.transform).GetComponent<Unit>();
            u.origin = outputBelt;
            u.destination = outputBeltObject.GetComponent<Belt>().lr.GetPosition(1);
            u.target = outputBeltObject.GetComponent<Belt>().target;
            u.value = value;
        }
    }
}
