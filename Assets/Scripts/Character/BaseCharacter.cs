using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using SabaSimpleDIContainer.Pipe;
using SabaSimpleDIContainer;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public abstract class BaseCharacter : MonoBehaviour, ICollisionable2D
{
    protected delegate void CharacterUpdateDel();
    protected CharacterUpdateDel _characterUpdateDel;
    [SerializeField, Tooltip("�ړ�")]
    protected BaseMove _move;

    [SerializeField]
    private BaseMainSkill _mainSkill;

    [SerializeField]
    protected CharacterStatus _status;
    public CharacterStatus Status { get => _status; }
    public IBaseCollisionData2D BaseData { get => _circle; }
    protected CircleData2D _circle;
    private CircleCollider2D _circleCollider;
    private Vector3 _origin;
    private Vector3 _end;
    public CheckCollisionMode CheckCollisionMode => collisionMode;
    private CheckCollisionMode collisionMode = CheckCollisionMode.collisionable;
    public LayerMask CollisionableLayer => mask;
    [SerializeField]
    private LayerMask mask;


    [Injection]
    private IPublisher<CollisionableAddEvent> _collisionableAddEvent;//�U���̓����蔻��n���p
    // Start is called before the first frame update
    void Start()
    {
        CheckActionInstance();
        _circleCollider = GetComponent<CircleCollider2D>();
        _circle = new(transform.position,_circleCollider.radius);
        _characterUpdateDel += UpdateCircle;
        
        _characterUpdateDel += _mainSkill.UpdateSkill;
        
        
        SubCharacterStart();

    }
    protected virtual void SubCharacterStart()
    {

    }
    public void CharacterUpdate()
    {
        _characterUpdateDel.Invoke();
    }

    public void MoveCharacter(Vector2 vector)
    {
        _move.CharacterMove(vector, _status.MoveSpeed);
    }
    
    
    /// <summary>
    /// serializeField�ɓo�^����ĂȂ���ΐ���
    /// </summary>
    private void CheckActionInstance()
    {

        _move ??= this.gameObject.AddComponent<CharacterMove>();
        _mainSkill ??= this.gameObject.AddComponent<MainSkill1>();
        

    }
    private void UpdateCircle()
    {
        _circle.SetData(transform.position, _circleCollider.radius);
    }
    public virtual void OnCollisionEvent(ICollisionable2D collisionable)
    {
        Debug.Log("Hit" + collisionable.gameObject.transform.name);
    }

}
