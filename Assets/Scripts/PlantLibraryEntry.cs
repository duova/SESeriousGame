using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;


public class PlantLibraryEntry : MonoBehaviour, ISelectHandler
{
    public Button button { get; private set; }
    private UnityAction onSelectAction;
    private TextMeshProUGUI buttonText;

    public void Initialize(string display, UnityAction action)
    {   
        this.button = GetComponent<Button>(); 
        this.buttonText = this.GetComponentInChildren<TextMeshProUGUI>();

        this.buttonText.text = display;
        this.onSelectAction = action;
    }

    public void OnSelect(BaseEventData eventData)
    {
        onSelectAction();
    }
}
