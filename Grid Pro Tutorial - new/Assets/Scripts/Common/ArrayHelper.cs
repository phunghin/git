using UnityEngine;
using System.Text;

public static class ArrayHelper
{
	public static int[] GetIndices(int length)
	{
		int[] a = new int[length];

		for (int i = 0; i < length; i++)
		{
			a[i] = i;
		}

		return a;
	}

	public static int[] SubArray(this int[] a, int count)
	{
		int[] sub = new int[count];

		for (int i = 0; i < count; i++)
		{
			sub[i] = a[i];
		}

		return sub;
	}

	public static void Swap(this float[] a)
	{
		int length = a.Length;
		float tmp;

		if (length < 2)
		{
			return;
		}

		if (length == 2)
		{
			if (Random.value > 0.5f)
			{
				tmp  = a[0];
				a[0] = a[1];
				a[1] = tmp;
			}

			return;
		}

		for (int i = 0; i < length; i++)
		{
			int j = Random.Range(0, length);

			if (i != j)
			{
				tmp  = a[i];
				a[i] = a[j];
				a[j] = tmp;
			}
		}
	}

	public static void Log(this int[,] a)
	{
		int rows    = a.GetLength(0);
		int columns = a.GetLength(1);

		StringBuilder sb = new StringBuilder();

		for (int i = 0; i < rows; i++)
		{
			sb.Append(a[i, 0]);

			for (int j = 1; j < columns; j++)
			{
				sb.AppendFormat("\t{0}", a[i, j]);
			}

			sb.AppendLine();
		}

		Debug.Log(sb.ToString());
	}
}
