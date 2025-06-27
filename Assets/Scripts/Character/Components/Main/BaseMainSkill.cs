using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseMainSkill : MonoBehaviour,ICollisionable2D
{
    private bool _isActive = default;
   
    protected delegate void UpdateDel();
    protected UpdateDel _updateDel;

    public abstract IBaseCollisionData2D BaseData { get; }

    public CheckCollisionMode CheckCollisionMode { get => _checkCollisionMode; }
   
    private CheckCollisionMode _checkCollisionMode;
    public LayerMask CollisionableLayer { get => _layer; }
    [SerializeField]
    protected LayerMask _layer;
    private void Start()
    {
        
        SubStart();
    }
    protected virtual void SubStart()
    {

    }
    public void MainSkill(int damage)
    {
       
        _isActive = true;
    }
    public virtual void UpdateSkill()
    {
        if(!_isActive)
        {
            return;
        }
        _updateDel.Invoke();
        //Ç±Ç±Ç…èàóù
    }

    public virtual void OnCollisionEvent(ICollisionable2D collisionable)
    {
        Debug.Log("Hit Collisionable is "+collisionable.gameObject.transform.name);
    }
}
