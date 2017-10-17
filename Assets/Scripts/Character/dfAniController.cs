using UnityEngine;
using System.Collections;

public class dfAniController : dfUnityObject
{
    Animator _animator = null;

    dfCharacterUnityObject _characterUnityObject = null;

    protected string _preAnimationName = "idle";
    protected string _animationName = "idle";

    void Awake()
    {
        _animator = GetComponent<Animator>();

        _characterUnityObject = GetComponent<dfCharacterUnityObject>();
    }

	// Use this for initialization
	protected override void Start () 
    {
        if (_animator != null)
        {
            dfStateMachineBehaviour[] stateMachineBevaviour = _animator.GetBehaviours<dfStateMachineBehaviour>();

            foreach (dfStateMachineBehaviour state in stateMachineBevaviour)
                state._aniController = this;
        }
	}

    // Update is called once per frame
    protected override void Update ()
    {
	
	}

    public void ChangeAnimation(string aniName)
    {
        if (isCurrentAnimation(_animationName, aniName))
        {
            return;
        }

        //Debug.Log("change animation : " + aniName);

        //if (isCurrentAnimation("idle"))
        //    Debug.Log("current animation : " + "idle");

        //if (isCurrentAnimation("attack"))
        //    Debug.Log("current animation : " + "attack");
                
        _animator.ResetTrigger(_animationName);
        //_animator.Play(aniName);
        _preAnimationName = _animationName;
        _animationName = aniName;

        _animator.SetTrigger(aniName);
    }

    public string GetCurrentAnimationName()
    {
        return _animationName;
    }

    public string GetPreAnimationName()
    {
        return _preAnimationName;
    }

    public bool isCurrentAnimation(string curAni, string nextAni)
    {
        if (_animator == null)
            return true;

        int nextAniHash = Animator.StringToHash(nextAni);
        string transName = curAni + " -> " + nextAni;
        int transNameHash = Animator.StringToHash(transName);

        //int curAniHash = Animator.StringToHash(curAni);
        //Debug.Log("Change Info => curAni : " + curAni + "   ,  nextAni : " + nextAni);

        //int hash = _animator.GetCurrentAnimatorStateInfo(0).fullPathHash;
        //Debug.Log("Change Info(" +hash.ToString() +") => curAni : " + curAniHash.ToString() + "   ,  nextAni : " + nextAniHash.ToString());

        //if (_animator.GetCurrentAnimatorStateInfo(0).fullPathHash == nextAniHash)
        if (_animator.GetCurrentAnimatorStateInfo(0).IsName(nextAni))
        {
            //Debug.Log("SameAni => curAni : " + curAni + "   ,  nextAni : " + nextAni);
            return true;
        }

        if (_animator.IsInTransition(0))
        {
            //Debug.Log("Transition.....");

            AnimatorTransitionInfo stateInfo = _animator.GetAnimatorTransitionInfo(0);
            if (stateInfo.nameHash == transNameHash)
            {
                //Debug.Log("Transition => curAni : " + curAni + " ->  nextAni : " + nextAni);
                return true;
            }
        }

        //Debug.Log("ChangeAni => curAni : " + curAni + "   ,  nextAni : " + nextAni);

        return false;
    }


    public void OnStateEnter(AnimatorStateInfo stateInfo, string aniName)
    {
        if (_characterUnityObject != null)
            _characterUnityObject.OnAnimationStart(stateInfo, aniName);
    }

    public void OnStateUpdate(AnimatorStateInfo stateInfo, string aniName)
    {
        if (_characterUnityObject != null)
            _characterUnityObject.OnAnimationUpdateFrame(stateInfo, aniName);
    }

    public void OnStateExit(AnimatorStateInfo stateInfo, string aniName)
    {
        if (_characterUnityObject != null)
            _characterUnityObject.OnAnimationEnd(aniName);
    }
}
