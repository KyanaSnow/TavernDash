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
