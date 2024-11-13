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

    void Awake()
    {
        winScreen = GetComponent<UIDocument>().rootVisualElement;
        winScreen.visible = false;
        restart = winScreen.Q<Button>("Restart");
        restart.RegisterCallback<ClickEvent>(Restart);
    }

    void Start()
    {
        winValue = Random.Range(5, 10);
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
            Time.timeScale = 0;
            winScreen.visible = true;
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
}
