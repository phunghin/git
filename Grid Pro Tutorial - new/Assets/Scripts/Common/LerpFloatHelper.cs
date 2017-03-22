using UnityEngine;

public class LerpFloatHelper
{
	// The start value
	protected float _start;

	// The end value
	protected float _end;

	// The delta value
	protected float _delta;

	// The current value
	protected float _value;

	// The duration
	private float _duration;

	// The easer
	private Easer _easer;

	// The accumulative time
	private float _time;

	// True if finished
	private bool _isFinished = true;

	public float Start
	{
		get { return _start; }
		set { _start = value; }
	}

	public float End
	{
		get { return _end; }
		set { _end = value; }
	}

	public float Current
	{
		get { return _value; }
	}

	public bool Finished
	{
		get { return _isFinished; }
	}

	public void Construct(float start, float end, float duration, Easer easer = null)
	{
		// Set start value
		_start = start;

		// Set end value
		_end = end;

		// Set duration
		_duration = duration;

		// Set easer
		_easer = easer ?? Ease.Linear;
	}

	public void Play()
	{
		// Set delta value
		_delta = _end - _start;

		// Set current value
		_value = _start;

		// Reset time
		_time = 0;

		// Not finished
		_isFinished = false;
	}

	public void Stop()
	{
		_isFinished = true;
	}

	public float Update(float deltaTime)
	{
		if (_isFinished)
		{
			return _value;
		}
     
        _time += deltaTime;

        if (_time < _duration)
		{
			_value = _start + _delta * _easer(_time / _duration);
		}
		else
		{
			// Set value to end
			_value = _end;

			// Set finished
			_isFinished = true;
		}

		return _value;
	}
}
