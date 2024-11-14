using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Factory : MonoBehaviour
{
    private Unit intakeUnit1;
    private Unit intakeUnit2;
    [SerializeField]
    private TextMeshPro intakeNumbers;
    public Unit unit;
    private float productionSpeed = 2;
    private bool safeToProduce;

    [Header("Input")]
    public List<Vector3> inputBelt = new List<Vector3>();
    public List<GameObject> inputBeltObject = new List<GameObject>();
    [Header("Output")]
    public Vector3 outputBelt;
    public GameObject outputBeltObject;
    [Header("Indicators")]
    public List<GameObject> indicators = new List<GameObject>(); //0 is first belt indicator, 1 is second, 2 is output //this is set in editor and should have anything added or removed
    private bool indicator0State, indicator1State, indicator2State;
    private Color32 indicatorOn = new Color32(255, 254, 0, 255);
    private Color32 indicatorOff = new Color32(130, 130, 130, 255);

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

        if (inputBeltObject[0] == null) //is there an input belt?
        {
            if (indicator0State) 
            {
                indicator0State = false;
                ChangeIndicator(indicators[0], indicator0State);
            }
            inputBelt[0] = Vector3.zero;
        }
        else 
        {
            if (!indicator0State)
            {
                indicator0State = true;
                ChangeIndicator(indicators[0], indicator0State);
            }
        }

        if (inputBeltObject[1] == null)
        {
            if (indicator1State) 
            {
                indicator1State = false;
                ChangeIndicator(indicators[1], indicator1State);
            }
            inputBelt[1] = Vector3.zero;
        }
        else
        {
            if (!indicator1State)
            {
                indicator1State = true;
                ChangeIndicator(indicators[1], indicator1State);
            }
        }

        if (outputBeltObject == null)
        {
            if (indicator2State) 
            {
                indicator2State = false;
                ChangeIndicator(indicators[2], indicator2State);
            }
            outputBelt = Vector3.zero;
        }
        else
        {
            if (outputBeltObject.GetComponent<Belt>().a != null) //when the belt is connected the arrow gets placed
            {
                if (!indicator2State)
                {
                    indicator2State = true;
                    ChangeIndicator(indicators[2], indicator2State);
                }
                if (safeToProduce)
                {
                    StartCoroutine(Production());
                }
            }
        }
    }

    public void Intake(Unit newUnit, GameObject owningBelt)
    {
        if (owningBelt == inputBeltObject[0]) //first belt connected
        {
            if (intakeUnit1 == null)
            {
                intakeUnit1 = newUnit;
                if (intakeUnit2 == null)
                {
                    intakeNumbers.text = $"[{intakeUnit1.value},0]";
                }
                else
                {
                    intakeNumbers.text = $"[{intakeUnit1.value},{intakeUnit2.value}]";
                    safeToProduce = true;
                }
            }
            else
            {
                Destroy(newUnit.gameObject);
                return;
            }
        }
        else if (owningBelt == inputBeltObject[1]) //second belt connected
        {
            if (intakeUnit2 == null)
            {
                intakeUnit2 = newUnit;
                if (intakeUnit1 == null)
                {
                    intakeNumbers.text = $"[0,{intakeUnit2.value}]";
                }
                else
                {
                    intakeNumbers.text = $"[{intakeUnit1.value},{intakeUnit2.value}]";
                    safeToProduce = true;
                }
            }
            else
            {
                Destroy(newUnit.gameObject);
                return;
            }
        }
    }

    private IEnumerator Production()
    {
        safeToProduce = false;
        Debug.Log($"Factory at {transform.position} has entered production");
        yield return new WaitForSecondsRealtime(productionSpeed);
        int fabUnitValue;
        fabUnitValue = AddValues(intakeUnit1, intakeUnit2);
        Destroy(intakeUnit1.gameObject);
        Destroy(intakeUnit2.gameObject);


        Unit u = Instantiate(unit, outputBelt + new Vector3(0, 0, -1.2f), Quaternion.identity, outputBeltObject.transform).GetComponent<Unit>();
        u.origin = outputBelt;
        u.destination = outputBeltObject.GetComponent<Belt>().lr.GetPosition(1);
        u.target = outputBeltObject.GetComponent<Belt>().target;
        u.belt = outputBeltObject;
        u.value = fabUnitValue;
        intakeNumbers.text = "[0,0]";
    }

    private int AddValues(Unit unit1, Unit unit2)
    {
        int sum = unit1.value + unit2.value;
        return sum;
    }

    private void ChangeIndicator(GameObject indicator, bool state)
    {
        if (state)
        { 
            indicator.GetComponent<SpriteRenderer>().color = indicatorOn;
        }
        else
        {
            indicator.GetComponent<SpriteRenderer>().color = indicatorOff;

        }
    }
}
