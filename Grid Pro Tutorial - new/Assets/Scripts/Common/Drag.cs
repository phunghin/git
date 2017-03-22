using UnityEngine;

public class Drag : MonoBehaviour, ITouchEventListener
{
	private static readonly Vector3 NormalScale   = Vector3.one;
	private static readonly Vector3 SelectedScale = new Vector3(1.1f, 1.1f, 1.0f);

	// Touch bounding box
	private float _left, _top, _right, _bottom;

	// The touch position
	private Vector3 _touchPosition;

	void Awake()
	{
		// Get sprite renderer
		SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
		if (spriteRenderer == null) return;

		// Get bounds
		Bounds bounds = spriteRenderer.sprite.bounds;

		// Set touch bounding box
		_left   = bounds.min.x;
		_bottom = bounds.min.y;
		_right  = bounds.max.x;
		_top    = bounds.max.y;
	}

	void Start()
	{
		TouchManager.Instance.AddEventListener(this);
	}

	void OnDestroy()
	{
		TouchManager.SafeRemoveEventListener(this);
	}

	public bool OnTouchPressed(Vector3 position)
	{
		float x = position.x - transform.position.x;
		float y = position.y - transform.position.y;

		// Check if touch inside
		if (x >= _left && x <= _right && y >= _bottom && y <= _top)
		{
			// Zoom-out
			transform.localScale = SelectedScale;

			// Save touch position
			_touchPosition = position;

			return true;
		}

		return false;
	}

	public bool OnTouchMoved(Vector3 position)
	{
		float deltaX = position.x - _touchPosition.x;
		float deltaY = position.y - _touchPosition.y;

		// Move
		Vector3 pos = transform.position;
		pos.x += deltaX;
		pos.y += deltaY;

		transform.position = pos;

		// Save touch position
		_touchPosition = position;

		return true;
	}

	public void OnTouchReleased(Vector3 position)
	{
		// Zoom-in
		transform.localScale = NormalScale;
	}
}
