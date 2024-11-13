using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Factory : MonoBehaviour
{
    public List<Unit> units = new List<Unit>();
    public TextMeshPro unitCount;
    public Unit unit;
    private float productionSpeed = 2;
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

    public void Intake(Unit newUnit)
    {
        if (units.Count >= 10)
        {
            Destroy(newUnit.gameObject);
            return;
        }
        units.Add(newUnit);
        unitCount.text = units.Count.ToString();
        if (units.Count > 1)
        {
            StartCoroutine(Production());
        }
        else
        {
            StopCoroutine(Production());
        }
    }

    private IEnumerator Production()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(productionSpeed);
            if (units.Count > 1 && outputBeltObject == null || outputBeltObject.GetComponent<Belt>().a == null ) { continue; } //when the arrow is placed
            int fabUnitValue;
            fabUnitValue = AddValues(units[0], units[1]);
            Destroy(units[1].gameObject);
            units.Remove(units[1]);//remove the added values
            Destroy(units[0].gameObject);
            units.Remove(units[0]);

            Unit u = Instantiate(unit, outputBelt + new Vector3(0, 0, -1.2f), Quaternion.identity, outputBeltObject.transform).GetComponent<Unit>();
            u.origin = outputBelt;
            u.destination = outputBeltObject.GetComponent<Belt>().lr.GetPosition(1);
            u.target = outputBeltObject.GetComponent<Belt>().target;
            u.value = fabUnitValue;
            unitCount.text = units.Count.ToString();
        }
    }
    private int AddValues(Unit unit1, Unit unit2)
    {
        int sum = unit1.value + unit2.value;
        return sum;
    }
}
