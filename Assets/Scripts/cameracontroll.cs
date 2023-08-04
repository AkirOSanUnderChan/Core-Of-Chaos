using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class cameracontroll : MonoBehaviour
{
    public float maxTiltAngle = 20f;
    public float tiltSpeed = 5f;

    private PlayerCOntroller playerController;

    private void Start()
    {
        // Знаходимо скрипт PlayerController на гравцеві
        playerController = FindObjectOfType<PlayerCOntroller>();
    }

    private void Update()
    {
        if (playerController != null)
        {
            // Отримуємо вхідне значення осі горизонтального руху з PlayerController
            float horizontalInput = Input.GetAxis("Horizontal");

            // Обчислюємо кут нахилу камери відповідно до вхідного значення
            float targetTiltAngle = -horizontalInput * maxTiltAngle;

            // Плавно змінюємо кут нахилу камери
            float currentTiltAngle = Mathf.LerpAngle(transform.localEulerAngles.z, targetTiltAngle, Time.deltaTime * tiltSpeed);

            // Обмежуємо кут нахилу, щоб не перевищував максимальний
            currentTiltAngle = Mathf.Clamp(currentTiltAngle, -maxTiltAngle, maxTiltAngle);

            // Застосовуємо нахил камери
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, currentTiltAngle);
        }
    }
}