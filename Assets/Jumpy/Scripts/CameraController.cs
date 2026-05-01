using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{
    [Header("Referencias")]
    public Transform player;

    private float highestY;
    private Vector3 offset;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();

        // Guardamos la posición Y inicial de la cámara
        highestY = transform.position.y;

        // Calculamos la distancia inicial entre la cámara y el jugador para mantener el encuadre
        if (player != null)
        {
            offset = transform.position - player.position;
        }
    }
    void LateUpdate()
    {
        if (player == null) return;

        float targetY = player.position.y + offset.y;

        // Si esta nueva posición es más alta que la máxima alcanzada, la actualizamos
        if (targetY > highestY)
        {
            highestY = targetY;
        }

        // Movemos la cámara. Mantenemos su X y Z originales, solo alteramos la Y a nuestro "highestY".
        transform.position = new Vector3(transform.position.x, highestY, transform.position.z);

        Vector3 playerInViewport = cam.WorldToViewportPoint(player.position);

        if (playerInViewport.y < -0.1f)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        Debug.Log("¡Game Over! El zorro se ha caído.");

        // Reiniciamos la escena actual para volver a empezar
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
