using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Extensions
{
    public static void SetX(this ref Vector2 vect, float newX)
    {
		vect = new Vector2(newX, vect.y);
    }

    public static void SetY(this ref Vector2 vect, float newY)
    {
		vect = new Vector2(vect.x, newY);
    }

    public static void SetZ(this ref Vector2 vect, float newZ)
    {
		vect = new Vector2(vect.x, vect.y);
    }

	public static void SetXYZ(this ref Vector2 vect, float newX, float newY, float newZ)
	{
		vect = new Vector2(newX, newY);
	}

	public static void SetXYZ(this ref Vector2 vect, float newXYZ)
    {
		vect = new Vector2(newXYZ, newXYZ);
    }

	public static void AddToX(this ref Vector2 vect, float newX)
	{
		vect = new Vector2(newX + vect.x, vect.y);
	}

	public static void AddToY(this ref Vector2 vect, float newY)
	{
		vect = new Vector2(vect.x, newY + vect.y);
	}

	public static void AddToXYZ(this ref Vector2 vect, float newX, float newY)
	{
		vect = new Vector2(newX + vect.x, newY + vect.y);
	}
	public static void AddToXYZ(this ref Vector2 vect, float newVal)
	{
		vect = new Vector2(newVal + vect.x, newVal + vect.y);
	}
}
