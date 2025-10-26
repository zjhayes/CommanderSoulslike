using UnityEngine;
using UnityEngine.Events;

public class Actions : MonoBehaviour
{
    public UnityEvent onStart;

    private void Start()
    {
        onStart?.Invoke();
    }
}
