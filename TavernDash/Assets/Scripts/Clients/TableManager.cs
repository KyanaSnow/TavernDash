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

			// shuffle de liste que j'ai trouvé sur internet.
			// je fais pareil avec les chaises.
			// trop gourmand ?
		int n = tables.Count;  
		while (n > 1) {  
			n--;  
			int k = Random.Range (0, n + 1);
			Table value = tables[k];  
			tables[k] = tables[n];  
			tables[n] = value;  
		} 
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
