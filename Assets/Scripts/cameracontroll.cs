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
        // ��������� ������ PlayerController �� �������
        playerController = FindObjectOfType<PlayerCOntroller>();
    }

    private void Update()
    {
        if (playerController != null)
        {
            // �������� ������ �������� �� ��������������� ���� � PlayerController
            float horizontalInput = Input.GetAxis("Horizontal");

            // ���������� ��� ������ ������ �������� �� �������� ��������
            float targetTiltAngle = -horizontalInput * maxTiltAngle;

            // ������ ������� ��� ������ ������
            float currentTiltAngle = Mathf.LerpAngle(transform.localEulerAngles.z, targetTiltAngle, Time.deltaTime * tiltSpeed);

            // �������� ��� ������, ��� �� ����������� ������������
            currentTiltAngle = Mathf.Clamp(currentTiltAngle, -maxTiltAngle, maxTiltAngle);

            // ����������� ����� ������
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, currentTiltAngle);
        }
    }
}