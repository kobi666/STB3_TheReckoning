    using System;
    using System.Collections;
    using System.Collections.Generic;
    using Sirenix.OdinInspector;
    using Sirenix.Serialization;
    using UnityEngine;
    using System.Linq;
    using System.Reflection;
    using Sirenix.Utilities;


    [System.Serializable]
    public class ProjectileAttacks : SerializedMonoBehaviour
    {
        [ShowInInspector]
        public ShootOneProjectile test1 = new ShootOneProjectile();
        
        
        [ValueDropdown("Atypes", DropdownWidth = 500)] [HideLabel] [BoxGroup] [SerializeField] [ShowInInspector]
        public static List<MethodInfo> methods = new List<MethodInfo>();

        private static ValueDropdownList<Func<PoolObjectQueue<GenericProjectile>, Effectable, Vector2,
            Vector2, Quaternion, float, float, IEnumerator>> valueList =
            new ValueDropdownList<Func<PoolObjectQueue<GenericProjectile>, Effectable, Vector2,
                Vector2, Quaternion, float, float, IEnumerator>>()
            {
                {"Single Projectile", FireSingleProjectile},
                {"single proj 2", FireSingleProjectile2}
            };

        public static IEnumerator FireSingleProjectile(PoolObjectQueue<GenericProjectile> pool, Effectable targetEffectable,
            Vector2 originPosition,
            Vector2 targetPosition, Quaternion direction, float assistingfloat1, float assistingFloat2)
        {
            GenericProjectile proj = pool.GetInactive();
            proj.TargetPosition = targetPosition;
            proj.transform.position = originPosition;
            proj.EffectableTarget = targetEffectable;
            proj.transform.rotation = direction;
            proj.Activate();
            yield break;
        }

        
        
        public static IEnumerator FireSingleProjectile2(PoolObjectQueue<GenericProjectile> pool, Effectable targetEffectable,
            Vector2 originPosition,
            Vector2 targetPosition, Quaternion direction, float assistingfloat1, float assistingFloat2)
        {
            GenericProjectile proj = pool.GetInactive();
            proj.TargetPosition = targetPosition;
            proj.transform.position = originPosition;
            proj.EffectableTarget = targetEffectable;
            proj.transform.rotation = direction;
            proj.Activate();
            yield break;
        }

        


        public static ValueDropdownList<MethodInfo> Atypes()
        {
            IList<Type> paramTypes = new List<Type>()
            {
                typeof(PoolObjectQueue<GenericProjectile>),
                typeof(Effectable),
                typeof(Vector2),
                typeof(Vector2),
                typeof(Quaternion),
                typeof(float),
                typeof(float)
            };
            ValueDropdownList<MethodInfo> list = new ValueDropdownList<MethodInfo>();
            var methods = typeof(ProjectileAttacks).GetMethods()
                .Where(x => x.ReturnType == typeof(IEnumerator))
                .Where(x => x.HasParamaters(paramTypes))
                .Select(x => x).ToList();

            foreach (var method in methods)
            {
                list.Add(method.Name, method);
            }
            return list;
        }
        
        public static ValueDropdownList<MethodInfo> AtypesFromObject(GameObject go)
        {
            ValueDropdownList<MethodInfo> list = new ValueDropdownList<MethodInfo>();
            if (go == null)
            {
                return list;
            }
            IList<Type> paramTypes = new List<Type>()
            {
                typeof(PoolObjectQueue<GenericProjectile>),
                typeof(Effectable),
                typeof(Vector2),
                typeof(Vector2),
                typeof(Quaternion),
                typeof(float),
                typeof(float)
            };

            List<Component> allComponents = new List<Component>();
            List<MethodInfo> allMethods = new List<MethodInfo>();
            foreach (var component in allComponents)
            {
                component.GetType().GetMethods().ToList().Concat(allMethods);
            }
            
            
            var methods = allMethods
                .Where(x => x.ReturnType == typeof(IEnumerator))
                .Where(x => x.HasParamaters(paramTypes))
                .Select(x => x).ToList();

            foreach (var method in methods)
            {
                list.Add(method.Name, method);
            }
            return list;
        }
        
    }

    



 



