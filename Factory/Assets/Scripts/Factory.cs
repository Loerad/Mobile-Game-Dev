using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

//This script is attached to the factories. It adds, subtracts, multiplies, or divides the units that are inputted then returns the result.
public class Factory : MonoBehaviour
{
    
    public Unit intakeUnit1;
    public Unit intakeUnit2;
    [SerializeField]
    private TextMeshPro intakeNumbers;
    public FactoryType factoryType;
    public Unit unit;
    public SpriteRenderer factoryIcon;

    [Header("Production")]
    private float productionSpeed = 2;
    private bool safeToProduce;
    private float productionPercent;
    public GameObject progressBar;
    private bool producing;

    [Header("Input")]
    public List<Vector3> inputBelt = new List<Vector3>();
    public List<GameObject> inputBeltObject = new List<GameObject>();
    [Header("Output")]
    public Vector3 outputBelt;
    public GameObject outputBeltObject;
    [Header("Indicators")]
    public List<GameObject> indicators = new List<GameObject>(); //0 is first belt indicator, 1 is second, 2 is output //this is set in editor and should not have anything added or removed
    private bool indicator0State, indicator1State, indicator2State;
    private Color32 indicatorOn = new Color32(255, 254, 0, 255);
    private Color32 indicatorOff = new Color32(130, 130, 130, 255);

    void Start()
    {
        inputBelt.Capacity = 2;
        inputBeltObject.Capacity = 2;
        factoryIcon.sprite = factoryType switch
        {
            FactoryType.Add => Resources.Load<Sprite>("plus"),
            FactoryType.Minus => Resources.Load<Sprite>("minus"),
            FactoryType.Divide => Resources.Load<Sprite>("divide"),
            FactoryType.Multiply => Resources.Load<Sprite>("multiply"),
            _ => throw new Exception("The Factory does not have a type"),

        };
    }

    void Update()
    {
        if (producing)
        {
            ProgressBar();
        }
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
            if (indicator0State) //no, turn off the indicator
            {
                indicator0State = false;
                ChangeIndicator(indicators[0], indicator0State);
            }
            inputBelt[0] = Vector3.zero;
        }
        else 
        {
            if (!indicator0State) //yes, turn on the indicator
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
            if (outputBeltObject.GetComponent<Belt>().placed != null) //when the belt is connected successfully the arrow gets placed, which is what the name of the variable "placed" is
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
        producing = true; //this is for the progress bar because I cannot figure out a way to access the yield value and use that instead
        yield return new WaitForSecondsRealtime(productionSpeed);
        producing = false;

        productionPercent = 0; //production has stopped, reset the progress bar
        progressBar.transform.localScale = new Vector3(0, progressBar.transform.localScale.y, progressBar.transform.localScale.z);

        //do the calc for the value attached to the new unit
        int fabUnitValue;
        fabUnitValue = ResultBasedOnEnum(intakeUnit1.value, intakeUnit2.value);
        if (fabUnitValue > 100) { fabUnitValue = 99; } //limiter
        Destroy(intakeUnit1.gameObject);
        Destroy(intakeUnit2.gameObject);
        intakeNumbers.text = "[0,0]";

        if (outputBeltObject == null) //incase you delete the belt while the factory is producing
        {
            yield break;
        }

        Unit u = Instantiate(unit, outputBelt + new Vector3(0, 0, -1.2f), Quaternion.identity, outputBeltObject.transform).GetComponent<Unit>(); //spit out the new unit
        u.origin = outputBelt;
        u.destination = outputBeltObject.GetComponent<Belt>().lr.GetPosition(1);
        u.target = outputBeltObject.GetComponent<Belt>().target;
        u.belt = outputBeltObject;
        u.value = fabUnitValue;
    }
    public void RemoveFactoryNumbers() //used by the delete button
    { 
        StopCoroutine(Production());
        intakeUnit1 = null;
        intakeUnit2 = null;
        intakeNumbers.text = "[0,0]";
    }

    private void ProgressBar()
    {
        productionPercent += Time.deltaTime;
        float barXValue = productionPercent / productionSpeed; 
        progressBar.transform.localScale = new Vector3(barXValue, progressBar.transform.localScale.y, progressBar.transform.localScale.z);
    }

    private int ResultBasedOnEnum(int u1, int u2)
    {
        int result = factoryType switch
        {
            FactoryType.Add => AddValues(u1, u2),
            FactoryType.Minus => SubtractValues(u1, u2),
            FactoryType.Divide => DivideValues(u1, u2),
            FactoryType.Multiply => MultiplyValues(u1, u2),
            _ => throw new Exception("The Factory does not have a type"),

        };
        return result;
    }
    private int AddValues(int a, int b)
    {
        int sum = a + b;
        return sum;
    }
    private int SubtractValues(int a, int b)
    {
        if (a < b)
        {
            return b - a;
        }
        else if (a > b)
        {
            return a - b;
        }
        else
        {
            return 1;
        }
    }
    private int DivideValues(int a, int b)
    {
        if (a < b)
        {
            return b / a;
        }
        else if(a > b)
        {
            return a / b;
        }
        else if (a == 0 || b == 0) //error prevention
        {
            return 1;
        }
        else 
        {
            return 1;
        }
    }
    private int MultiplyValues(int a, int b)
    {
        return a * b;
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
