using UnityEngine;
using System.Collections;

public class dfPeanutCharacterUnityObject : dfCharacterUnityObject
{
    public GameObject _attackEffect = null;
    public float _attackEffectTime = 0.0f;

    public Rigidbody _bullet = null;

    protected ParticleSystem _particleSystem = null;
    public Transform _gunFireTransform;
    public GameObject _GunFireEffect = null;


    // Use this for initialization
    protected override void Start ()
    {
        base.Start();

        dfAIManager.Instance._plyer = gameObject;

        _particleSystem = _bullet.GetComponent<ParticleSystem>();
	}

    // Update is called once per frame
    protected override void Update ()
    {
        //base.Update();
        if (_dfObject != null)
        {
            if (GetAnimationName() == "walk")
            {
                if (_characterController)
                    //    _characterController.Move(transform.forward * Time.deltaTime * _speed);
                    _characterController.SimpleMove(transform.forward * Time.deltaTime * _speed);
            }

            if (dfInputManager.Instance.GetButton("fire"))
            {
                Debug.Log("fire");

                SetAnimationName("attack");
            }
        }

    }


    public override void AttackEffect()
    {
        GameObject obj = Instantiate(_GunFireEffect, _gunFireTransform.position, _gunFireTransform.rotation) as GameObject;
        obj.transform.parent = _gunFireTransform;

        Rigidbody instantiatedProjectile = Instantiate(_bullet, _attackEffect.transform.position, Quaternion.identity) as Rigidbody;
        instantiatedProjectile.velocity = _gunFireTransform.forward * 15.0f;// new Vector3(0, -10.0f, 30.0f));

    }

    public override void OnTouchUp()
    {
        StopCharacter();
    }

    public override void OnAnimationStart(AnimatorStateInfo stateInfo, string aniName)
    {

    }

    public override void OnAnimationUpdateFrame(AnimatorStateInfo stateInfo, string aniName)
    {

    }

    public override void OnAnimationEnd(string aniName)
    {
        StopCharacter();
    }

}
