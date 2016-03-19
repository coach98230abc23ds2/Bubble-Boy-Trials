using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

/*

  This class represents a problem that will be generated for each turn of combat.
  It currently supports addition, multiplication, and subtraction questions,
  and is responsible for creating a list of possible solutions and storing
  the actual solution.

 */

public class Problem
{

		public enum Operator
		{
				PLUS,
				MINUS,
				TIMES
		};

		static int SOLUTION_COUNT = 4;
		static int MAX_OPERAND = 10;

		int m_operandFirst;
		int m_operandSecond;
		Operator m_operator;
		List<int> m_solutionList;

		public Problem (Operator problemType = Operator.PLUS)
		{
				m_operandFirst = Mathf.CeilToInt (Random.Range (0, 10));
				m_operandSecond = Mathf.CeilToInt (Random.Range (0, 10));
				m_operator = problemType;

				// now ensure that we don't end up giving negative solutions
				if (m_operator == Operator.MINUS && m_operandFirst < m_operandSecond) {
						int swap = m_operandFirst;
						m_operandFirst = m_operandSecond;
						m_operandSecond = swap;
				}
        
				m_solutionList = new List<int> (SOLUTION_COUNT);
        
				// fill solutionList with dummy solutions
				// making sure every value is unique
				for (int i = 0; i < SOLUTION_COUNT; ++i) {
						int possibleSolution;
						do {
								possibleSolution = Mathf.RoundToInt (Random.Range (0, MaxSolution ()));
						} while (m_solutionList.Contains (possibleSolution) || possibleSolution == GetSolution ());
                
						m_solutionList.Add (possibleSolution);
				}

				// now fill in a random position with the actual solution
				int rightIndex = Mathf.RoundToInt (Random.Range (0, SOLUTION_COUNT - 1));
				m_solutionList [rightIndex] = GetSolution ();

				// ensure our list is made of unique elements
				Assert.IsTrue (m_solutionList.TrueForAll (s => m_solutionList.IndexOf(s) == m_solutionList.LastIndexOf(s)));

		}	

		public int[] GetPossibleSolutions ()
		{
				return m_solutionList.ToArray ();
		}

		// disables warning about default case,
		// which we want to keep for safety
		#pragma warning disable 0162
		public override string ToString ()
		{
        
				// convert the operator to its string equivalent
				string operatorString = "";
				switch (m_operator) {
				case Operator.PLUS:
						operatorString = " + ";
						break;
				case Operator.MINUS:
						operatorString = " - ";
						break;
				case Operator.TIMES:
						operatorString = " * ";
						break;
				default:
						throw new System.Exception ("Unexpected operator value: " + m_operator.ToString ());
				}
        
				return m_operandFirst + operatorString + m_operandSecond + " = ?";
		}
		#pragma warning restore 0162

		// disables warning about default case,
		// which we want to keep for safety
		#pragma warning disable 0162
		public int GetSolution ()
		{
				switch (m_operator) {
				case Operator.PLUS:
						return m_operandFirst + m_operandSecond;
						break;
				case Operator.MINUS:
						return m_operandFirst - m_operandSecond;
						break;
				case Operator.TIMES:
						return m_operandFirst * m_operandSecond;
						break;
				default:
						throw new System.Exception ("Unexpected operator value: " + m_operator.ToString ());
				}
		}
		#pragma warning restore 0162

		public int GetSolutionIndex ()
		{
				Assert.IsTrue (m_solutionList.IndexOf(GetSolution()) < 4);
				return m_solutionList.IndexOf (GetSolution ());
		}

		// disables warning about default case,
		// which we want to keep for safety
		#pragma warning disable 0162
		private int MaxSolution ()
		{
				switch (m_operator) {
				case Operator.PLUS:
						return MAX_OPERAND + MAX_OPERAND;
						break;
				case Operator.MINUS:
						return MAX_OPERAND;
						break;
				case Operator.TIMES:
						return MAX_OPERAND * MAX_OPERAND;
						break;
				default:
						throw new System.Exception ("Unexpected operator value: " + m_operator.ToString ());
				}
		}
		#pragma warning restore 0162
}
