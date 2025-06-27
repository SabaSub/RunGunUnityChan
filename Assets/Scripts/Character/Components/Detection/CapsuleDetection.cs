using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CapsuleDetection : BaseDetection
{
    public override IBaseCollisionData2D BaseData { get=>_capsuleData; }
    private CapsuleData2D _capsuleData=new();
    [SerializeField]
    private Transform _origin;
    [SerializeField]
    private Transform _end;
    [SerializeField]
    private float _radius=0.2f;
    protected override void SubStart()
    {
        CheckTransformInstance();
        _capsuleData=new CapsuleData2D(_origin.position,_end.position,_radius);
    }
    private void CheckTransformInstance()
    {
        _origin ??= this.transform;
        _end ??= this.transform;
    }
    public override void AttackStart(int damage)
    {
        _collisionMode = CheckCollisionMode.collisionable;
        Debug.Log("AttackStart");
        _capsuleData.SetData(_origin.position, _end.position, _radius);
    }
    public override void AttackEnd()
    {
        _collisionMode=CheckCollisionMode.dontCollisionable;
    }
    public override void DetectionUpdate()
    {
        _capsuleData.SetData(_origin.position,_end.position,_radius);
    }
    public override void OnCollisionEvent(ICollisionable2D collisionable)
    {
        base.OnCollisionEvent(collisionable);
    }
}
