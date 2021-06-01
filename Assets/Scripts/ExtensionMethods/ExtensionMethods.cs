using UnityEngine;
using System;

public static class ExtensionMethods {

    public static bool Contains2d(this BoxCollider2D BoxCollider2D, Vector2 TargetPosition)
    {
        bool contains = false;
        Vector2 position = BoxCollider2D.transform.position;
        float Xsize = BoxCollider2D.size.x / 2f;
        float Ysize = BoxCollider2D.size.y / 2f;
        float maxX = position.x + Xsize;
        float maxY = position.y + Ysize;
        float minX = position.x - Xsize;
        float minY = position.y - Ysize;
        if (TargetPosition.x <= maxX && TargetPosition.y <= maxY && TargetPosition.x >= minX &&
            TargetPosition.y >= minY)
        {
            contains = true;
        }

        return contains;

    }
    
    public static bool Contains2d(this BoxCollider2D BoxCollider2D, Vector2 TargetPosition, bool _Debug)
    {
        bool contains = false;
        Vector2 position = BoxCollider2D.transform.position;
        float Xsize = BoxCollider2D.size.x / 2f;
        float Ysize = BoxCollider2D.size.y / 2f;
        float maxX = position.x + Xsize;
        float maxY = position.y + Ysize;
        float minX = position.x - Xsize;
        float minY = position.y - Ysize;
        if (TargetPosition.x <= maxX && TargetPosition.y <= maxY && TargetPosition.x >= minX &&
            TargetPosition.y >= minY)
        {
            Debug.DrawLine(new Vector2(minX,maxY),new Vector2(maxX,maxY),Color.black,20f);
            Debug.DrawLine(new Vector2(maxX,maxY),new Vector2(maxX,minY),Color.black,20f);
            Debug.DrawLine(new Vector2(maxX,minY),new Vector2(minX,minY),Color.black,20f);
            Debug.DrawLine(new Vector2(minX,minY),new Vector2(minX,maxY),Color.black,20f);
            contains = true;
        }

        return contains;

    }
}
