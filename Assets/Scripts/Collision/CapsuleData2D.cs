using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// àÍäáÇ≈ä«óùÇ∑ÇÈÇΩÇﬂ
/// </summary>
public interface IBaseCollisionData2D
{
    public bool CheckCollision(IBaseCollisionData2D collisionData);
    
   
}

public class CapsuleData2D:IBaseCollisionData2D
{
    public Vector2 endPoint;
    public Vector2 originPoint;
    public float radius;
    private LineData2D lineData;
    public CapsuleData2D()
    {
        lineData = new LineData2D(originPoint,endPoint);
    }
    public CapsuleData2D(Vector2 origin, Vector2 end, float radius)
    {
        this.endPoint = end;
        this.originPoint = origin;
        this.radius = radius;
    }
    public void SetData(Vector2 origin, Vector2 end, float radius)
    {
        this.endPoint = end;
        this.originPoint = origin;
        this.radius = radius;
    }
    public LineData2D ToLine()
    {
        lineData.SetData(originPoint, endPoint);
        return lineData;
    }
    public bool CheckCollision(IBaseCollisionData2D collisionData)
    {
        return this.CheckCollision2D(collisionData);
    }
}
public class CircleData2D:IBaseCollisionData2D
{
    public Vector2 position;
    public float radius;
    public CircleData2D(Vector2 position, float radius)
    {
        this.position = position;
        this.radius = radius;
    }
    public void SetData(Vector2 position,float radius)
    {
        this.position = position;
        this.radius = radius;
    }
    public bool CheckCollision(IBaseCollisionData2D collisionData)
    {
        return this.CheckCollision2D(collisionData);
    }
}
public class LineData2D:IBaseCollisionData2D
{
    public Vector2 endPoint;
    public Vector2 originPoint;
    public LineData2D(Vector2 origin, Vector2 end)
    {
        this.endPoint = end;
        this.originPoint = origin;
    }
    public void SetData(Vector2 origin, Vector2 end)
    {
        this.endPoint = end;
        this.originPoint = origin;
    }
    public bool CheckCollision(IBaseCollisionData2D collisionData)
    {
        return this.CheckCollision2D(collisionData);
    }
}
public class BoxData2D : IBaseCollisionData2D
{
    public Vector2 originPoint;
    public Vector2 endPoint;
    public float boxWidth;
    private LineData2D lineData;
    public BoxData2D(Vector2 originPoint,Vector2 endPoint,float boxWidth)
    {
        this.boxWidth = boxWidth;
        this.originPoint = originPoint;
        this.endPoint = endPoint;
        lineData = new LineData2D(originPoint, endPoint);
    }
    public void SetData(Vector2 originPoint,Vector2 endPoint,float boxWidth)
    {
        this.boxWidth = boxWidth;
        this.originPoint = originPoint;
        this.endPoint = endPoint;
    }
    public LineData2D ToLine()
    {
        lineData.SetData(originPoint, endPoint);
        return lineData;
    }
    public bool CheckCollision(IBaseCollisionData2D collisionData)
    {
        return this.CheckCollision2D(collisionData);
    }
}


