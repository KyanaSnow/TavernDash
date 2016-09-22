using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TableManager : MonoBehaviour {

	public static TableManager Instance;

	private List<Table> tables = new List<Table>();

	void Awake () {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public Table GetTable () {

		List<Table> tmpsTables = new List<Table> ();

		int n = tmpsTables.Count;  
		while (n > 1) {  
			n--;  
			int k = Random.Range (0, n + 1);
			Table value = tmpsTables[k];  
			tmpsTables[k] = tmpsTables[n];  
			tmpsTables[n] = value;  
		} 

//		int [] indexes = new int[tables.Count];
//		for (int i = 0; i < indexes.Count; i++) {
//			int temp = indexes[i];
//			int randomIndex = Random.Range(i, indexes.Length);
//			alpha[i] = alpha[randomIndex];
//			alpha[randomIndex] = temp;
//		}
//
		foreach ( Table table in tables ) {
			if ( table.HasFreeChairs () ) {
				return table;
			}
		}

		return null;
	}

	public void AddTable ( Table table ) {
		tables.Add (table);
	}

	public List<Table> Tables {
		get {
			return tables;
		}
		set {
			tables = value;
		}
	}
}
