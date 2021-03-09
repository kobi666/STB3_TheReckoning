using UnityEngine;

    public class UnitUtils : MonoBehaviour
    {

        // Start is called before the first frame update
        public static void MakeUnitSpriteLookRight(SpriteRenderer sr) {
            if (sr != null) {
                sr.flipX = false;
            }
        }

        public static void MakeUnitSpriteLookLeft(SpriteRenderer sr) {
            if (sr != null) {
                sr.flipX = true;
            }
        }

        

        
    }
