using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxController : MonoBehaviour
{
    [Header("Configuración de Salto")]
    public float jumpForce = 9f;
    [SerializeField]
    private bool canJump;

    [Header("Configuración de Movimiento")]
    [SerializeField]
    private float speed = 6f;

    private Rigidbody rb;
    private Vector3 moveDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ProcessInput();
        JumpIsNeeded();
        HandleOneWayPlatforms();
    }

    void FixedUpdate()
    {
        ApplyMovement();
    }

    private void JumpIsNeeded()
    {
        if (canJump && Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Espacio pulsado");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canJump = false;
        }
    }

    private void ProcessInput()
    {
        float moveX = Input.GetAxis("Horizontal");
        moveDirection = new Vector3(moveX, 0f, 0f);

        if (moveX > 0)
        {
            // Giro a la derecha
            transform.rotation = Quaternion.Euler(0f, 90f, 0f);
        }
        else if (moveX < 0)
        {
            // Giro a la izquierda
            transform.rotation = Quaternion.Euler(0f, -90f, 0f);
        }
    }

    private void ApplyMovement()
    {
        rb.linearVelocity = new Vector3(moveDirection.x * speed, rb.linearVelocity.y, 0f);
    }

    private void HandleOneWayPlatforms()
    {
        if (rb.linearVelocity.y > 0)
        {
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("ground"), true);
        }
        else
        {
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("ground"), false);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            canJump = false;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("ground"))
        {
            canJump = true;
        }
    }
}