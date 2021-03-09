﻿using UnityEngine;

public class PlayerControllerUtilis 
{
    public static void MoveInDirection(Transform self, Vector2 Direction, float speed) {
        self.position = Vector2.MoveTowards(self.position, (Vector2)self.position + Direction, StaticObjects.DeltaGameTime * speed );
    }
    
}
