using System;
using UnityEngine;

public class ControllerAddEvent
{

    public readonly PlayerController controller;
    public ControllerAddEvent(PlayerController controller)
    {
        this.controller = controller;
    }

}
public class CharacterAddEvent
{
    public readonly BaseCharacter character;
    public CharacterAddEvent(BaseCharacter character) 
    {
        this.character = character; 
    }
}

public class CollisionableAddEvent
{
    public readonly ICollisionable2D collisionable;
    public CollisionableAddEvent(ICollisionable2D collisionable)
    {
        this.collisionable = collisionable;
    }
}

public class CharacterCreateRequest
{
    public readonly BaseCharacter character;
    public readonly Vector3 position;
    public readonly Quaternion rotation;
    public readonly Transform parent;
    public CharacterCreateRequest(BaseCharacter character,Vector3 position,Quaternion rotation,Transform parent)
    {
        this.character = character;
        this.position = position;
        this.rotation = rotation;
        this.parent = parent;
    }
}
