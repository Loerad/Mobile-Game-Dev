using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    public Vector3 inputBelt;
    public Vector3 outputBelt;
    public GameObject inputBeltObject;
    public GameObject outputBeltObject;

    void LateUpdate()
    {
        if (inputBelt == outputBelt)//if the player accidentally places the belt on the same space
        {
            inputBelt = Vector3.zero;
            outputBelt = Vector3.zero;
            Destroy(inputBeltObject);
        }
        if (inputBeltObject == null)
        {
            inputBelt = Vector3.zero;
        }
        if (outputBeltObject == null)
        {
            outputBelt = Vector3.zero;
        }
    }
}
