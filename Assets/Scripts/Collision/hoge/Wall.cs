using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Wall : MonoBehaviour, ICollisionable2D
{
    public IBaseCollisionData2D BaseData { get => _boxData; }
    private BoxData2D _boxData;
    public CheckCollisionMode CheckCollisionMode { get => _collisionMode; }
    private CheckCollisionMode _collisionMode;
    public LayerMask CollisionableLayer { get => _mask; }
    [SerializeField]
    private LayerMask _mask = default;
    private BoxCollider _boxCollider;
    private void Start()
    {
        _boxCollider = GetComponent<BoxCollider>();
        CreateBoxData();

    }

    private void SetBoxData()
    {
        Vector3 herf = new Vector3(0, _boxCollider.size.y / 2, 0);
        Vector3 origin = transform.position + herf;
        Vector2 end = transform.position - herf;
        
    }
    private void CreateBoxData()
    {
        Vector3 herf = new Vector3(0, transform.lossyScale.y*_boxCollider.size.y,0);
        Vector3 origin = transform.position + herf;
        Vector2 end = transform.position - herf;
        _boxData = new(origin, end, transform.lossyScale.x*_boxCollider.size.x/2);
        Debug.Log("o"+_boxData.originPoint+" e"+_boxData.endPoint+" r"+_boxData.boxWidth);
    }
    public void OnCollisionEvent(ICollisionable2D collisionable)
    {
        
    }
}
