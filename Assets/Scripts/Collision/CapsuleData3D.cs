using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 一括で管理するため
/// </summary>
public interface IBaseCollisionData3D
{
    public bool CheckCollision(IBaseCollisionData3D collisionData);


}

public class CapsuleData3D : IBaseCollisionData3D
{
    public Vector3 endPoint;
    public Vector3 originPoint;
    public float radius;
    private LineData3D lineData;
    public CapsuleData3D()
    {
        lineData = new LineData3D(originPoint, endPoint);
    }
    public CapsuleData3D(Vector3 origin, Vector3 end, float radius)
    {
        this.endPoint = end;
        this.originPoint = origin;
        this.radius = radius;
        lineData = new LineData3D(originPoint, endPoint);
    }
    public void SetData(Vector3 origin, Vector3 end, float radius)
    {
        this.endPoint = end;
        this.originPoint = origin;
        this.radius = radius;
    }
    public LineData3D ToLine()
    {
        
        lineData.SetData(originPoint, endPoint);
        return lineData;
    }
    public bool CheckCollision(IBaseCollisionData3D collisionData)
    {
        return this.CheckCollision3D(collisionData);
    }
}
public class CircleData3D : IBaseCollisionData3D
{
    public Vector3 position;
    public float radius;
    public CircleData3D(Vector3 position, float radius)
    {
        this.position = position;
        this.radius = radius;
    }
    public void SetData(Vector3 position, float radius)
    {
        this.position = position;
        this.radius = radius;
    }
    public bool CheckCollision(IBaseCollisionData3D collisionData)
    {
        return this.CheckCollision3D(collisionData);
    }
}
public class LineData3D : IBaseCollisionData3D
{
    public Vector3 endPoint;
    public Vector3 originPoint;
    public LineData3D(Vector3 origin, Vector3 end)
    {
        this.endPoint = end;
        this.originPoint = origin;
    }
    public void SetData(Vector3 origin, Vector3 end)
    {
        this.endPoint = end;
        this.originPoint = origin;
    }
    public bool CheckCollision(IBaseCollisionData3D collisionData)
    {
        return this.CheckCollision3D(collisionData);
    }
}
public class BoxData3D : IBaseCollisionData3D
{
    public Vector3 originPoint;
    public Vector3 endPoint;
    public float boxWidth;
    private LineData3D lineData;
    public BoxData3D(Vector3 originPoint, Vector3 endPoint, float boxWidth)
    {
        this.boxWidth = boxWidth;
        this.originPoint = originPoint;
        this.endPoint = endPoint;
        lineData = new LineData3D(originPoint, endPoint);
    }
    public void SetData(Vector3 originPoint, Vector3 endPoint, float boxWidth)
    {
        this.boxWidth = boxWidth;
        this.originPoint = originPoint;
        this.endPoint = endPoint;
    }
    public LineData3D ToLine()
    {
        lineData.SetData(originPoint, endPoint);
        return lineData;
    }
    public bool CheckCollision(IBaseCollisionData3D collisionData)
    {
        return this.CheckCollision3D(collisionData);
    }
}


