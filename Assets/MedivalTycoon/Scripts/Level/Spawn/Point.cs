using System;
using UnityEngine;


public class Point : MonoBehaviour
{
   public bool IsFill;
   public Action<bool> Filling;

   public void Fill()
   {
      IsFill = true;
      Filling?.Invoke(IsFill);
   }

   public void Free()
   {
      IsFill = false;
   }
}
