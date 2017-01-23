using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlateCheck : MonoBehaviour {

	[SerializeField]
	private Table table;

	[SerializeField]
	private Dish dish;

	void Start () {
		dish = DishManager.Instance.GetRandomDish;
	}

	void OnTriggerStay ( Collider other ) {
		
		var tmp = other.GetComponent<Plate> ();

		if ( tmp != null ) {
			if ( tmp.GetComponent<Pickable>().PickableState == Pickable.PickableStates.Dropped ) {
				CheckPlate (tmp);
			}
		}

	}

	private void CheckPlate (Plate plate) {

		foreach ( Client client in table.Clients ) {
			
			if ( client.WantedDish == null ) {
				Debug.Log ("pas de plat client");
				continue;
			}

			if ( plate.Dish == null ) {
				Debug.Log ("pas de plat assiete");
				continue;
			}

			bool match = true;

			List<Ingredient> tmp = client.WantedDish.Ingredients;

			foreach ( Ingredient ingredient in plate.Dish.Ingredients ) {
				int result = tmp.FindIndex (x => x == ingredient);
				if (result != -1)
					tmp.RemoveAt (result);
				else
					match = false;
			}

			if ( match )
				client.Serve (plate);
		}

	}

}
