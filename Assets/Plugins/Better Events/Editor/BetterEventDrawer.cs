using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class BetterEventDrawer : OdinValueDrawer<BetterEventEntry>
{
    private UnityEngine.Object tmpTarget;
    private int tmpInt = 0;
    private Type tempType = typeof(NoType);

    private static List<int> someList = new List<int>()
    {
        1,
        2
    };
    
    public static List<Type> SupportedReturnTypes = new List<Type>()
    {
        typeof(NoType),
        typeof(int),
        typeof(string),
        typeof(IEnumerator),
        typeof(LineRenderer),
        typeof(int[])
    };

    
    

    protected override void DrawPropertyLayout(GUIContent label)
    {
        SirenixEditorGUI.BeginBox();
        {
            SirenixEditorGUI.BeginToolbarBoxHeader();
            {
                var rect = GUILayoutUtility.GetRect(0, 19);
                var unityObjectFieldRect = rect.Padding(2).AlignLeft(rect.width / 2);
                var methodSelectorRect = rect.Padding(2).AlignRight(rect.width / 2 - 5);
                var dInfo = this.GetDelegateInfo();
                var intlabel = new GUIContent("some type");

                EditorGUI.BeginChangeCheck();
                var someType = SirenixEditorFields.Dropdown(intlabel, tempType , SupportedReturnTypes);
                /*public static bool Dropdown<T>(Rect rect, IList<int> selected, IList<T> items, bool multiSelection)*/
                var newTarget = SirenixEditorFields.UnityObjectField(unityObjectFieldRect, dInfo.Target, typeof(UnityEngine.Object), true);
                if (EditorGUI.EndChangeCheck())
                {
                    this.tmpTarget = newTarget;
                    this.tempType = someType;
                }

                EditorGUI.BeginChangeCheck();
                var selectorText = (dInfo.Method == null || this.tmpTarget) ? "Select a method" : dInfo.Method.Name;
                var newMethod = MethodSelector.DrawSelectorDropdown(methodSelectorRect, selectorText, this.CreateSelector);
                if (EditorGUI.EndChangeCheck())
                {
                    this.CreateAndAssignNewDelegate(newMethod.FirstOrDefault());
                    this.tmpTarget = null;
                }
            }
            SirenixEditorGUI.EndToolbarBoxHeader();

            // Draws the rest of the ICustomEvent, and since we've drawn the label, we simply pass along null.
            for (int i = 0; i < this.Property.Children.Count; i++)
            {
                var child = this.Property.Children[i];
                if (child.Name == "Result") continue;
                child.Draw();
            }
        }
        SirenixEditorGUI.EndBox();
    }

    private void CreateAndAssignNewDelegate(DelegateInfo delInfo)
    {
        var method = delInfo.Method;
        var target = delInfo.Target;
        var pTypes = method.GetParameters().Select(x => x.ParameterType).ToArray();
        var args = new object[pTypes.Length];

        Type delegateType = null;

        if (method.ReturnType == typeof(void))
        {
            if (args.Length == 0) delegateType = typeof(Action);
            else if (args.Length == 1) delegateType = typeof(Action<>).MakeGenericType(pTypes);
            else if (args.Length == 2) delegateType = typeof(Action<,>).MakeGenericType(pTypes);
            else if (args.Length == 3) delegateType = typeof(Action<,,>).MakeGenericType(pTypes);
            else if (args.Length == 4) delegateType = typeof(Action<,,,>).MakeGenericType(pTypes);
            else if (args.Length == 5) delegateType = typeof(Action<,,,,>).MakeGenericType(pTypes);
        }
        else
        {
            pTypes = pTypes.Append(method.ReturnType).ToArray();
            if (args.Length == 0) delegateType = typeof(Func<>).MakeArrayType();
            else if (args.Length == 1) delegateType = typeof(Func<,>).MakeGenericType(pTypes);
            else if (args.Length == 2) delegateType = typeof(Func<,,>).MakeGenericType(pTypes);
            else if (args.Length == 3) delegateType = typeof(Func<,,,>).MakeGenericType(pTypes);
            else if (args.Length == 4) delegateType = typeof(Func<,,,,>).MakeGenericType(pTypes);
            else if (args.Length == 5) delegateType = typeof(Func<,,,,,>).MakeGenericType(pTypes);
            else if (args.Length == 6) delegateType = typeof(Func<,,,,,,>).MakeGenericType(pTypes);
            else if (args.Length == 7) delegateType = typeof(Func<,,,,,,,>).MakeGenericType(pTypes);
            else if (args.Length == 8) delegateType = typeof(Func<,,,,,,,,>).MakeGenericType(pTypes);
            else if (args.Length == 9) delegateType = typeof(Func<,,,,,,,,,>).MakeGenericType(pTypes);
        }
        
        if (delegateType == null)
        {
            Debug.LogError("Unsupported Method Type");
            return;
        }

        var del = Delegate.CreateDelegate(delegateType, target, method);
        this.Property.Tree.DelayActionUntilRepaint(() =>
        {
            this.ValueEntry.WeakSmartValue = new BetterEventEntry(del);
            GUI.changed = true;
            this.Property.RefreshSetup();
        });
    }

    private DelegateInfo GetDelegateInfo()
    {
        var value = this.ValueEntry.SmartValue;
        var del = value.Delegate;
        var methodInfo = del == null ? null : del.Method;

        UnityEngine.Object target = null;
        if (this.tmpTarget)
        {
            target = this.tmpTarget;
        }
        else if (del != null)
        {
            target = del.Target as UnityEngine.Object;
        }

        return new DelegateInfo() { Target = target, Method = methodInfo };
    }

    private OdinSelector<DelegateInfo> CreateSelector(Rect arg)
    {
        arg.xMin -= arg.width;
        var info = this.GetDelegateInfo();
        var sel = new MethodSelector(info.Target, tempType);
        sel.SetSelection(info);
        sel.ShowInPopup(arg);
        return sel;
    }
    
    
}
