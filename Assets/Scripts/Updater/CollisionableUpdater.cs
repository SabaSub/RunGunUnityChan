using System;
using System.Collections.Generic;
using SabaSimpleDIContainer;
using SabaSimpleDIContainer.Pipe;

public class CollisionableUpdater
{
    [Injection]
    private CollisionChecker _collisionChecker;
    [Injection]
    private CharacterUpdater _characterUpdater;
   
    public void UpdateCollisionable()
    {
        _collisionChecker.CheckCollisions();
        
        _characterUpdater.UpdateCharacter();
    }
}