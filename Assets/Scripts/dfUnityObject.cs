using UnityEngine;
using System.Collections;

public class dfUnityObject : MonoBehaviour 
{
    protected dfObject _dfObject = null;
    public string _name = "";
    public float _speed { get; set; }

    
    // Use this for initialization
    protected virtual void Start () 
    {
	}
	
	// Update is called once per frame
	protected virtual void Update () 
    {	   
	}

    protected virtual void OnEnable()
    {
    }

    public virtual void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
