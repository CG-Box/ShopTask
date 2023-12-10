using UnityEngine;
using TMPro;

public class TicketsPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text ticketsText;

    void OnEnable()
    {
        StaticEvents.Tickets.UpdateUI += UpdateTicketsUI;
    }
    void OnDisable()
    {
        StaticEvents.Tickets.UpdateUI -= UpdateTicketsUI;
    }

    public void UpdateTicketsUI(int ticketsAmount)
    {
        ticketsText.text = ticketsAmount.ToString(); 
    }
}
