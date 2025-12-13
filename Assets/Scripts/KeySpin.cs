using UnityEngine;

public class KeySpin : MonoBehaviour
{
    public float rotationSpeed;
    void FixedUpdate()
    {
        this.transform.Rotate(rotationSpeed * Time.deltaTime, 0f, 0f);
        
    }
}
