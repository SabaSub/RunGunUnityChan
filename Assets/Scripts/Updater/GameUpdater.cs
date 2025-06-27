using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SabaSimpleDIContainer;

public class GameUpdater : ITickable,IStartable
{
    [Injection]
    private ControllerUpdater _controllerUpdater;
    [Injection]
    private CollisionableUpdater _collisionUpdater;
    void IStartable.Start()
    {

    }
    void ITickable.Tick()
    {
       
        
        _collisionUpdater.UpdateCollisionable();
        _controllerUpdater.UpdateController();
    }
}
