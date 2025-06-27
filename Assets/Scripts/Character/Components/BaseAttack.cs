using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public abstract class BaseAttack : MonoBehaviour
{


    public ReactiveProperty<bool> isAttacking=new(false);
    protected bool _isAttacking=default;
    private float _attackCount = 0f;
    [SerializeField]
    private float _attackTime = 0.5f;
    public BaseDetection Detection { get => _attackDetection; }
    [SerializeField]
    private BaseDetection _attackDetection;
    private delegate void UpdateDelegate();
    private UpdateDelegate _updateDelegate;
    // Start is called before the first frame update
    void Start()
    {
        _updateDelegate += TestLog;
        SubStart();
    }
    protected void SubStart()
    {
       
    }
    private void TestLog()
    {
        Debug.Log("IsAttacking="+_isAttacking);
    }
    /// <summary>
    /// çUåÇ
    /// </summary>
    public void AttackAction()
    {
        _isAttacking = true;
        isAttacking.Value = _isAttacking;
    }
    protected void AttackEnd()
    {
        _isAttacking = false;
        isAttacking.Value = false;
    }
    /// <summary>
    /// çUåÇíÜÇÃçXêV
    /// </summary>
    public void UpdateAttack()
    {
        _updateDelegate.Invoke();
    }
}
