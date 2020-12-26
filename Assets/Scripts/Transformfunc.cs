using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 using System.Linq;

public static class Transformfunc 
{
    public static void CenterOnChild(this Transform aParent)
     {
         var childs = aParent.Cast<Transform>().ToList();
         var pos = Vector3.zero;
         foreach(var C in childs)
         {
             pos += C.position;
             C.parent = null;
         }
         pos /= childs.Count;
         aParent.position = pos;
         foreach(var C in childs)
             C.parent = aParent;
     }    

     public static void ChangeChildTag(this Transform aParent)
     {
         var childs = aParent.Cast<Transform>().ToList(); 
         foreach(var C in childs)
         {
             C.tag = "Rocket";
         }
     }

     
 }
