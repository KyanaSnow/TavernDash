using UnityEngine;
using System.Collections;

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
			if (plate.Dish == client.WantedDish) {
				client.Serve (plate);
			}
		}

	}

}
