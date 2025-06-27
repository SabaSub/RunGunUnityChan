using System.Collections;
using System.Collections.Generic;
using SabaSimpleDIContainer;
using SabaSimpleDIContainer.Pipe;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(Animator))]
public class PlayerCharacter : BaseCharacter
{
   
   
    protected override void SubCharacterStart()
    {
       
        
    }
    
   
    
    public override void OnCollisionEvent(ICollisionable2D collisionable)
    {
        switch (collisionable)
        {
            case Wall wall:
                {

                    return;
                }
        }



    }
}
