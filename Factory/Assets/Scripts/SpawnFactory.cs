using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnFactory : MonoBehaviour
{
    public int factoryCount = 5;
    public GameObject factory;
    public LineRenderer belt;
    private Vector3 touchPosition;
    private Button factoryButton;
    private bool factorySelected = true;
    private Button beltButton;
    void Awake()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        factoryButton = root.Q<Button>("FactoryButton");
        factoryButton.RegisterCallback<ClickEvent>(ev => factorySelected = true);
        beltButton = root.Q<Button>("BeltButton");
        beltButton.RegisterCallback<ClickEvent>(ev => factorySelected = false);
        factoryButton.text = factoryCount.ToString();
    }

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            Debug.Log("Touching at: " + touch.position);
            //https://discussions.unity.com/t/best-way-to-detect-touch-on-a-gameobject/157075/2
            if (Input.touchCount > 0)
            {
                touchPosition = Camera.main.ScreenToWorldPoint(touch.position) + new Vector3(0, 0, 9);
                Vector2 touch2D = new Vector2(touchPosition.x, touchPosition.y);
                RaycastHit2D hit = Physics2D.Raycast(touch2D, Camera.main.transform.forward);
                // Construct a ray from the current touch coordinates
                if (factorySelected)
                {
                    try
                    {
                        if (hit.collider.gameObject.CompareTag("Factory"))
                        {
                            Debug.Log("Touched the factory");
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
                    catch (System.Exception)
                    {
                        return; //allows ui buttons to be pressed without spawning a factory
                    }
                }
                else
                {
                    try
                    {
                        switch (touch.phase)
                        {
                            case TouchPhase.Began:
                                {
                                    if (hit.collider.gameObject.CompareTag("Factory"))
                                    {
                                        belt.SetPosition(0, hit.collider.gameObject.transform.position);
                                    }
                                    break;
                                }
                            case TouchPhase.Moved:
                            {
                                belt.SetPosition(1, touchPosition);
                                break;
                            }
                            case TouchPhase.Stationary:
                            {
                                belt.SetPosition(1, touchPosition);
                                break;
                            }
                            case TouchPhase.Ended:
                            {
                                if (hit.collider.gameObject.CompareTag("Factory"))
                                {
                                    belt.SetPosition(1, hit.collider.gameObject.transform.position);
                                }
                                else
                                {

                                }

                                break;
                            }
                        }
                    }
                    catch (System.Exception)
                    {
                        return; //allows ui buttons to be pressed without spawning a factory
                    }
                }
            }
        }
    }
}