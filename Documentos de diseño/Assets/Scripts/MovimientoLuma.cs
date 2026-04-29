using UnityEngine;

public class MovimientoLuma : MonoBehaviour
{
    public float velocidad = 5f;
    public float gravedad = -20f;
    public float velocidadRotacion = 12f;

    public float alturaSalto = 0.1f;
    public KeyCode teclaSalto = KeyCode.Space;
    public ParticleSystem splash;

    private CharacterController controller;
    private Vector3 velocidadY;
    private Animator animator;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 direccion = new Vector3(x, 0f, z).normalized;

        // ROTACIÓN
        if (direccion.magnitude > 0.1f)
        {
            Quaternion objetivo = Quaternion.LookRotation(direccion);
            objetivo *= Quaternion.Euler(0, 90, 0);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                objetivo,
                velocidadRotacion * Time.deltaTime
            );
        }

        // SUELO
        if (controller.isGrounded && velocidadY.y < 0)
        {
            velocidadY.y = -2f;
            animator.SetBool("isJumping", false);
        }

        // SALTO
        if (controller.isGrounded && Input.GetKeyDown(teclaSalto))
        {
            velocidadY.y = Mathf.Sqrt(alturaSalto * -2f * gravedad) * 0.6f;
            animator.SetBool("isJumping", true);
        }

        // GRAVEDAD
        velocidadY.y += gravedad * Time.deltaTime;

        // MOVIMIENTO
        Vector3 move = direccion * velocidad;
        move.y = velocidadY.y;

        controller.Move(move * Time.deltaTime);

        animator.SetFloat("Speed", direccion.magnitude);

        // SPLASH AL CAMINAR
        if (direccion.magnitude > 0.1f && controller.isGrounded)
        {
            if (!splash.isPlaying)
                splash.Play();
        }
        else
        {
            if (splash.isPlaying)
                splash.Stop();
        }
    }
}