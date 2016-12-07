using UnityEngine;
using System.Collections;

public class DishManager : MonoBehaviour {

	int[] getDish () {


		int amount = Random.Range (1, 4);

		int[] dish = new int[amount];

		for ( int i = 0; i < amount ; i++ ) {

			dish [i] = Random.Range ( 0,1 );

		}

		return dish;

	}
}
