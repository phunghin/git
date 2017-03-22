using UnityEngine;

public class SpeedFloatHelper
{
	// The current speed
	private float _speed;

	// The accelerate
	private float _accelerate;
	
	// The target
	private float _target;

	// The current value
	private float _current;

	// True if finished
	private bool _isFinished = true;

	public float Current
	{
		get { return _current; }
	}

	public float Target
	{
		get { return _target; }
	}

	public bool Finished
	{
		get { return _isFinished; }
	}

	public void Construct(float current, float speed, float accelerate, float target)
	{
		// Set speed
		_speed = speed;

		// Set accelerate
		_accelerate = accelerate;

		// Set target
		_target = target;

		// Set current value
		_current = current;

		// Set finished
		_isFinished = (_current == target);
	}

	public void Stop()
	{
		_isFinished = true;
	}

	public float Update()
	{
		if (_isFinished) return _target;

		// Update speed
		_speed += _accelerate;

		// Update value
		_current += _speed * Time.deltaTime;

		if (_speed < 0)
		{
			if (_current > _target)
			{
				return _current;
			}

			_isFinished = true;

			return _target;
		}
		else
		{
			if (_current < _target)
			{
				return _current;
			}

			_isFinished = true;

			return _target;
		}
	}
}
