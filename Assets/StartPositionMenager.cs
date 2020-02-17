using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class StartPositionMenager : MonoBehaviour
{
    public GameObject[] helicopterPrefabs;
    public HelicopterSelector helicopterSelector;

    private GameObject _currentHelicopter;
    private ARRaycastManager _sessionOrigin;
    private List<ARRaycastHit> _hits;

    private int _currentMode;
    private int _currentHelicopterNum;

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
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (_sessionOrigin.Raycast(touch.position, _hits, TrackableType.PlaneWithinPolygon))
                {
                    var pose = _hits[0].pose;

                    if (_currentHelicopter == null)
                    {
                        _currentHelicopter = Instantiate(helicopterPrefabs[_currentHelicopterNum], pose.position,
                            pose.rotation);
                    }
                    else
                    {
                        _currentHelicopter.transform.position = pose.position;
                    }
                }
            }
        }
    }

    private void SetPositionMode(int currentHelicopterNum)
    {
        _currentHelicopterNum = currentHelicopterNum;
        _currentMode = 1;
        helicopterSelector.transform.parent.parent.gameObject.SetActive(false);
    }
}