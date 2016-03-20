using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using NUnit.Framework;

public class ProblemTest
{

		[Test]
		public void TestProblemSolutionIndexInRange ()
		{
				Problem testProblem = new Problem ();
				Assert.IsTrue (testProblem.GetSolutionIndex() < Problem.SOLUTION_COUNT);
		}

		[Test]
		public void TestProblemSolutionsUnique ()
		{
				Problem testProblem = new Problem ();
				List<int> solutions = new List<int>(testProblem.GetPossibleSolutions ());
				Assert.IsTrue (solutions.TrueForAll (s => solutions.IndexOf(s) == solutions.LastIndexOf(s)));
		}

		[Test]
		public void TestProblemPlusSolutionCorrect ()
		{
				Problem testProblem = new Problem (Problem.Operator.PLUS, 3, 6);
				Assert.AreEqual (testProblem.GetSolution (), 3 + 6);
		}

		[Test]
		public void TestProblemMinusSolutionCorrect ()
		{
				Problem testProblem = new Problem (Problem.Operator.MINUS, 6, 3);
				Assert.AreEqual (testProblem.GetSolution (), 6 - 3);
		}

		[Test]
		public void TestProblemTimesSolutionCorrect ()
		{
				Problem testProblem = new Problem (Problem.Operator.TIMES, 6, 3);
				Assert.AreEqual (testProblem.GetSolution (), 6 * 3);
		}

		[Test]
		public void TestProblemPlusToStringCorrect ()
		{
				Problem testProblem = new Problem (Problem.Operator.PLUS, 3, 6);
				Assert.AreEqual (testProblem.ToString(), "3 + 6 = ?");
		}

		[Test]
		public void TestProblemMinusToStringCorrect ()
		{
				Problem testProblem = new Problem (Problem.Operator.MINUS, 6, 3);
				Assert.AreEqual (testProblem.ToString (), "6 - 3 = ?");
		}

		[Test]
		public void TestProblemTimesToStringCorrect ()
		{
				Problem testProblem = new Problem (Problem.Operator.TIMES, 6, 3);
				Assert.AreEqual (testProblem.ToString (), "6 * 3 = ?");
		}


}
