using UnityEngine;

[System.AttributeUsage(System.AttributeTargets.Field)]
public class Colored : PropertyAttribute
{
    public readonly Color color;

    public Colored(float r, float g, float b)
    {
        color = new Color(r, g, b);
    }
}