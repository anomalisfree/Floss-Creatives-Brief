using UnityEngine;

public class HelicopterControl : MonoBehaviour
{
    public Transform pivotRotation;

    public Transform rotor;
    public Transform rotorBack;

    private bool _isRotate;

    private FixedJoystick _leftJoystick;
    private FixedJoystick _rightJoystick;

    private AudioSource _audioSource;

    private float _xAngleRotation;
    private float _yAngleRotation;
    private float _zAngleRotation;

    private float _xTranslate;
    private float _yTranslate;
    private float _zTranslate;

    private float _currentRotatorSpeed;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    public void SetControl(FixedJoystick leftJoystick, FixedJoystick rightJoystick)
    {
        _leftJoystick = leftJoystick;
        _rightJoystick = rightJoystick;
    }

    public void SetControl(bool isRotate)
    {
        _isRotate = isRotate;
    }

    private void Update()
    {
        if (_isRotate)
        {
            _currentRotatorSpeed = Mathf.Lerp(_currentRotatorSpeed, 1 / Time.deltaTime, Time.deltaTime);
            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
        }
        else if (_leftJoystick != null && _rightJoystick != null)
        {
            _zAngleRotation = Mathf.Lerp(_zAngleRotation, -_leftJoystick.Vertical * 45f, Time.deltaTime);
            _yAngleRotation = Mathf.Lerp(_yAngleRotation, _rightJoystick.Horizontal / 1f, Time.deltaTime);
            _xAngleRotation = Mathf.Lerp(_xAngleRotation, -_leftJoystick.Horizontal * 45f, Time.deltaTime);

            _xTranslate = Mathf.Lerp(_xTranslate, -_leftJoystick.Horizontal / 25f, Time.deltaTime);
            _yTranslate = Mathf.Lerp(_yTranslate, _rightJoystick.Vertical / 50f, Time.deltaTime);
            _zTranslate = Mathf.Lerp(_zTranslate, _leftJoystick.Vertical / 25f, Time.deltaTime);

            pivotRotation.localRotation =
                Quaternion.Euler(_xAngleRotation, pivotRotation.localRotation.eulerAngles.y, _zAngleRotation);

            pivotRotation.Rotate(Vector3.up, _yAngleRotation);

            var right = pivotRotation.right;
            var forward = pivotRotation.forward;
            transform.Translate((new Vector3(right.x, 0, right.z) * _zTranslate +
                                 pivotRotation.up * _yTranslate +
                                 new Vector3(forward.x, 0, forward.z) * _xTranslate) * transform.localScale.x);

            _currentRotatorSpeed = Mathf.Lerp(_currentRotatorSpeed, 1 / Time.deltaTime, Time.deltaTime);

            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
        }
        else
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Stop();
            }

            _currentRotatorSpeed = Mathf.Lerp(_currentRotatorSpeed, 0, Time.deltaTime);
        }

        rotor.transform.Rotate(Vector3.up, _currentRotatorSpeed);
        rotorBack.transform.Rotate(Vector3.forward, _currentRotatorSpeed);
    }
}