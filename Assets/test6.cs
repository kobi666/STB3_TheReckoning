using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

public class test6 : SerializedMonoBehaviour
{

    [ValueDropdown("vdlist", DropdownWidth = 100)]
    [BoxGroup]
    [SerializeField]
    protected string DaEnum;
    
    [ShowInInspector]
    public static GameObject IenumPlaceHolder;
    
    public static List<IEnumerator> listofIenums()
    {
        string[] methodnames;
        int[] index;
        

        return null;
    }

    public static ValueDropdownList<IEnumerator> GetIEnumsFromObject(GameObject go)
    {
        List<IEnumerator> methods = new List<IEnumerator>();
        MemberInfo[] memberInfos = new MemberInfo[0];
        string err;
        if (go != null)
        {
            bool foundMembers = go.GetType().FindMember()
                .HasReturnType(typeof(IEnumerator))
                .TryGetMembers(out memberInfos, out err);
        }

        List<MemberInfo> meminfolist = memberInfos.ToList();
        List<IEnumerator> ienumList = new List<IEnumerator>();
        foreach (var VARIABLE in meminfolist)
        {
            ienumList.Add(VARIABLE.GetMemberValue(typeof(IEnumerator)) as IEnumerator);
        }

        ValueDropdownList<IEnumerator> vdlist = new ValueDropdownList<IEnumerator>();
        for (int i = 0; i < ienumList.Count; i++)
        {
            vdlist.Add(meminfolist[i].Name, ienumList[i]);
        }

        return vdlist;
    }

    public ValueDropdownList<IEnumerator> vdlist = GetIEnumsFromObject(IenumPlaceHolder);




}
