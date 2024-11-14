using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Help : MonoBehaviour
{
    private VisualElement helpSection;
    private Button helpButton;
    private Label helpText;
    private string description = "How to play:\n\n"+
    "The goal is to output the same number into the green box by connecting foundries to factories that add, subtract, multiply and divide.\n\n"+
    "To connect foundries and factories, after placing, tap and drag to start a belt. \nIf you finish the belt on a building that has a input you should see the indicator light up.\n\n"+
    "The first belt you connect will light up the left indicator, no matter which side you connect it from, and will only put numbers into the first slot. This is the same for the second slot\n\n"+
    "You can upgrade foundries by tapping on them again after connecting it's output belt to a factory. Upgrading costs the same as placing foundry.\n\n"+
    "In delete mode, if you tap a factory with numbers inside that is not currently producing a new number, it will clear those numbers before deleting, requiring you to press again if you want to delete the factory.\n\n"+
    "To delete belts you must delete either one of the connected buildings."+
    "The lowest a number can be is 1.\n\n";

    void Awake()
    {
        VisualElement doc = GetComponent<UIDocument>().rootVisualElement;
        helpSection = doc.Q<VisualElement>("HelpPanel");
        helpSection.visible = false;
        helpButton = doc.Q<Button>("HelpButton");
        helpButton.RegisterCallback<ClickEvent>(HelpPopUp);    
        helpText = doc.Q<Label>("HelpText");
        helpText.text = description;
    }

    void HelpPopUp(ClickEvent click)
    {
        helpSection.visible = !helpSection.visible;
    }
}
