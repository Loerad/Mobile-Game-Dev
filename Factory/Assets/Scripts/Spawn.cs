using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Spawn : MonoBehaviour
{
    [HideInInspector]
    public Vector3 touchPosition;
    private StyleColor active = new StyleColor(new Color32(0,255,0,255));
    private StyleColor inactive = new StyleColor(new Color32(100,100,100,255));
    private List<Button> mainButtons = new List<Button>();
    private List<Button> subButtons = new List<Button>();
    [Header("Factory")]
    public GameObject factory;
    private Button factoryButton;
    private int factoryCount = 10;
    private FactoryType factoryType;
    private VisualElement factoryChoice;
    private Button plusButton;
    private Button minusButton;
    private Button divideButton;
    private Button multiplyButton;

    [Header("Belt")]

    public GameObject belt;
    private Button deleteButton;
    [Header("Foundry")]
    public GameObject foundry;
    private Button foundryButton;
    private int foundryCount = 10;
    void Awake()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        factoryChoice = root.Q<VisualElement>("FactoryChoice");
        factoryChoice.visible = false;
        factoryButton = root.Q<Button>("FactoryButton");
        factoryButton.RegisterCallback<ClickEvent>(ev => 
            { 
                State.mode = Mode.Factory; 
                SwitchButton(factoryButton, false);
                factoryChoice.visible = true;
            } 
        );
        deleteButton = root.Q<Button>("DeleteButton");
        deleteButton.RegisterCallback<ClickEvent>(ev => 
            { 
                State.mode = Mode.Delete; 
                SwitchButton(deleteButton, false);
            } 
        );
        foundryButton = root.Q<Button>("FoundryButton");
        foundryButton.RegisterCallback<ClickEvent>(ev => 
            { 
                State.mode = Mode.Foundry; 
                SwitchButton(foundryButton, false);
            } 
        );
        factoryButton.text = $"Factories:\n{factoryCount}";
        deleteButton.text = "Delete";
        foundryButton.text = $"Foundries:\n{foundryCount}";

        plusButton = root.Q<Button>("Plus");
        plusButton.style.unityBackgroundImageTintColor = active;
        plusButton.RegisterCallback<ClickEvent>(ev =>
        {
            factoryType = FactoryType.Add;
            SwitchButton(plusButton, true);
            factoryChoice.visible = false;
        });

        minusButton = root.Q<Button>("Minus");
        minusButton.RegisterCallback<ClickEvent>(ev =>
        {
            factoryType = FactoryType.Minus;
            SwitchButton(minusButton, true);
            factoryChoice.visible = false;
        });

        divideButton = root.Q<Button>("Divide");
        divideButton.RegisterCallback<ClickEvent>(ev =>
        {
            factoryType = FactoryType.Divide;
            SwitchButton(divideButton, true);
            factoryChoice.visible = false;
        });

        multiplyButton = root.Q<Button>("Multiply");
        multiplyButton.RegisterCallback<ClickEvent>(ev =>
        {
            factoryType = FactoryType.Multiply;
            SwitchButton(multiplyButton, true);
            factoryChoice.visible = false;
        });

        mainButtons.Add(factoryButton); //this is gross but I can't think of another way :P
        mainButtons.Add(deleteButton);
        mainButtons.Add(factoryButton);
        subButtons.Add(divideButton);
        subButtons.Add(minusButton);
        subButtons.Add(multiplyButton);
        subButtons.Add(plusButton);
    }

    void SwitchButton(Button button, bool sub)
    {
        if (sub)
        {
            foreach (Button b in subButtons)
            {
                if (b == button)
                {
                    b.style.unityBackgroundImageTintColor = active;
                }
                else
                {
                    b.style.unityBackgroundImageTintColor = inactive;
                }
            }
        }
        else
        {
            foreach (Button b in mainButtons)
            {
                if (b == button)
                {
                    b.style.backgroundColor = active;
                }
                else
                {
                    b.style.backgroundColor = inactive;
                }
            }
        }
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
                                    if (hit.collider.gameObject.CompareTag("Factory"))
                                    {
                                        DrawLineFromFactory(hit);
                                    }
                                    else if (hit.collider.gameObject.CompareTag("Foundry"))
                                    {
                                        DrawLineFromFoundry(hit);
                                    }
                                    else if (hit.collider.gameObject.CompareTag("Finish"))
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
                                        Factory f = Instantiate(factory, touchPosition, Quaternion.identity).GetComponent<Factory>();
                                        f.factoryType = factoryType;
                                        factoryChoice.visible = false;
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
                                    if (hit.collider.gameObject.CompareTag("Foundry"))
                                    {
                                        Foundry f = hit.collider.gameObject.GetComponent<Foundry>();
                                        if (f.outputBeltObject != null)
                                        {
                                            if (f.outputBeltObject.GetComponent<Belt>().placed != null)
                                            {
                                                if (foundryCount <= 0)
                                                {
                                                    return;
                                                }                                               
                                                f.value++;
                                                f.valueText.text = f.value.ToString();
                                                foundryCount--; 
                                                foundryButton.text = $"Foundries:\n{foundryCount}";
                                            }
                                        }
                                        else
                                        {
                                            DrawLineFromFoundry(hit);                                            
                                        }
                                    }
                                    else if (hit.collider.gameObject.CompareTag("Factory"))
                                    {
                                        DrawLineFromFactory(hit);
                                    }
                                    else if (hit.collider.gameObject.CompareTag("Finish"))
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
                                    Factory target = hit.collider.gameObject.GetComponent<Factory>();
                                    factoryCount++;
                                    Destroy(target.outputBeltObject);
                                    Destroy(target.inputBeltObject[0]);
                                    Destroy(target.inputBeltObject[1]);
                                    Destroy(target.gameObject);
                                    factoryButton.text = $"Factories:\n{factoryCount}";
                                }
                                if (hit.collider.gameObject.CompareTag("Foundry"))
                                {
                                    Foundry target = hit.collider.gameObject.GetComponent<Foundry>();
                                    foundryCount += target.value;
                                    Destroy(target.outputBeltObject); //foundries only give out one output and do not take inputs unlike factories
                                    Destroy(target.gameObject);
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

    void DrawLineFromFactory(RaycastHit2D hit)
    {
        GameObject target = hit.collider.gameObject;
        Vector3 outputPoint = target.transform.position;
        GameObject currentbelt = Instantiate(belt, outputPoint, Quaternion.identity);

        if (target.GetComponent<Factory>().outputBelt != Vector3.zero) //do I already have a output belt?
        { Destroy(currentbelt); return; } //yes because that vector is not zero, destroy yourself now!

        currentbelt.GetComponent<LineRenderer>().SetPosition(0, outputPoint);
        target.GetComponent<Factory>().outputBelt = currentbelt.GetComponent<LineRenderer>().GetPosition(0);
        target.GetComponent<Factory>().outputBeltObject = currentbelt;
    }

    void DrawLineFromFoundry(RaycastHit2D hit)
    {
        GameObject target = hit.collider.gameObject;
        Vector3 outputPoint = target.transform.position;
        GameObject currentbelt = Instantiate(belt, outputPoint, Quaternion.identity);

        if (target.GetComponent<Foundry>().outputBelt != Vector3.zero) { Destroy(currentbelt); return; } //same as above

        currentbelt.GetComponent<LineRenderer>().SetPosition(0, outputPoint);
        target.GetComponent<Foundry>().outputBelt = currentbelt.GetComponent<LineRenderer>().GetPosition(0);
        target.GetComponent<Foundry>().outputBeltObject = currentbelt;
    }
}