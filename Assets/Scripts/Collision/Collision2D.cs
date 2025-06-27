using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;


public static class CapsuleCollision2D
{
    #region 計算
    private static Vector2 GetNearestPoint2D(Vector2 origin, Vector2 end, Vector2 position)
    {
        //カプセルの始点から終点のベクトルを正規化したもの
        Vector2 vector1 = (end - origin).normalized;
        //始点から点へのベクトル
        Vector2 originToPoint = position - origin;
        //終点から点へのベクトル
        Vector2 endToPoint = position - end;

        if (0 > Vector2.Dot(vector1, originToPoint))
        {

            return origin;
        }
        else if (0 < Vector2.Dot(vector1, endToPoint))
        {
            return end;
        }

        Vector2 point = origin + vector1 * Vector2.Dot(vector1, originToPoint);

        return point;
    }
    private static Vector2 GetNearestPoint2D(LineData2D lineData1, LineData2D lineData2)
    {
        Vector2 originNearPoint = GetNearestPoint2D(lineData1.originPoint, lineData1.endPoint, lineData2.originPoint);
        float originDistance = GetDistance2D(originNearPoint, lineData2.originPoint);
        Vector2 endNearPoint = GetNearestPoint2D(lineData1.originPoint, lineData1.endPoint, lineData2.endPoint);
        float endDistance = GetDistance2D(endNearPoint, lineData2.endPoint);
        if (originDistance <= endDistance)
        {
            return originNearPoint;
        }
        return endNearPoint;
    }


    private static float GetDistance2D(Vector2 point1, Vector2 point2)
    {

        Vector2 distanceVec = point1 - point2;
        float distance = Mathf.Sqrt(
            Mathf.Pow(Mathf.Abs(distanceVec.x), 2) +
            Mathf.Pow(Mathf.Abs(distanceVec.y), 2)
            );
        return distance;
    }
    private static float GetLinesDistance2D(LineData2D lineData1, LineData2D lineData2)
    {
        Vector2 originNearPoint = GetNearestPoint2D(lineData1.originPoint, lineData1.endPoint, lineData2.originPoint);
        float originDistance = GetDistance2D(originNearPoint, lineData2.originPoint);
        Vector2 endNearPoint = GetNearestPoint2D(lineData1.originPoint, lineData1.endPoint, lineData2.endPoint);
        float endDistance = GetDistance2D(endNearPoint, lineData2.endPoint);
        if (originDistance <= endDistance)
        {
            return originDistance;
        }
        return endDistance;
    }
    [MethodImpl (MethodImplOptions.AggressiveInlining)]
    private  static Vector2 GetVector(Vector2 point,Vector2 target)
    {
        return target - point;
    }
    #endregion
    #region capsule
    public static bool CheckCollision2D(this CapsuleData2D capsuleData, IBaseCollisionData2D collisionData)
    {
        switch (collisionData)
        {
            case CircleData2D circle:
                {
                    return capsuleData.CheckCollision2D(circle);
                }
            case CapsuleData2D capsule:
                {
                    return capsuleData.CheckCollision2D(capsule);
                }
            case LineData2D line:
                {
                    return capsuleData.CheckCollision2D(line);
                }

        }
        
        return false;
    }
    /// <summary>
    /// point
    /// </summary>
    /// <param name="capsuleData"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this CapsuleData2D capsuleData, Vector2 position)
    {
        Vector2 point = GetNearestPoint2D(capsuleData.originPoint, capsuleData.endPoint, position);
        //Debug.Log(point);
        float distance = GetDistance2D(position, point);
       
        if (distance - capsuleData.radius <= 0)
        {
            
            return true;
        }

        return false;
    }
    /// <summary>
    /// circle
    /// </summary>
    /// <param name="capsuleData"></param>
    /// <param name="position"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this CapsuleData2D capsuleData, CircleData2D circleData)
    {
        Vector2 nearestPoint = GetNearestPoint2D(capsuleData.originPoint, capsuleData.endPoint, circleData.position);
        Vector2 direction = circleData.position - nearestPoint;
        float distance = direction.magnitude;

        if (distance < capsuleData.radius + circleData.radius)
        {
            
            return true;
        }

        
        return false;
    }
    /// <summary>
    /// line
    /// </summary>
    /// <param name="capsuleData"></param>
    /// <param name="circleData"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this CapsuleData2D capsuleData, LineData2D lineData)
    {

        float distance = GetLinesDistance2D(capsuleData.ToLine(), lineData);

        if (distance - (capsuleData.radius + capsuleData.radius) <= 0)
        {
            
            return true;
        }
        return false;
    }
    /// <summary>
     /// capsule
     /// </summary>
     /// <param name="capsuleData"></param>
     /// <param name="circleData"></param>
     /// <returns></returns>
    public static bool CheckCollision2D(this CapsuleData2D capsuleData1, CapsuleData2D capsuleData2)
    {
        Vector2 nearestPoint1 = GetNearestPoint2D(capsuleData1.originPoint, capsuleData1.endPoint, capsuleData2.originPoint);
        Vector2 nearestPoint2 = GetNearestPoint2D(capsuleData2.originPoint, capsuleData2.endPoint, capsuleData1.originPoint);
        Vector2 direction = nearestPoint2 - nearestPoint1;
        float distance = direction.magnitude;

        if (distance < capsuleData1.radius + capsuleData2.radius)
        {
            
            return true;
        }

        
        return false;
    }
    public static bool CheckCollision2D(this CapsuleData2D capsule, BoxData2D box)
    {
        Vector2 nearestPoint = GetNearestPoint2D(box.ToLine(), capsule.ToLine());
        if (CheckLineOut(box, nearestPoint, out Vector2 point))
        {
            return false;
        }
        if (GetDistance2D(point, nearestPoint) > box.boxWidth + capsule.radius)
        {
            return false;
        }
        return true;
    }
    #endregion
    #region circle
    public static bool CheckCollision2D(this CircleData2D circleData, IBaseCollisionData2D collisionData)
    {
        switch (collisionData)
        {
            case CircleData2D circle:
                {
                    return circleData.CheckCollision2D(circle);
                }
            case CapsuleData2D capsule:
                {
                    return circleData.CheckCollision2D(capsule);
                }
            case LineData2D line:
                {
                    return circleData.CheckCollision2D(line);
                }
            case BoxData2D box:
                {
                    return circleData.CheckCollision2D(box);
                }

        }

        return false;
    }

    /// <summary>
    /// circle
    /// </summary>
    /// <param name="circleData"></param>
    /// <param name="circleData1"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this CircleData2D circleData, CircleData2D circleData1)
    {


        float distance = GetDistance2D(circleData.position, circleData1.position);

        if (distance - (circleData.radius + circleData1.radius) <= 0)
        {

            return true;
        }
        return false;
    }
    /// <summary>
    /// point
    /// </summary>
    /// <param name="circleData"></param>
    /// <param name="point"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this CircleData2D circleData, Vector2 point)
    {

        //Debug.Log(point);
        float distance = GetDistance2D(circleData.position, point);
        if (distance - circleData.radius <= 0)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// line
    /// </summary>
    /// <param name="circleData"></param>
    /// <param name="lineData"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this CircleData2D circleData, LineData2D lineData)
    {

        Vector2 point = GetNearestPoint2D(lineData.originPoint, lineData.endPoint, circleData.position);
        float distance = GetDistance2D(circleData.position, point);
        if (distance - circleData.radius <= 0)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// casule
    /// </summary>
    /// <param name="circleData"></param>
    /// <param name="capsuleData"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this CircleData2D circleData, CapsuleData2D capsuleData)
    {

        Vector2 point = GetNearestPoint2D(capsuleData.originPoint, capsuleData.endPoint, circleData.position);
        float distance = GetDistance2D(circleData.position, point);
        if (distance - (circleData.radius + capsuleData.radius) <= 0)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// box
    /// </summary>
    /// <param name="circleData"></param>
    /// <param name="box"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this CircleData2D circle, BoxData2D box)
    {
        if (!CheckLineOut(box, circle.position, out Vector2 point))
        {
            return false;
        }
        if (GetDistance2D(point, circle.position) > box.boxWidth + circle.radius)
        {
            return false;
        }
        return true;
    }
    #endregion
    #region line
    public static bool CheckCollision2D(this LineData2D lineData, IBaseCollisionData2D collisionData)
    {
        switch (collisionData)
        {
            case CircleData2D circle:
                {
                    return lineData.CheckCollision2D(circle);
                }
            case CapsuleData2D capsule:
                {
                    return lineData.CheckCollision2D(capsule);
                }
            case LineData2D line:
                {
                    return lineData.CheckCollision2D(line);
                }
            case BoxData2D box:
                {
                    return lineData.CheckCollision2D(box);
                }

        }

        return false;
    }
    /// <summary>
    /// point
    /// </summary>
    /// <param name="lineData"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this LineData2D lineData, Vector2 position)
    {
        Vector2 point = GetNearestPoint2D(lineData.originPoint, lineData.endPoint, position);
        Debug.Log(point);
        float distance = GetDistance2D(position, point);
        if (distance <= 0)
        {

            return true;
        }
        return false;
    }
    /// <summary>
    /// circle
    /// </summary>
    /// <param name="lineData"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this LineData2D lineData, CircleData2D circleData)
    {
        Vector2 point = GetNearestPoint2D(lineData.originPoint, lineData.endPoint, circleData.position);
        Debug.Log(point);
        float distance = GetDistance2D(circleData.position, point);
        if (distance - circleData.radius <= 0)
        {

            return true;
        }
        return false;
    }
    /// <summary>
    /// capsule
    /// </summary>
    /// <param name="lineData"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this LineData2D lineData, CapsuleData2D capsuleData)
    {

        float distance = GetLinesDistance2D(lineData, capsuleData.ToLine());
        if (distance - capsuleData.radius <= 0)
        {

            return true;
        }
        return false;
    }/// <summary>
     /// line
     /// </summary>
     /// <param name="lineData"></param>
     /// <param name="position"></param>
     /// <returns></returns>
    public static bool CheckCollision2D(this LineData2D lineData1, LineData2D lineData2)
    {

        float distance = GetLinesDistance2D(lineData1, lineData2);
        if (distance <= 0)
        {

            return true;
        }
        return false;
    }
    /// <summary>
    /// box
    /// </summary>
    /// <param name="lineData1"></param>
    /// <param name="lineData2"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this LineData2D lineData, BoxData2D box)
    {

        Vector2 nearestPoint = GetNearestPoint2D(box.ToLine(), lineData);
        if (CheckLineOut(box, nearestPoint, out Vector2 point))
        {
            return false;
        }
        if (GetDistance2D(point, nearestPoint) > box.boxWidth)
        {
            return false;
        }
        return true;
    }
    #endregion
    #region box
    public static bool CheckCollision2D(this BoxData2D boxData, IBaseCollisionData2D collisionData)
    {
        switch (collisionData)
        {
            case CircleData2D circle:
                {
                    return boxData.CheckCollision2D(circle);
                }
            case CapsuleData2D capsule:
                {
                    return boxData.CheckCollision2D(capsule);
                }
            case LineData2D line:
                {
                    return boxData.CheckCollision2D(line);
                }
            case BoxData2D box:
                {
                    return boxData.CheckCollision2D(box);
                }

        }

        return false;
    }
    /// <summary>
    /// point
    /// </summary>
    /// <param name="box"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this BoxData2D box, Vector2 position)
    {

        if (!CheckLineOut(box, position, out Vector2 point))
        {
            return false;
        }
        if (GetDistance2D(point, position) > box.boxWidth)
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// line
    /// </summary>
    /// <param name="box"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this BoxData2D box, LineData2D line)
    {
        Vector2 nearestPoint = GetNearestPoint2D(box.ToLine(), line);
        if (CheckLineOut(box, nearestPoint, out Vector2 point))
        {
            return false;
        }
        if (GetDistance2D(point, nearestPoint) > box.boxWidth)
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// circle
    /// </summary>
    /// <param name="box"></param>
    /// <param name="line"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this BoxData2D box, CircleData2D circle)
    {

        if (!CheckLineOut(box, circle.position, out Vector2 point))
        {

            return false;
        }
        if (GetDistance2D(point, circle.position) > box.boxWidth + circle.radius)
        {

            return false;
        }
        return true;
    }
    public static bool CheckCollision2D(this BoxData2D box, CapsuleData2D capsule)
    {
        Vector2 nearestPoint = GetNearestPoint2D(box.ToLine(), capsule.ToLine());
        if (CheckLineOut(box, nearestPoint, out Vector2 point))
        {
            return false;
        }
        if (GetDistance2D(point, nearestPoint) > box.boxWidth + capsule.radius)
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// box
    /// </summary>
    /// <param name="box"></param>
    /// <param name="capsule"></param>
    /// <returns></returns>
    public static bool CheckCollision2D(this BoxData2D box1, BoxData2D box2)
    {
        Vector2 nearestPoint = GetNearestPoint2D(box1.ToLine(), box2.ToLine());
        if (CheckLineOut(box1, nearestPoint, out Vector2 point))
        {
            return false;
        }
        if (GetDistance2D(point, nearestPoint) > box1.boxWidth + box2.boxWidth)
        {
            return false;
        }
        return true;
    }


    /// <summary>
    /// 点がラインの外か判別
    /// </summary>
    /// <param name="box"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    private static bool CheckLineOut(this BoxData2D box, Vector2 position)
    {
        //カプセルの始点から終点のベクトルを正規化したもの
        Vector2 vector1 = (box.endPoint - box.originPoint).normalized;
        //始点から点へのベクトル
        Vector2 originToPoint = position - box.originPoint;
        //終点から点へのベクトル
        Vector2 endToPoint = position - box.endPoint;

        if (0 > Vector2.Dot(vector1, originToPoint) || 0 < Vector2.Dot(vector1, endToPoint))
        {
            return false;

        }
        return true;
    }
    /// 点がラインの外か判別
    /// </summary>
    /// <param name="box"></param>
    /// <param name="position"></param>
    /// <returns></returns>
    private static bool CheckLineOut(this BoxData2D box, Vector2 position, out Vector2 nearestPoint)
    {
        //カプセルの始点から終点のベクトルを正規化したもの
        Vector2 vector1 = (box.endPoint - box.originPoint).normalized;
        //始点から点へのベクトル
        Vector2 originToPoint = position - box.originPoint;
        //終点から点へのベクトル
        Vector2 endToPoint = position - box.endPoint;

        if (0 > Vector2.Dot(vector1, originToPoint) || 0 < Vector2.Dot(vector1, endToPoint))
        {
            nearestPoint = default;
            return false;

        }
        nearestPoint = box.originPoint + vector1 * Vector2.Dot(vector1, originToPoint);
        return true;
    }
    #endregion

    //vector3のxzをvector2に変換する
    public static Vector2 ToVector2XZ(this Vector3 vector3)
    {

        return new Vector2(vector3.x, vector3.z);

    }


}
