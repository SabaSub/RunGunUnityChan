using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TestCharacter : BaseCharacter
{

    protected override void SubCharacterStart()
    {
        
    }
    public override void OnCollisionEvent(ICollisionable2D collisionable)
    {
        Debug.Log("Hit"+collisionable);
    }
}
