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
    
    public class MoveRight : ICommand
    {
        private Transform myPlayerTransform;

        public MoveRight(Transform playerTransform)
        {
            myPlayerTransform = playerTransform;
        }
        public void Do()
        {
            myPlayerTransform.position += Vector3.up;
        } 
    }

    public class GetCoin : ICommand
    {
        private GameObject coinObject;
        private SimplePlayer player;
    }
}