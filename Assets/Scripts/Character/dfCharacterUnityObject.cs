using UnityEngine;
using System.Collections;

public class dfCharacterUnityObject : dfUnityObject
{
    protected dfAniController _aniController = null;
    protected CharacterController _characterController;   
    protected float _currentHP;

    public bool _isDead { get; set; }

    public virtual void CreatedByCharacterManager(dfObject dfObj)
    {
        _dfObject = dfObj;
        _currentHP = ((dfCharacter)_dfObject).GetMaxHp();
    }

	// Use this for initialization
	protected override void Start () 
    {
        _aniController = gameObject.GetComponent<dfAniController>();
        _characterController = GetComponent<CharacterController>();

        dfInputManager.Instance._delegateTouchUp += OnTouchUp;

        if (_name == "")
            _name = gameObject.name;       
        
        if(_dfObject == null)
        {
            CreatedByCharacterManager(dfCharacterManager.Instance.GetCharacterProperty(_name, this));
        }

        _isDead = false;
    }

    /*
    public override void SetActive(bool active)
    {
        this.enabled = active;        
    }
    */



    // Update is called once per frame
    protected override void Update () 
    {
	    if(_dfObject != null)
        {
            if(GetAnimationName() == "walk")
            {
                if (_characterController)
                //    _characterController.Move(transform.forward * Time.deltaTime * ((dfCharacter)_dfObject)._speed);
                    _characterController.SimpleMove(transform.forward * Time.deltaTime * _speed);
            }

            if (dfInputManager.Instance.GetButton("fire"))
            {
                Debug.Log("fire");

                SetAnimationName("attack");
            }
        }
	}

    public virtual void OnTouchUp()
    {
        //StopCharacter();
    }

    public virtual void MoveCharacter(Vector3 vec)
    {
        if (GetAnimationName() != "walk")
        {
            _speed = 150.0f;
            SetAnimationName("walk");
            
        }

        SetDirection(vec);

        //Debug.Log("move");
    }
       

    public virtual void StopCharacter()
    {
        _speed = 0.0f;
        SetAnimationName("idle");         
    }

    public virtual void AttackEffect()
    {
    }

    public virtual IEnumerator DeathEffect()
    {
        yield return null;
    }


    public virtual float CalcHitDemage(float de) { return _currentHP; }



    public virtual void OnAnimationEventMessage(string evt)
    {

    }

    public virtual void OnAnimationStart(AnimatorStateInfo stateInfo, string aniName)
    {

    }


    public virtual void OnAnimationUpdateFrame(AnimatorStateInfo stateInfo, string aniName)
    {

    }

    public virtual void OnAnimationEnd(string aniName)
    {
    }

    public virtual void SetAnimationName(string ani)
    {
        if (_aniController != null)
            _aniController.ChangeAnimation(ani);
    }

    public virtual string GetAnimationName()
    {
        if (_aniController == null)
            return "";

        return _aniController.GetCurrentAnimationName();
    }

    public virtual void SetDirection(Vector3 dir)
    {
        gameObject.transform.forward = dir;
    }

    public virtual Vector3 GetDirection()
    {
        return gameObject.transform.forward;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
    }

    public virtual void OnTriggerExit(Collider other)
    {
    }
}
