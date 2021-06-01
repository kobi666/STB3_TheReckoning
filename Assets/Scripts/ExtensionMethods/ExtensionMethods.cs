using UnityEngine;
using System;

public static class ExtensionMethods {

    public static bool Contains2d(this BoxCollider2D BoxCollider2D, Vector2 TargetPosition)
    {
        bool contains = false;
        Vector2 position = BoxCollider2D.transform.position;
        float Xsize = BoxCollider2D.size.x / 2f;
        float Ysize = BoxCollider2D.size.y;
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
}
