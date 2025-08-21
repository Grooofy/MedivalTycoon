using System;
using UnityEngine;


public class Point : MonoBehaviour
{
   public bool IsFill { get; private set; }

   public void Fill()
   {
      IsFill = true;
   }

   public void Free()
   {
      IsFill = false;
   }
}
