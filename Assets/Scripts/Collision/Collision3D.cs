using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class Collision3D
{
    #region 計算
    private static Vector3 GetNearestPoint3D(Vector3 origin, Vector3 end, Vector3 position)
    {
        //カプセルの始点から終点のベクトルを正規化したもの
        Vector3 vector1 = (end - origin).normalized;
        //始点から点へのベクトル
        Vector3 originToPoint = position - origin;
        //終点から点へのベクトル
        Vector3 endToPoint = position - end;

        if (0 > Vector3.Dot(vector1, originToPoint))
        {

            return origin;
        }
        else if (0 < Vector3.Dot(vector1, endToPoint))
        {
            return end;
        }

        Vector3 point = origin + vector1 * Vector3.Dot(vector1, originToPoint);

        return point;
    }
    private static Vector3 GetNearestPoint3D(LineData3D lineData1, LineData3D lineData2)
    {
        Vector3 originNearPoint = GetNearestPoint3D(lineData1.originPoint, lineData1.endPoint, lineData2.originPoint);
        float originDistance = GetDistance3D(originNearPoint, lineData2.originPoint);
        Vector3 endNearPoint = GetNearestPoint3D(lineData1.originPoint, lineData1.endPoint, lineData2.endPoint);
        float endDistance = GetDistance3D(endNearPoint, lineData2.endPoint);
        if (originDistance <= endDistance)
        {
            return originNearPoint;
        }
        return endNearPoint;
    }


    private static float GetDistance3D(Vector3 point1, Vector3 point2)
    {

        Vector3 distanceVec = point1 - point2;
        float distance = Mathf.Sqrt(
            Mathf.Pow(Mathf.Abs(distanceVec.x), 2) +
            Mathf.Pow(Mathf.Abs(distanceVec.y), 2)+
            Mathf.Pow(Mathf.Abs(distanceVec.z),2)
            );
        return distance;
    }
    private static float GetLinesDistance3D(LineData3D lineData1, LineData3D lineData2)
    {
        Vector3 originNearPoint = GetNearestPoint3D(lineData1.originPoint, lineData1.endPoint, lineData2.originPoint);
        float originDistance = GetDistance3D(originNearPoint, lineData2.originPoint);
        Vector3 endNearPoint = GetNearestPoint3D(lineData1.originPoint, lineData1.endPoint, lineData2.endPoint);
        float endDistance = GetDistance3D(endNearPoint, lineData2.endPoint);
        if (originDistance <= endDistance)
        {
            return originDistance;
        }
        return endDistance;
    }
    #endregion
    #region capsule
    public static bool CheckCollision3D(this CapsuleData3D capsuleData, IBaseCollisionData3D collisionData)
    {
        switch (collisionData)
        {
            case CircleData3D circle:
                {
                    return capsuleData.CheckCollision3D(circle);
                }
            case CapsuleData3D capsule:
                {
                    return capsuleData.CheckCollision3D(capsule);
                }
            case LineData3D line:
                {
                    return capsuleData.CheckCollision3D(line);
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
    public static bool CheckCollision3D(this CapsuleData3D capsuleData, Vector3 position)
    {
        Vector3 point = GetNearestPoint3D(capsuleData.originPoint, capsuleData.endPoint, position);
        //Debug.Log(point);
        float distance = GetDistance3D(position, point);
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
    public static bool CheckCollision3D(this CapsuleData3D capsuleData, CircleData3D circleData)
    {
        Vector3 point = GetNearestPoint3D(capsuleData.originPoint, capsuleData.endPoint, circleData.position);
        //Debug.Log(point);
        float distance = GetDistance3D(circleData.position, point);
        if (distance - (capsuleData.radius + circleData.radius) <= 0)
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
    public static bool CheckCollision3D(this CapsuleData3D capsuleData, LineData3D lineData)
    {

        float distance = GetLinesDistance3D(capsuleData.ToLine(), lineData);

        if (distance - (capsuleData.radius + capsuleData.radius) <= 0)
        {

            return true;
        }
        return false;
    }/// <summary>
     /// capsule
     /// </summary>
     /// <param name="capsuleData"></param>
     /// <param name="circleData"></param>
     /// <returns></returns>
    public static bool CheckCollision3D(this CapsuleData3D capsuleData1, CapsuleData3D capsuleData2)
    {
        
        float distance = GetLinesDistance3D(capsuleData1.ToLine(), capsuleData2.ToLine());
        
        if (distance - (capsuleData1.radius + capsuleData2.radius) <= 0)
        {

            return true;
        }
        return false;
    }
    public static bool CheckCollision3D(this CapsuleData3D capsule, BoxData3D box)
    {
        Vector3 nearestPoint = GetNearestPoint3D(box.ToLine(), capsule.ToLine());
        if (CheckLineOut(box, nearestPoint, out Vector3 point))
        {
            Debug.Log("p2");
            return false;
        }
        if (GetDistance3D(point, nearestPoint) > box.boxWidth + capsule.radius)
        {
            Debug.Log("p1");
            return false;
        }
        return true;
    }
    #endregion
    #region circle
    public static bool CheckCollision3D(this CircleData3D circleData, IBaseCollisionData3D collisionData)
    {
        switch (collisionData)
        {
            case CircleData3D circle:
                {
                    return circleData.CheckCollision3D(circle);
                }
            case CapsuleData3D capsule:
                {
                    return circleData.CheckCollision3D(capsule);
                }
            case LineData3D line:
                {
                    return circleData.CheckCollision3D(line);
                }
            case BoxData3D box:
                {
                    return circleData.CheckCollision3D(box);
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
    public static bool CheckCollision3D(this CircleData3D circleData, CircleData3D circleData1)
    {


        float distance = GetDistance3D(circleData.position, circleData1.position);

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
    public static bool CheckCollision3D(this CircleData3D circleData, Vector3 point)
    {

        //Debug.Log(point);
        float distance = GetDistance3D(circleData.position, point);
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
    public static bool CheckCollision3D(this CircleData3D circleData, LineData3D lineData)
    {

        Vector3 point = GetNearestPoint3D(lineData.originPoint, lineData.endPoint, circleData.position);
        float distance = GetDistance3D(circleData.position, point);
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
    public static bool CheckCollision3D(this CircleData3D circleData, CapsuleData3D capsuleData)
    {

        Vector3 point = GetNearestPoint3D(capsuleData.originPoint, capsuleData.endPoint, circleData.position);
        float distance = GetDistance3D(circleData.position, point);
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
    public static bool CheckCollision3D(this CircleData3D circle, BoxData3D box)
    {
        if (!CheckLineOut(box, circle.position, out Vector3 point))
        {
            return false;
        }
        if (GetDistance3D(point, circle.position) > box.boxWidth + circle.radius)
        {
            return false;
        }
        return true;
    }
    #endregion
    #region line
    public static bool CheckCollision3D(this LineData3D lineData, IBaseCollisionData3D collisionData)
    {
        switch (collisionData)
        {
            case CircleData3D circle:
                {
                    return lineData.CheckCollision3D(circle);
                }
            case CapsuleData3D capsule:
                {
                    return lineData.CheckCollision3D(capsule);
                }
            case LineData3D line:
                {
                    return lineData.CheckCollision3D(line);
                }
            case BoxData3D box:
                {
                    return lineData.CheckCollision3D(box);
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
    public static bool CheckCollision3D(this LineData3D lineData, Vector3 position)
    {
        Vector3 point = GetNearestPoint3D(lineData.originPoint, lineData.endPoint, position);
        Debug.Log(point);
        float distance = GetDistance3D(position, point);
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
    public static bool CheckCollision3D(this LineData3D lineData, CircleData3D circleData)
    {
        Vector3 point = GetNearestPoint3D(lineData.originPoint, lineData.endPoint, circleData.position);
        Debug.Log(point);
        float distance = GetDistance3D(circleData.position, point);
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
    public static bool CheckCollision3D(this LineData3D lineData, CapsuleData3D capsuleData)
    {

        float distance = GetLinesDistance3D(lineData, capsuleData.ToLine());
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
    public static bool CheckCollision3D(this LineData3D lineData1, LineData3D lineData2)
    {

        float distance = GetLinesDistance3D(lineData1, lineData2);
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
    public static bool CheckCollision3D(this LineData3D lineData, BoxData3D box)
    {

        Vector3 nearestPoint = GetNearestPoint3D(box.ToLine(), lineData);
        if (CheckLineOut(box, nearestPoint, out Vector3 point))
        {
            return false;
        }
        if (GetDistance3D(point, nearestPoint) > box.boxWidth)
        {
            return false;
        }
        return true;
    }
    #endregion
    #region box
    public static bool CheckCollision3D(this BoxData3D boxData, IBaseCollisionData3D collisionData)
    {
        switch (collisionData)
        {
            case CircleData3D circle:
                {
                    return boxData.CheckCollision3D(circle);
                }
            case CapsuleData3D capsule:
                {
                    return boxData.CheckCollision3D(capsule);
                }
            case LineData3D line:
                {
                    return boxData.CheckCollision3D(line);
                }
            case BoxData3D box:
                {
                    return boxData.CheckCollision3D(box);
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
    public static bool CheckCollision3D(this BoxData3D box, Vector3 position)
    {

        if (!CheckLineOut(box, position, out Vector3 point))
        {
            return false;
        }
        if (GetDistance3D(point, position) > box.boxWidth)
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
    public static bool CheckCollision3D(this BoxData3D box, LineData3D line)
    {
        Vector3 nearestPoint = GetNearestPoint3D(box.ToLine(), line);
        if (CheckLineOut(box, nearestPoint, out Vector3 point))
        {
            return false;
        }
        if (GetDistance3D(point, nearestPoint) > box.boxWidth)
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
    public static bool CheckCollision3D(this BoxData3D box, CircleData3D circle)
    {

        if (!CheckLineOut(box, circle.position, out Vector3 point))
        {

            return false;
        }
        if (GetDistance3D(point, circle.position) > box.boxWidth + circle.radius)
        {

            return false;
        }
        return true;
    }
    public static bool CheckCollision3D(this BoxData3D box, CapsuleData3D capsule)
    {
        
        Vector3 nearestPoint = GetNearestPoint3D(capsule.ToLine(), box.ToLine());
        
        if (!CheckLineOut(box, nearestPoint, out Vector3 point))
        {
            
            return false;
        }
        float line = box.boxWidth + capsule.radius;
        float dis = GetDistance3D(point, nearestPoint);
        if (GetDistance3D(point, nearestPoint) > box.boxWidth + capsule.radius)
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
    public static bool CheckCollision3D(this BoxData3D box1, BoxData3D box2)
    {
        Vector3 nearestPoint = GetNearestPoint3D(box1.ToLine(), box2.ToLine());
        if (CheckLineOut(box1, nearestPoint, out Vector3 point))
        {
            return false;
        }
        if (GetDistance3D(point, nearestPoint) > box1.boxWidth + box2.boxWidth)
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
    private static bool CheckLineOut(this BoxData3D box, Vector3 position)
    {
        //カプセルの始点から終点のベクトルを正規化したもの
        Vector3 vector1 = (box.endPoint - box.originPoint).normalized;
        //始点から点へのベクトル
        Vector3 originToPoint = position - box.originPoint;
        //終点から点へのベクトル
        Vector3 endToPoint = position - box.endPoint;

        if (0 > Vector3.Dot(vector1, originToPoint) || 0 < Vector3.Dot(vector1, endToPoint))
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
    private static bool CheckLineOut(this BoxData3D box, Vector3 position, out Vector3 nearestPoint)
    {
        //カプセルの始点から終点のベクトルを正規化したもの
        Vector3 vector1 = (box.endPoint - box.originPoint).normalized;
        //始点から点へのベクトル
        Vector3 originToPoint = position - box.originPoint;
        //終点から点へのベクトル
        Vector3 endToPoint = position - box.endPoint;

        if (0 > Vector3.Dot(vector1, originToPoint) || 0 < Vector3.Dot(vector1, endToPoint))
        {
            
            nearestPoint = default;
            return false;

        }
        
        nearestPoint = box.originPoint + vector1 * Vector3.Dot(vector1, originToPoint);
        return true;
    }
    #endregion
}
