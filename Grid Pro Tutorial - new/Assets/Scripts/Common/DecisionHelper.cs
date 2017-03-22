using UnityEngine;
using System.Collections.Generic;

public class DecisionHelper
{
	// The capacity
	protected int _capacity;

	// The number of choices
	protected int _choiceCount;

	// The array of weights
	protected float[] _weights;

	// The total weight
	protected float _totalWeight;

	// The stack of preferred choices
	protected Stack<int> _preferredChoices = new Stack<int>();

	public int ChoiceCount
	{
		get { return _choiceCount; }
	}

	public bool HasChoice
	{
		get { return _totalWeight > 0; }
	}

	public DecisionHelper(int capacity = 10)
	{
		// Set capacity
		_capacity = capacity;
		
		// Set number of choices
		_choiceCount = 0;
		
		// Create array of weights
		_weights = new float[capacity];

		// Set total weight
		_totalWeight = 0;
	}
	
	public bool AddChoice(float weight)
	{
//		Assert.IsTrue(weight >= 0, "E1234567800");

		if (_choiceCount < _capacity)
		{
			_weights[_choiceCount++] = weight;
			
			if (weight > 0)
			{
				_totalWeight += weight;
			}

			return true;
		}
		
		return false;
	}
	
	public void RemoveChoice(int index)
	{
//		Assert.IsInRange(index, 0, _choiceCount - 1, "E1234567801");

		if (_weights[index] > 0)
		{
			_totalWeight -= _weights[index];
		}

		_weights[index] = 0;
	}

	public float GetWeight(int index)
	{
//		Assert.IsInRange(index, 0, _choiceCount - 1, "E1234567802");

		return _weights[index];
	}
	
	public void SetWeight(int index, float weight)
	{
//		Assert.IsInRange(index, 0, _choiceCount - 1, "E1234567803");

		if (_weights[index] > 0)
		{
			_totalWeight -= _weights[index];
		}

		_weights[index] = weight;

		if (weight > 0)
		{
			_totalWeight += weight;
		}
	}

	public bool IsChoiceEnabled(int index)
	{
//		Assert.IsInRange(index, 0, _choiceCount - 1, "E1234567804");

		return _weights[index] > 0;
	}

	public void SetChoiceEnabled(int index, bool enabled)
	{
//		Assert.IsInRange(index, 0, _choiceCount - 1, "E1234567805");

		if (enabled)
		{
			if (_weights[index] < 0)
			{
				_weights[index] *= -1;
			}
		}
		else
		{
			if (_weights[index] > 0)
			{
				_weights[index] *= -1;
			}
		}
	}

	public void Push(int choice)
	{
		_preferredChoices.Push(choice);
	}

	public int Select()
	{
		if (_preferredChoices.Count > 0)
		{
			return _preferredChoices.Pop();
		}

		if (_totalWeight > 0)
		{
			float rand = Random.value * _totalWeight;

			for (int i = 0; i < _choiceCount; i++)
			{
				if (_weights[i] > 0)
				{
					if (rand <= _weights[i])
					{
						return i;
					}
					
					rand -= _weights[i];
				}
			}
		}
		
		return -1;
	}
}
