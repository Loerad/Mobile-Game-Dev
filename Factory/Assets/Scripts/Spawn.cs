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
    private Button beltButton;
    [Header("Foundry")]
    public GameObject foundry;
    private Button foundryButton;
    public int foundryCount = 5;
    void Awake()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        factoryButton = root.Q<Button>("FactoryButton");
        factoryButton.RegisterCallback<ClickEvent>(ev => State.mode = Mode.Factory);
        beltButton = root.Q<Button>("BeltButton");
        beltButton.RegisterCallback<ClickEvent>(ev => State.mode = Mode.Belt);
        foundryButton = root.Q<Button>("FoundryButton");
        foundryButton.RegisterCallback<ClickEvent>(ev => State.mode = Mode.Foundry);
        factoryButton.text = $"Factories:\n{factoryCount}";
        beltButton.text = "Belts";

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
                // Construct a ray from the current touch coordinates
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
                                    Destroy(hit.collider.gameObject);
                                    factoryCount++;
                                    factoryButton.text = factoryCount.ToString();
                                }
                                else
                                {
                                    if (factoryCount <= 0)
                                    {
                                        return;
                                    }
                                    factoryCount--;
                                    factoryButton.text = factoryCount.ToString();
                                    Instantiate(factory, touchPosition, Quaternion.identity);
                                } 
                            }
                        }
                        catch (System.Exception)
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
                                    Destroy(hit.collider.gameObject);
                                    foundryCount++;
                                    foundryButton.text = foundryCount.ToString();
                                }
                                else
                                {
                                    if (foundryCount <= 0)
                                    {
                                        return;
                                    }
                                    foundryCount--;
                                    foundryButton.text = foundryCount.ToString();
                                    Instantiate(foundry, touchPosition, Quaternion.identity);
                                } 
                            }
                        }
                        catch (System.Exception)
                        {
                            return; //allows ui buttons to be pressed without spawning a factory
                        }
                        break;
                    }
                    case Mode.Belt:
                    {
                        try
                        {
                            GameObject currentbelt = null;
                            switch (touch.phase)
                            {
                                case TouchPhase.Began:
                                {
                                    if (hit.collider.gameObject.CompareTag("Factory"))
                                    {
                                        GameObject target = hit.collider.gameObject;

                                        currentbelt = Instantiate(belt, target.transform.position, Quaternion.identity);

                                        if (target.GetComponent<Factory>().outputBelt != Vector3.zero) { Destroy(currentbelt); return; }
                                        currentbelt.GetComponent<LineRenderer>().SetPosition(0, target.transform.position);
                                        target.GetComponent<Factory>().outputBelt = currentbelt.GetComponent<LineRenderer>().GetPosition(0);

                                    }
                                    break;
                                }
                            }
                        }
                        catch (System.Exception)
                        {
                            return; //allows ui buttons to be pressed without spawning a factory
                        }
                        break;
                    }
                }
            }
        }
    }
}