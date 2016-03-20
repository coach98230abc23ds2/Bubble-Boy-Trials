using UnityEngine;
using UnityEditor;
using NUnit.Framework;

public class RollingAverageTest
{

		[Test]
		public void AverageDefaultSets ()
		{
				RollingAverage rollingTest = new RollingAverage (5, 5);
				Assert.AreEqual(rollingTest.GetAverage(), 5);
		}

		[Test]
		public void AverageAddWorks ()
		{
				RollingAverage rollingTest = new RollingAverage (5, 0);

				// ensure the first values add correctly
				rollingTest.AddValue (1);
				rollingTest.AddValue (2);
				rollingTest.AddValue (3);
				Assert.AreEqual (rollingTest.GetAverage (), (1 + 2 + 3) / 5f);

				rollingTest.AddValue (4);
				rollingTest.AddValue (5);
				Assert.AreEqual (rollingTest.GetAverage (), (1 + 2 + 3 + 4 + 5) / 5f);

				// ensure rolling through the array also works
				rollingTest.AddValue (6);
				rollingTest.AddValue (7);
				rollingTest.AddValue (8);
				rollingTest.AddValue (9);
				Assert.AreEqual (rollingTest.GetAverage (), (5 + 6 + 7 + 8 + 9) / 5f);
		}
}
