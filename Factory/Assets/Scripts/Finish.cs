using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class Finish : MonoBehaviour
{
    public Vector3 inputBelt;
    public GameObject inputBeltObject;
    public int winValue;
    public TextMeshPro winAmount;
    private VisualElement winScreen;
    private Button restart;
    public List<GameObject> indicators = new List<GameObject>(); //0 is first belt indicator, 1 is second, 2 is output //this is set in editor and should not have anything added or removed
    private bool indicator0State, indicator1State, indicator2State;
    private Color32 indicatorOn = new Color32(255, 254, 0, 255);
    private Color32 indicatorOff = new Color32(130, 130, 130, 255);
    public List<Unit> winningUnits = new List<Unit>();

    void Awake()
    {
        winScreen = GetComponent<UIDocument>().rootVisualElement;
        winScreen.visible = false;
        restart = winScreen.Q<Button>("Restart");
        restart.RegisterCallback<ClickEvent>(Restart);
    }

    void Start()
    {
        winValue = Random.Range(23, 74); //arbitrary range, can be set by a difficulty setting or just changed to whatever you want under 99 and above 5 (The highest value factories can create and the highest level foundry)
        winAmount.text = winValue.ToString();
    }

    void LateUpdate()
    {
        if (inputBeltObject == null)
        {
            inputBelt = Vector3.zero;
        }

    }
    public void CheckWin(Unit unit)
    {
        if (unit.value == winValue)
        {
            if (winningUnits[0] == null) //is there a unit here?
            {
                winningUnits[0] = unit; //no, put the unit here
                CheckStates();
            }
            else if (winningUnits[1] == null)
            {
                winningUnits[1] = unit;
                CheckStates();
            }
            else if (winningUnits[2] == null)
            {
                winningUnits[2] = unit;
                CheckStates();
            }
            else //if all are full
            {
                Destroy(unit.gameObject);
            }
        }
        else
        {
            Destroy(unit.gameObject);
        }
    }

    private void Restart(ClickEvent click)
    {
        SceneManager.LoadScene("MainScene");
    }

    void CheckStates()
    {
        if (winningUnits[2] != null && !indicator2State) //has the final slot been filled?
        {
            indicator2State = true; //win the game
            ChangeIndicator(indicators[2], indicator2State); //this is just set to look good as the win screen is transparent
            winScreen.visible = true;
            Time.timeScale = 0;
        }
        if (winningUnits[1] != null && !indicator1State)
        {
            indicator1State = true;
            ChangeIndicator(indicators[1], indicator1State);
            
        }
        if (winningUnits[0] != null && !indicator0State)
        {
            indicator0State = true;
            ChangeIndicator(indicators[0], indicator0State);
           
        }

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
