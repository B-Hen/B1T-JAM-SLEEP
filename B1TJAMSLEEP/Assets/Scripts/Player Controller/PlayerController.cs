using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed, movement;

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        if (Input.GetKey(KeyCode.W))
            transform.localPosition += new Vector3(0f, movement, 0f) * (speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.A))
            transform.localPosition -= new Vector3(movement, 0f, 0f) * (speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.S))
            transform.localPosition -= new Vector3(0f, movement, 0f) * (speed * Time.deltaTime);
        if (Input.GetKey(KeyCode.D))
            transform.localPosition += new Vector3(movement, 0f, 0f) * (speed * Time.deltaTime);
    }
}
