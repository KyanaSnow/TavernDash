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

	void OnTriggerStay( Collider other ) {
		
		var tmp = other.GetComponent<Plate> ();

		if ( tmp != null ) {

//			Debug.Log (tmp.GetComponent<Pickable>().PickableState);
<<<<<<< HEAD
			if ( tmp.GetComponent<Pickable>().PickableState == Pickable.PickableStates.Dropped 
				|| tmp.GetComponent<Pickable>().PickableState == Pickable.PickableStates.Thrown) {
=======
			if ( tmp.GetComponent<Pickable>().PickableState == Pickable.PickableStates.Dropped ) {
>>>>>>> 1ac3a55293fb3196c09c14a4f99defedbb5d7d9f
				
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

			if (match)
				client.Serve (plate);
//			else
//				client.Dialogue.Speak ("je veux pas ça");
		}

	}

}
