using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CharacterStatus
{
    #region moveSpeed
    [SerializeField]
    private float moveSpeed = 1f;
    public float MoveSpeed
    {
        get 
        { 
            return moveSpeed; 
        }
        
    }
    #endregion
    #region HitPoint
    [SerializeField, Range(0, 9999)]
    private int hitPoint = 1;
    public float HitPoint
    {
        get
        {
            if (hitPoint < 0)
            {
                return 0;
            }
            return hitPoint;
        }
    }
    #endregion

}


