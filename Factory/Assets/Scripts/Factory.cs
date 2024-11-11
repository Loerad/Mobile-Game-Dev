using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    public List<Vector3> inputBelt = new List<Vector3>();
    public Vector3 outputBelt;
    public List<GameObject> inputBeltObject = new List<GameObject>();
    public GameObject outputBeltObject;
    void Awake()
    {
        inputBelt.Capacity = 2;
        inputBeltObject.Capacity = 2;
    }
    void LateUpdate()
    {
        if (inputBeltObject[0] == outputBeltObject)//if the player accidentally places the belt on the same space
        {
            inputBelt[0] = Vector3.zero;
            outputBelt = Vector3.zero;
            Destroy(inputBeltObject[0]);
        }
        if (inputBeltObject[1] == outputBeltObject)
        {
            inputBelt[1] = Vector3.zero;
            outputBelt = Vector3.zero;
            Destroy(inputBeltObject[1]);
        }
        if (inputBeltObject[0] == null)
        {
            inputBelt[0] = Vector3.zero;
        }
        if (inputBeltObject[1] == null)
        {
            inputBelt[1] = Vector3.zero;
        }
        if (outputBeltObject == null)
        {
            outputBelt = Vector3.zero;
        }
    }
}
