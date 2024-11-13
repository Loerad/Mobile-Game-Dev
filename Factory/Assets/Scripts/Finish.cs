using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Finish : MonoBehaviour
{
    public Vector3 inputBelt;
    public GameObject inputBeltObject;
    public int winValue;
    public TextMeshPro winAmount;

    void Start()
    {
        winValue = Random.Range(5, 10);
        winAmount.text = winValue.ToString();
    }
    public void CheckWin(Unit unit)
    {
        if (unit.value == winValue)
        {
            //win!!
        }
        else
        {
            Destroy(unit.gameObject);
        }
    }
}
