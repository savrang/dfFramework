using UnityEngine;
using System.Collections;

public class dfBulletCollision : MonoBehaviour
{
    public GameObject _explosionGround = null;
    public GameObject _explosionGroundFire = null;


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter(Collision other)
    {
        Debug.Log("OnCollisionEnter");
        //if (other.gameObject.tag == "terrain")
        {
            if (_explosionGround != null)
                Instantiate(_explosionGround, transform.position, Quaternion.identity);

            if (_explosionGroundFire != null)
                Instantiate(_explosionGroundFire, transform.position, Quaternion.identity);


            Collider[] collider = GetComponents<Collider>();
            foreach(Collider col in collider)
            {
                if(col.enabled == false)
                {
                    col.isTrigger = true;
                    col.enabled = true;
                }
            }



            GameObject.Destroy(this.gameObject);
            //Debug.Log("Terrain Collision");
        }
    }
}
