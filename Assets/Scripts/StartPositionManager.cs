﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class StartPositionManager : MonoBehaviour
{
    public GameObject[] helicopterPrefabs;
    public HelicopterSelector helicopterSelector;
    public float rotationSpeed = 500;
    public GameObject canvasPoseMode;
    public GameObject canvasPose;

    public GameObject canvasControl;
    public FixedJoystick leftJoystick;
    public FixedJoystick rightJoystick;
    public Material planeMaterial;
    public Material planeLineMaterial;

    private GameObject _currentHelicopter;
    private ARRaycastManager _sessionOrigin;
    private List<ARRaycastHit> _hits;

    private int _currentMode;
    private int _currentHelicopterNum;

    private bool _isInRotation;
    private Vector3 _startTouchPosition;
    private Quaternion _startRotation;

    private float _startTouchDestination;
    private Vector3 _startScale;
    private float _currentHelicopterScale = 1;
    private Quaternion _currentHelicopterRotation = Quaternion.identity;

    private void Awake()
    {
        _sessionOrigin = GetComponent<ARRaycastManager>();
        _hits = new List<ARRaycastHit>();

        helicopterSelector.onSelect += SetPositionMode;
    }

    private void Update()
    {
        switch (_currentMode)
        {
            case 0:

                break;
            case 1:
                InPositionMode();
                break;
            case 2:

                break;
        }
    }

    private void InPositionMode()
    {
        if (Input.touchCount == 1)
        {
            var touch = Input.GetTouch(0);
            var pose = GetSessionOriginRayPose(touch);

            if (touch.phase == TouchPhase.Began)
            {
                if (_isInRotation)
                {
                    _startTouchPosition = touch.position;
                    _startRotation = _currentHelicopter.GetComponent<HelicopterControl>().pivotRotation.localRotation;
                }
                else
                {
                    SetHelicopterPose(pose);
                }
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                if (_isInRotation)
                {
                    _currentHelicopter.GetComponent<HelicopterControl>().pivotRotation.localRotation = _startRotation;
                    _currentHelicopter.GetComponent<HelicopterControl>().pivotRotation.Rotate(Vector3.up,
                        (_startTouchPosition.x - touch.position.x) * rotationSpeed / Screen.width);
                }
                else
                {
                    SetHelicopterPose(pose);
                }
            }
        }
        else if (_isInRotation && Input.touchCount == 2 && _currentHelicopter != null)
        {
            var touchOne = Input.GetTouch(0);
            var touchTwo = Input.GetTouch(1);

            if (touchTwo.phase == TouchPhase.Began)
            {
                _startTouchDestination = Vector2.Distance(touchOne.position, touchTwo.position);
                _startScale = _currentHelicopter.transform.localScale;
            }
            else if (touchOne.phase == TouchPhase.Moved || touchTwo.phase == TouchPhase.Moved)
            {
                _currentHelicopter.transform.localScale = _startScale + Vector3.one *
                    (Vector2.Distance(touchOne.position, touchTwo.position) -
                     _startTouchDestination) / Screen.width;
            }
        }
    }

    private Pose GetSessionOriginRayPose(Touch touch)
    {
        return _sessionOrigin.Raycast(touch.position, _hits, TrackableType.PlaneWithinPolygon)
            ? _hits[0].pose
            : Pose.identity;
    }

    private void SetHelicopterPose(Pose pose)
    {
        if (pose == Pose.identity) return;

        if (_currentHelicopter == null)
        {
            _currentHelicopter =
                Instantiate(helicopterPrefabs[_currentHelicopterNum], pose.position, Quaternion.identity);
            _currentHelicopter.transform.localScale = Vector3.one * _currentHelicopterScale;
            _currentHelicopter.GetComponent<HelicopterControl>().pivotRotation.localRotation =
                _currentHelicopterRotation;

            canvasPose.SetActive(true);
            _isInRotation = true;
        }
        else
        {
            _currentHelicopter.transform.position = pose.position;
            _currentHelicopter.transform.rotation = Quaternion.identity;
        }
    }

    private void SetPositionMode(int currentHelicopterNum)
    {
        _currentHelicopterNum = currentHelicopterNum;
        _currentMode = 1;
        helicopterSelector.transform.parent.parent.gameObject.SetActive(false);
        canvasPoseMode.SetActive(true);
    }

    public void SetMovement()
    {
        _isInRotation = false;
        canvasPose.SetActive(false);
        canvasPoseMode.SetActive(true);
        canvasControl.SetActive(false);

        DestroyHelicopter();
    }

    public void SetControlMode()
    {
        _currentMode = 2;
        _isInRotation = false;
        canvasPose.SetActive(false);
        canvasPoseMode.SetActive(false);
        canvasControl.SetActive(true);
        planeMaterial.color = new Color(1, 1, 1, 0);
        planeLineMaterial.color = new Color(0.5f, 0.5f, 0.5f, 0);

        _currentHelicopter.GetComponent<HelicopterControl>().leftJoystick = leftJoystick;
        _currentHelicopter.GetComponent<HelicopterControl>().rightJoystick = rightJoystick;
    }

    public void SetSelectMode()
    {
        _currentMode = 0;
        _isInRotation = false;
        canvasPose.SetActive(false);
        canvasPoseMode.SetActive(false);
        canvasControl.SetActive(false);
        helicopterSelector.transform.parent.parent.gameObject.SetActive(true);
        planeMaterial.color = new Color(1, 1, 1, 0.2f);
        planeLineMaterial.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);

        DestroyHelicopter();
    }

    private void DestroyHelicopter()
    {
        if (_currentHelicopter != null)
        {
            _currentHelicopterScale = _currentHelicopter.transform.localScale.x;
            _currentHelicopterRotation =
                _currentHelicopter.GetComponent<HelicopterControl>().pivotRotation.localRotation;
            Destroy(_currentHelicopter);
        }
    }
}