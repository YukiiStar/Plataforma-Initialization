using UnityEngine;

// Script que escuta o evento e abre a porta se for o botão correto
public class Door : MonoBehaviour
{
    public string expectedButtonID; // ID que a porta espera receber para abrir
    private Collider2D col;
    private SpriteRenderer rend;

    private void Awake()
    {
        // Pega os componentes necessários para ativar/desativar
        col = GetComponent<Collider2D>();
        rend = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        // Se inscreve no evento assim que o objeto é ativado
        StaticEventChannel.OnButtonPressed += HandleButtonPressed;
    }

    private void OnDisable()
    {
        // Se desinscreve ao ser desativado (boa prática para evitar erros)
        StaticEventChannel.OnButtonPressed -= HandleButtonPressed;
    }

    // Função que lida com o evento recebido
    private void HandleButtonPressed(string buttonID)
    {
        // Se o botão pressionado for o que a porta espera...
        if (buttonID == expectedButtonID)
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        // Desativa o colisor (porta "abre")
        col.enabled = false;

        // Muda a cor da porta como feedback visual
        rend.color = Color.green;
    }
}

