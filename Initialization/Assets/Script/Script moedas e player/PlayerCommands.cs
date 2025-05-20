using UnityEngine;

public class MoveUp : ICommand
{
    private Transform myPlayerTransform;

    public MoveUp(Transform playerTransform)
    {
        myPlayerTransform = playerTransform;
    }

    public void Do()
    {
        myPlayerTransform.position += Vector3.up;
    }
    public void Undo()
    {
        myPlayerTransform.position -= Vector3.up;
    }
}

public class MoveRight : ICommand


{
    private Transform myPlayerTransform;

    public MoveRight(Transform playerTransform)
    {
        myPlayerTransform = playerTransform;
    }
    public void Do()
    {
        myPlayerTransform.position += Vector3.right;
    } 
    
    public void Undo()
    {
        myPlayerTransform.position -= Vector3.right;
    } 
}

public class GetCoin : ICommand
{
    private GameObject coinObject;
    private SimplePlayer player;

    public GetCoin(GameObject coin, SimplePlayer player)
    {
        coinObject = coin;
        this.player = player;
    }
    public void Do()
    {
        player.moedas++;
        coinObject.SetActive(false);
    }

    public void Undo()
    {
        player.moedas--;
        coinObject.SetActive(true);
        player.UndoLastCommand();
    }
}