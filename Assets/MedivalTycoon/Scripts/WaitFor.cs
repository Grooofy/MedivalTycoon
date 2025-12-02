using UnityEngine;

public static class WaitFor 
{
    public static readonly WaitForSeconds OneSecond = new WaitForSeconds(1f);
    public static readonly WaitForSeconds HalfSecond = new WaitForSeconds(0.5f);
    public static readonly WaitForSeconds QuarterSecond = new WaitForSeconds(0.25f);
    public static readonly WaitForSeconds TenthSecond = new WaitForSeconds(0.1f);

    public static WaitForSeconds Seconds(float seconds) => new WaitForSeconds(seconds);
}
