using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelicopterControl : MonoBehaviour
{
    public FixedJoystick leftJoystick;
    public FixedJoystick rightJoystick;

    public Transform pivotRotation;

    public Transform rotor;
    public Transform rotorBack;

    private float _xAngleRotation;
    private float _yAngleRotation;
    private float _zAngleRotation;

    private float _xTranslate;
    private float _yTranslate;
    private float _zTranslate;


    private void Update()
    {
        if (leftJoystick != null && rightJoystick != null)
        {
            _zAngleRotation = Mathf.Lerp(_zAngleRotation, -leftJoystick.Vertical * 45f, Time.deltaTime);
            _yAngleRotation = Mathf.Lerp(_yAngleRotation, -rightJoystick.Horizontal / 2f, Time.deltaTime);
            _xAngleRotation = Mathf.Lerp(_xAngleRotation, -leftJoystick.Horizontal * 45f, Time.deltaTime);

            _xTranslate = Mathf.Lerp(_xTranslate, -leftJoystick.Horizontal / 50f, Time.deltaTime);
            _yTranslate = Mathf.Lerp(_yTranslate, rightJoystick.Vertical / 100f, Time.deltaTime);
            _zTranslate = Mathf.Lerp(_zTranslate, leftJoystick.Vertical / 50f, Time.deltaTime);

            pivotRotation.localRotation =
                Quaternion.Euler(_xAngleRotation, pivotRotation.localRotation.eulerAngles.y, _zAngleRotation);

            pivotRotation.Rotate(Vector3.up, _yAngleRotation);

            var right = pivotRotation.right;
            var forward = pivotRotation.forward;
            transform.Translate(new Vector3(right.x, 0, right.z) * _zTranslate +
                                pivotRotation.up * _yTranslate +
                                new Vector3(forward.x, 0, forward.z) * _xTranslate);

            rotor.transform.Rotate(Vector3.up, 100);
            rotorBack.transform.Rotate(Vector3.forward, 100);
        }
    }
}