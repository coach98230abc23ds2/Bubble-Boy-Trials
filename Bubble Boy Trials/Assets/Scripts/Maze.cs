using System;
using System.Collections.Generic;
using UnityEngine;

/*
 * Implements a maze as a 3-tree.  It was originall going to be a directed graph
 * but seemed simpler to make it a 3-tree with public children so that mazes
 * can be built up in the scene editor instead of having to be written out
 * programatically.
*/

public class MazeNode : MonoBehaviour
{
		public MazeNode Left_Node;
		public MazeNode Up_Node;
		public MazeNode Right_Node;

		public int Scene_Id;

		void Awake ()
		{
				GameObject.DontDestroyOnLoad (this);
		}
}