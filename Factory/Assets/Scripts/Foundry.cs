using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foundry : MonoBehaviour
{

    public Vector3 outputBelt;
    public GameObject outputBeltObject;

    void LateUpdate()
    {
        if (outputBeltObject == null)
        {
            outputBelt = Vector3.zero;
        }
    }
}
