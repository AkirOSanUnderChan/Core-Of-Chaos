using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI tooltipText;
    public GameObject tooltipWindow;

    private void Start()
    {
        tooltipWindow.SetActive(false);
    }

    private void Update()
    {
        if (tooltipWindow.activeSelf)
        {
            Vector3 tooltipPosition = Input.mousePosition + new Vector3(10f, 10f, 0f);
            tooltipWindow.transform.position = tooltipPosition;
        }
    }

    public void OnPointerEnter()
    {
        tooltipText.text = "Тут може бути ваша інформація"; // Замініть це повідомлення на свою інформацію
        tooltipWindow.SetActive(true);
    }

    public void OnPointerExit()
    {
        tooltipWindow.SetActive(false);
    }
}
