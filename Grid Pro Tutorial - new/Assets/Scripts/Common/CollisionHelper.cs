using UnityEngine;

public static class CollisionHelper
{
	public static bool IsTag(this Collision2D collision, string tag)
	{
		return collision.gameObject.tag == tag;
	}

    public static bool IsTag(this Collider2D collision, string tag)
    {
        return collision.gameObject.tag == tag;
    }

    public static float GetWidth(this Collision2D collision)
	{
		return collision.collider.bounds.size.x;
	}
}
