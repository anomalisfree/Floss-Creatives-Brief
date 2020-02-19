using UnityEngine;

public class BubbleController : MonoBehaviour
{
    private Animator _animator;
    private static readonly int Show = Animator.StringToHash("show");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Camera.main != null) transform.LookAt(Camera.main.transform);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag($"CameraCollider"))
        {
            _animator.SetBool(Show, true);
        }
        
        Debug.Log("OnCollisionEnter " + other.transform.tag);
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag($"CameraCollider"))
        {
            _animator.SetBool(Show, false);
        }
    }
}