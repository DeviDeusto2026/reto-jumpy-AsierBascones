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
            // ForceMode.Impulse para que el salto sea inmediato
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            // Para evitar el doble salto
            canJump = false;
        }
    }

    private void ProcessInput()
    {
        // Solo nos hace falta mover en el eje x
        float moveX = Input.GetAxis("Horizontal");
        moveDirection = new Vector3(moveX, 0f, 0f);
    }

    private void ApplyMovement()
    {
            rb.linearVelocity = new Vector3(moveDirection.x * speed, rb.linearVelocity.y, 0f);
    }
    private void HandleOneWayPlatforms()
    {
        // La velocidad del rb es positiva cuando el zorro está subiendo
        if (rb.linearVelocity.y > 0)
        {
            // Ignoramos los choques entre la capa Player y la capa ground
            Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("ground"), true);
        }
        else
        {
            // Si la velocidad del rb es 0 o negativa (está cayendo), reactivamos los choques para que aterrice
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
