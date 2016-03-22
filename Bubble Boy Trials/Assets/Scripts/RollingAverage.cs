using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

public class RollingAverage
{
		// the average
		float m_average;

		// stores the array of values
		float[] m_values;

		// points to the oldest value
		// so it can be replaced next
		int m_index;

		public RollingAverage (int count, float defaultValue)
		{
				Assert.IsTrue (count > 0);
				m_values = new float[count];
				m_index = 0;

				// initialize to a default value
				// so that initial additions occur slowly
				m_average = defaultValue;
				for (int i = 0; i < m_values.Length; i++) {
						m_values[i] = defaultValue;
				}
		}

		public float GetAverage ()
		{
				return m_average;
		}

		public void AddValue (float value)
		{
				// the new average is the same as swapping the difference between new
				// and old values
				m_average += (value - m_values [m_index]) / m_values.Length;

				m_values [m_index] = value;
				m_index++;

				// reset index if at the end of the list
				if (m_index >= m_values.Length) {
						m_index = 0;
				}
		}
}
