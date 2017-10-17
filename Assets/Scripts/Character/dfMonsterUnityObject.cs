using UnityEngine;
using System.Collections;

public class dfMonsterUnityObject : dfCharacterUnityObject
{
    protected bool _aiEnable = false;
    protected bool _isAttack = false;
    protected float _attackInterval;
    protected float _deadInterval;


    public override void CreatedByCharacterManager(dfObject dfObj)
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

        _attackInterval = 0.0f;
    }

    // Update is called once per frame
    protected override void Update ()
    {
        float distance = CheckPlayerDistance();

        if(_aiEnable)
        {
            _attackInterval += Time.deltaTime;

            //if(CheckPlayerVisible())
            {
                string aniName = GetAnimationName();

                if (aniName != "attack" && aniName != "hit" && aniName != "death")
                {
                    //transform.LookAt(dfAIManager.Instance._plyer.transform);
                    LookAtPlayer();

                    if (_isAttack)
                    {
                        if (_attackInterval >= ((dfMonster)_dfObject).GetAttackDelay())
                        {
                            _aniController.ChangeAnimation("attack");
                            _attackInterval = 0.0f;
                        }
                    }
                    else
                    {
                        MoveCharacter(Vector3.zero);
                    }
                }
            }
        }
    }

    protected void LookAtPlayer()
    {
        Vector3 posPlayer = dfAIManager.Instance._plyer.transform.position;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(posPlayer - transform.position), 20.0f * Time.deltaTime);
        transform.eulerAngles = new Vector3(0.0f, transform.eulerAngles.y, 0.0f);

    }

    public override void MoveCharacter(Vector3 vec)
    {
        if (GetAnimationName() != "walk")
            SetAnimationName("walk");

        Debug.DrawLine(transform.position, transform.position + transform.forward, Color.red);

        _speed = 0.7f;

        //_characterController.SimpleMove(transform.forward * Time.deltaTime * _speed);
        transform.position += transform.forward * + _speed * Time.deltaTime;
    }


    protected float CalculatePlayerDirection()
    {        
        Vector3 direction = (dfAIManager.Instance._plyer.transform.position - transform.position).normalized;
        float angle = Vector3.Dot(direction, transform.forward);

        return angle;
    }

    protected bool CheckPlayerVisible()
    {
        RaycastHit ray = new RaycastHit();
        LayerMask layer = dfAIManager.Instance.GetPlayerLayerMask();

        float angle = CalculatePlayerDirection();

        if(Physics.Linecast(transform.position, dfAIManager.Instance._plyer.transform.position, out ray, layer))
        {
            return false;
        }
        else
        {
            if (angle > 0)
                return true;
        }

        return false;
    }

    protected float CheckPlayerDistance()
    {
        if (dfAIManager.Instance._plyer == null)
            return 100000.0f;

        float distance = Vector3.Distance(transform.position, dfAIManager.Instance._plyer.transform.position);
        if(distance <= ((dfMonster)_dfObject).GetAiDistance())
        {
            _aiEnable = true;
        }
        else
        {
            _aiEnable = false;
        }

        return distance;
    }


    public override IEnumerator DeathEffect()
    {
        yield return new WaitForSeconds(2.0f);

        GameObject.Destroy(this.gameObject);
    }


    public override float CalcHitDemage(float de)
    {
        _currentHP -= de;
        if (_currentHP < 0.0f)
            _currentHP = 0.0f;

        return _currentHP;
    }


    public override void OnAnimationEventMessage(string evt)
    {
        if(evt == "death")
        {
            dfAIManager.Instance.DestroyAiObject();

            _deadInterval = 0.0f;

            Collider[] collider = GetComponents<Collider>();
            foreach (Collider col in collider)
            {
                col.enabled = false;
            }

            StartCoroutine(DeathEffect());
            return;
        }

        StopCharacter();
    }

    public override void OnAnimationEnd(string aniName)
    {
        //StopCharacter();
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isAttack = true;
        } 
        
        if(other.gameObject.tag == "bullet")
        {
            float demage = CalcHitDemage(4.0f);
            if (demage <= 0.0f)
            {
                SetAnimationName("death");                
                _isDead = true;               
            }
            else
                SetAnimationName("hit");
        }
    }

    public override void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _isAttack = false;            
        }
    }
}
