﻿using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    public float RotationSpeed = 150f; // 카메라와 회전 속도와 같아야 한다.

    private float _rotationX = 0;
    private void Update()
    {
        if (GameManager.Instance.CurrentGameMove != GameMode.Run)
        {
            return;
        }
        float mouseX = Input.GetAxis("Mouse X");

        _rotationX += mouseX * RotationSpeed * Time.deltaTime;

        transform.eulerAngles = new Vector3(0, _rotationX, 0);
    }
}
