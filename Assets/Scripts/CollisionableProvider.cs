using System;
using System.Collections.Generic;

using SabaSimpleDIContainer;
using SabaSimpleDIContainer.Pipe;
using SabaSimpleDIContainer.Unity;
using UnityEngine;

public class CollisionableProvider:IStartable
{
    [Injection]
    private IPublisher<CollisionableAddEvent> _publisher;
    private ISubscriber<CharacterAddEvent> _subscriber;
    #region DIMethod
    [Injection]
    private void InjectCharacterAddEvent(ISubscriber<CharacterAddEvent> subscriber)
    {
        _subscriber = subscriber;
        _subscriber.Subscribe(OnCharacterAdded);
    }
    #endregion
    #region Event
    private void OnCharacterAdded(CharacterAddEvent addEvent)
    {
        if(addEvent.character is not ICollisionable2D col)
        {
            return;
        }
        _publisher.Publish(new CollisionableAddEvent(col));
    }
    #endregion 

    
    void IStartable.Start()
    {
        //Debug.Log("start");
        //ICollisionable[] collisionables = InterfaceFinder.FindObjectsOfTypeByInterface<ICollisionable>().ToArray();
        //foreach(ICollisionable collisionable in collisionables)
        //{
        //    Debug.Log("collisionable"+collisionable);
        //    _publisher.Publish(new CollisionableAddEvent(collisionable));
        //}
    }
}