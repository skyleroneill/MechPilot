using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions
{
    public static void SetX(this ref Vector3 vect, float newX)
    {
		vect = new Vector3(newX, vect.y, vect.z);
    }

    public static void SetY(this ref Vector3 vect, float newY)
    {
		vect = new Vector3(vect.x, newY, vect.z);
    }

    public static void SetZ(this ref Vector3 vect, float newZ)
    {
		vect = new Vector3(vect.x, vect.y, newZ);
    }

	public static void SetXYZ(this ref Vector3 vect, float newX, float newY, float newZ)
	{
		vect = new Vector3(newX, newY, newZ);
	}

	public static void SetXYZ(this ref Vector3 vect, float newXYZ)
    {
		vect = new Vector3(newXYZ, newXYZ, newXYZ);
    }

	public static void AddToX(this ref Vector3 vect3, float newX)
	{
		vect3 = new Vector3(newX + vect3.x, vect3.y, vect3.z);
	}
	public static void AddToY(this ref Vector3 vect3, float newY)
	{
		vect3 = new Vector3(vect3.x, newY + vect3.y, vect3.z);
	}
	public static void AddToZ(this ref Vector3 vect3, float newZ)
	{
		vect3 = new Vector3(vect3.x, vect3.y, newZ + vect3.z);
	}
	public static void AddToXYZ(this ref Vector3 vect3, float newX, float newY, float newZ)
	{
		vect3 = new Vector3(newX + vect3.x, newY + vect3.y, newZ + vect3.z);
	}
	public static void AddToXYZ(this ref Vector3 vect3, float newVal)
	{
		vect3 = new Vector3(newVal + vect3.x, newVal + vect3.y, newVal + vect3.z);
	}
	public static Vector3 ToVector3(this ref Vector2 vect2)
	{
		return new Vector3(vect2.x, 0, vect2.y);
	}
	public static Vector2 ToVector2(this ref Vector3 vect3)
	{
		return new Vector3(vect3.x, vect3.y);
	}
}
