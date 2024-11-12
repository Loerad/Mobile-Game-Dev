using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawn : MonoBehaviour
{
    [HideInInspector]
    public Vector3 touchPosition;
    [Header("Factory")]
    public GameObject factory;
    private Button factoryButton;
    public int factoryCount = 5;

    [Header("Belt")]

    public GameObject belt;
    private Button deleteButton;
    [Header("Foundry")]
    public GameObject foundry;
    private Button foundryButton;
    public int foundryCount = 5;
    void Awake()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        factoryButton = root.Q<Button>("FactoryButton");
        factoryButton.RegisterCallback<ClickEvent>(ev => State.mode = Mode.Factory);
        deleteButton = root.Q<Button>("DeleteButton");
        deleteButton.RegisterCallback<ClickEvent>(ev => State.mode = Mode.Delete);
        foundryButton = root.Q<Button>("FoundryButton");
        foundryButton.RegisterCallback<ClickEvent>(ev => State.mode = Mode.Foundry);
        factoryButton.text = $"Factories:\n{factoryCount}";
        deleteButton.text = "Delete";
        foundryButton.text = $"Foundries:\n{foundryCount}";

    }

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            //https://discussions.unity.com/t/best-way-to-detect-touch-on-a-gameobject/157075/2
            if (Input.touchCount > 0)
            {

                touchPosition = Camera.main.ScreenToWorldPoint(touch.position) + new Vector3(0, 0, 9);
                Vector2 touch2D = new Vector2(touchPosition.x, touchPosition.y);
                RaycastHit2D hit = Physics2D.Raycast(touch2D, Camera.main.transform.forward);


                switch (State.mode)
                {
                    case Mode.Factory:
                        {
                            try
                            {
                                if (touch.phase == TouchPhase.Began)
                                {
                                    GameObject currentbelt = null;
                                    if (hit.collider.gameObject.CompareTag("Factory"))
                                    {
                                        GameObject target = hit.collider.gameObject;
                                        Vector3 outputPoint = target.transform.position;

                                        currentbelt = Instantiate(belt, outputPoint, Quaternion.identity);

                                        if (target.GetComponent<Factory>().outputBelt != Vector3.zero) //do I already have a output belt?
                                        { Destroy(currentbelt); return; } //yes because that vector is not zero, destroy yourself now!

                                        currentbelt.GetComponent<LineRenderer>().SetPosition(0, outputPoint);
                                        target.GetComponent<Factory>().outputBelt = currentbelt.GetComponent<LineRenderer>().GetPosition(0);
                                        target.GetComponent<Factory>().outputBeltObject = currentbelt;

                                    }
                                    else if (hit.collider.gameObject.CompareTag("Foundry"))
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        if (factoryCount <= 0)
                                        {
                                            return;
                                        }
                                        factoryCount--;
                                        factoryButton.text = $"Factories:\n{factoryCount}";
                                        Instantiate(factory, touchPosition, Quaternion.identity);
                                    }
                                    break;
                                }
                            }
                            catch (System.NullReferenceException)
                            {
                                return; //allows ui buttons to be pressed without spawning a factory
                            }
                            break;
                        }
                    case Mode.Foundry:
                        {
                            try
                            {
                                if (touch.phase == TouchPhase.Began)
                                {
                                    GameObject currentbelt = null;
                                    if (hit.collider.gameObject.CompareTag("Foundry"))
                                    {
                                        GameObject target = hit.collider.gameObject;
                                        Vector3 outputPoint = target.transform.position;

                                        currentbelt = Instantiate(belt, outputPoint, Quaternion.identity);

                                        if (target.GetComponent<Foundry>().outputBelt != Vector3.zero) { Destroy(currentbelt); return; } //same as above

                                        currentbelt.GetComponent<LineRenderer>().SetPosition(0, outputPoint);
                                        target.GetComponent<Foundry>().outputBelt = currentbelt.GetComponent<LineRenderer>().GetPosition(0);
                                        target.GetComponent<Foundry>().outputBeltObject = currentbelt;
                                    }
                                    else if (hit.collider.gameObject.CompareTag("Factory"))
                                    {
                                        return;
                                    }
                                    else
                                    {
                                        if (foundryCount <= 0)
                                        {
                                            return;
                                        }
                                        foundryCount--;
                                        foundryButton.text = $"Foundries:\n{foundryCount}";
                                        Instantiate(foundry, touchPosition, Quaternion.identity);
                                    }
                                }
                            }
                            catch (System.NullReferenceException)
                            {
                                return; //allows ui buttons to be pressed without spawning a factory
                            }
                            break;
                        }
                    case Mode.Delete:
                        {
                            try
                            {
                                if (hit.collider.gameObject.CompareTag("Factory"))
                                {
                                    GameObject target = hit.collider.gameObject;
                                    Destroy(target.GetComponent<Factory>().outputBeltObject);
                                    Destroy(target.GetComponent<Factory>().inputBeltObject[0]);
                                    Destroy(target.GetComponent<Factory>().inputBeltObject[1]);
                                    Destroy(target);
                                    factoryCount++;
                                    factoryButton.text = $"Factories:\n{factoryCount}";
                                }
                                if (hit.collider.gameObject.CompareTag("Foundry"))
                                {
                                    GameObject target = hit.collider.gameObject;
                                    Destroy(target.GetComponent<Foundry>().outputBeltObject); //foundries only give out one output and do not take inputs unlike factories
                                    Destroy(target);
                                    foundryCount++;
                                    foundryButton.text = $"Foundries:\n{foundryCount}";
                                }
                                break;
                            }
                            catch (System.NullReferenceException)
                            {
                                return; //allows ui buttons to be pressed without spawning a factory
                            }

                        }
                }
            }
        }
    }
}