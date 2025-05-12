using UnityEngine;

// Script que detecta se o jogador pisou no botão e envia o evento correspondente
public class PlatformButton : MonoBehaviour
{
    public string buttonID; // Identificador único para o botão

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verifica se o objeto que entrou no botão é o Player
        if (collision.CompareTag("Player"))
        {
            // Dispara o evento informando qual botão foi pressionado
            StaticEventChannel.RaiseButtonPressed(buttonID);
        }
    }
}


