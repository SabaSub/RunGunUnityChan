using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class MainSkill1 : BaseMainSkill
{
    public override IBaseCollisionData2D BaseData { get => _circle; }
    private CircleData2D _circle;
    private CircleCollider2D _circleCollider;
    protected override void SubStart()
    {
        _circleCollider=this.gameObject.GetComponent<CircleCollider2D>();
        _circle = new (transform.position,_circleCollider.radius);
        _updateDel += UpdateCircle;
    }
    private void UpdateCircle()
    {
        _circle.SetData(transform.position,_circleCollider.radius);
    }
    public override void OnCollisionEvent(ICollisionable2D collisionable)
    {
       
    }
}
