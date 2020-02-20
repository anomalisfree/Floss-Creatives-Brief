using UnityEngine;

public class HideInTime : MonoBehaviour
{
    public float timeToHide;

    private float _timer;

    private void OnEnable()
    {
        _timer = timeToHide;
    }

    private void Update()
    {
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}