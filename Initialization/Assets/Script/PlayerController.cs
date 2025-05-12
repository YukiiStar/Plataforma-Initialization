using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public int moedas = 0;
    public float moveSpeed = 5f;
    
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    
    // Update is called once per frame
    void Update()
    {
        // Captura entrada do teclado (A/D ou setas)
        float moveX = Input.GetAxis("Horizontal");

        // Move o jogador na horizontal, mantendo a velocidade vertical
        rb.velocity = new Vector2(moveX * moveSpeed, rb.velocity.y);

    }
}
