using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlaceOnPlane : MonoBehaviour
{
    public GameObject prefabToPlace;
    private ARRaycastManager _sessionOrigin;
    private List<ARRaycastHit> _hits;

    private void Start()
    {
        _sessionOrigin = GetComponent<ARRaycastManager>();
        _hits = new List<ARRaycastHit>();
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (_sessionOrigin.Raycast(touch.position, _hits, TrackableType.PlaneWithinPolygon))
                {
                    var pose = _hits[0].pose;
                    Instantiate(prefabToPlace, pose.position, pose.rotation);
                }
            }
        }
    }
}
