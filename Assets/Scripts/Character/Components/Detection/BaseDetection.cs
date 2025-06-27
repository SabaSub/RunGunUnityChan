using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 当たり判定用コンポーネントクラス
/// </summary>
public abstract class BaseDetection : MonoBehaviour, ICollisionable2D
{
    public abstract IBaseCollisionData2D BaseData {  get; }

    public CheckCollisionMode CheckCollisionMode { get=>_collisionMode; }
    protected CheckCollisionMode _collisionMode=default;
    [SerializeField]
    private LayerMask _layerMask=default;
    public LayerMask CollisionableLayer {  get=>_layerMask; }
    private void Start()
    {
        
        SubStart();
    }
    /// <summary>
    /// LateStart
    /// </summary>
    protected virtual void SubStart()
    {

    }
    public abstract void AttackStart(int damage);
    public abstract void AttackEnd();
    public abstract void DetectionUpdate();
    public virtual void OnCollisionEvent(ICollisionable2D collisionable)
    {
        Debug.LogWarning("AttackCollisionable="+collisionable.gameObject.transform.name);
    }
}
