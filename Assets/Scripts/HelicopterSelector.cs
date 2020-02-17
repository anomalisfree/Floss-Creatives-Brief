using UnityEngine;
using UnityEngine.UI;

public class HelicopterSelector : MonoBehaviour
{
    public float rotationSpeed = 500;
    public string[] helicoptersName;
    public Text currentHelicopterName;
    public int currentHelicopter;

    private Quaternion _startRotation;
    private Vector3 _startTouchPosition;
    private float _targetAngle;

    private void Start()
    {
        SelectHelicopter(0);
    }

    private void Update()
    {
        if (Input.touchCount == 1)
        {
            var touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                _startTouchPosition = touch.position;
                _startRotation = transform.localRotation;
            }

            if (touch.phase == TouchPhase.Moved)
            {
                transform.localRotation = _startRotation;
                transform.Rotate(Vector3.up, (_startTouchPosition.x - touch.position.x) * rotationSpeed / Screen.width);

                if ((transform.localRotation.eulerAngles.y >= 0 && transform.localRotation.eulerAngles.y < 60) ||
                    transform.localRotation.eulerAngles.y > 300)
                {
                    SelectHelicopter(0);
                }
                else if ((transform.localRotation.eulerAngles.y >= 60 && transform.localRotation.eulerAngles.y < 180))
                {
                    SelectHelicopter(1);
                }
                else if ((transform.localRotation.eulerAngles.y >= 180 && transform.localRotation.eulerAngles.y < 300))
                {
                    SelectHelicopter(2);
                }
            }
        }
        else
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(0, _targetAngle, 0),
                Time.deltaTime * 10);
        }
    }

    private void SelectHelicopter(int number)
    {
        currentHelicopter = number;
        currentHelicopterName.text = helicoptersName[number];

        switch (number)
        {
            case 0:
                _targetAngle = 0f;
                break;
            case 1:
                _targetAngle = 120f;
                break;
            case 2:
                _targetAngle = 240f;
                break;
        }
    }
}