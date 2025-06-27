using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public abstract class BaseMove : MonoBehaviour
{
    private Vector2 _moveVec=new Vector2();
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void CharacterMove(Vector2 inputVector,float moveSpeed)
    {
        ConvertVec(inputVector);
        MoveAction(_moveVec,moveSpeed);
        
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    protected virtual void MoveAction(Vector2 moveVec,float moveSpeed)
    {
       
        transform.position +=(Vector3)moveVec*Time.fixedDeltaTime*moveSpeed;
    }
    protected void ConvertVec(Vector2 vector)
    {
        _moveVec.x=vector.x;
        _moveVec.y= vector.y;
       
    }
}
