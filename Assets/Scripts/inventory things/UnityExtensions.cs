using UnityEngine;

// c# extension methods
public static class UnityExtensions
{
    public static void DestroyChildrens(this Transform parent)
    {
        for (int i = parent.childCount - 1; i >= 0; i--)
        {
            var child = parent.GetChild(i);
            Object.Destroy(child.gameObject);
        }
    }
}
