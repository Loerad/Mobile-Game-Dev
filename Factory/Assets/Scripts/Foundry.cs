using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Foundry : MonoBehaviour
{
    public GameObject unit;
    public TextMeshPro valueText;

    [Header("Output")]
    public Vector3 outputBelt;
    public GameObject outputBeltObject;

    [Header("Production")]
    private float productionSpeed = 4;
    private float productionPercent;
    public GameObject progressBar;
    private bool producing;
    public int value = 1;

    [Header("Indicator")]
    public GameObject indicator;
    private bool indicatorState;
    private Color32 indicatorOn = new Color32(255, 254, 0, 255);
    private Color32 indicatorOff = new Color32(130, 130, 130, 255);
    private bool safeToProduce;

    void Start()
    {
        safeToProduce = true;
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
        if (outputBeltObject == null)
        {
            if (indicatorState) 
            {
                indicatorState = false;
                ChangeIndicator(indicator, indicatorState);
            }
            outputBelt = Vector3.zero;
        }
        else
        {
            if (outputBeltObject.GetComponent<Belt>().placed != null) //when the belt is connected the arrow gets placed
            {
                if (!indicatorState)
                {
                    indicatorState = true;
                    ChangeIndicator(indicator, indicatorState);
                }
                if (safeToProduce)
                {
                    StartCoroutine(Production());
                }
            }
        }
    }

    IEnumerator Production()
    {
        //this works identically to the factory production coroutine. Needs to be abstracted into a single base class they draw from later
        safeToProduce = false;
        producing = true;
        yield return new WaitForSecondsRealtime(productionSpeed);
        producing = false;

        productionPercent = 0;
        progressBar.transform.localScale = new Vector3(0, progressBar.transform.localScale.y, progressBar.transform.localScale.z);
        safeToProduce = true;
        
        if (outputBeltObject == null)
        {
            yield break;
        }
        
        Unit u = Instantiate(unit, outputBelt + new Vector3(0,0,-1.2f), Quaternion.identity, outputBeltObject.transform).GetComponent<Unit>();
        u.origin = outputBelt;
        u.destination = outputBeltObject.GetComponent<Belt>().lr.GetPosition(1);
        u.target = outputBeltObject.GetComponent<Belt>().target;
        u.belt = outputBeltObject;
        u.value = value;

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

    private void ProgressBar()
    {
        productionPercent += Time.deltaTime;
        float barXValue = productionPercent / productionSpeed; 
        progressBar.transform.localScale = new Vector3(barXValue, progressBar.transform.localScale.y, progressBar.transform.localScale.z);
    }
}
