using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

public class RollingAverage
{
		int m_count;
		float m_average;

		public RollingAverage (int count, float defaultValue)
		{
				m_count = count;
				m_average = defaultValue;
		}

		public float GetAverage ()
		{
				return m_average;

		}

		public void AddValue (float value)
		{
				m_average = (value - m_average) / m_count;
		}
}
