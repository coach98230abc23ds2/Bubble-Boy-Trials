using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using NUnit.Framework;

public class PlayerTest
{

		[Test]
		public void PlayerTakeDamage ()
		{
				// instantiate player script
				GameObject gameObject = new GameObject ();
				gameObject.AddComponent (typeof(Player));

				Player player = gameObject.GetComponent (typeof(Player)) as Player;

				int oldHealth = player.Health;
				player.TakeDamage (10);
				int newHealth = player.Health;

				Assert.AreEqual (oldHealth - newHealth, 10);
		}
}
