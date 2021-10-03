using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Object = UnityEngine.Object;

public abstract class LootActions : CursorActions<LootAction,LootObject>
{
    public override IEnumerable<Type> GetActions()
    {
        {
            var q = typeof(LootAction).Assembly.GetTypes()
                .Where(x => !x.IsAbstract) // Excludes BaseClass
                .Where(x => !x.IsGenericTypeDefinition) // Excludes C1<>
                .Where(x => x.IsSubclassOf(typeof(LootAction)));
        
            return q;
        }
    }
}

public class LootItemActions : LootActions
{
    
    public override LootAction Action_North { get => null;
        set { }
    }
    public override LootAction Action_East { get => null;
        set { }
    }
    public override LootAction Action_South { get => null;
        set { }
    }
    public override LootAction Action_West { get => null;
        set { }
    }
}


