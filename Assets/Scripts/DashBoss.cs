using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyBehaviorTree;


public class DashBoss : MonoBehaviour {

    float timer = 0f;
    float dashTime = 0.1f;
    float backTime = 0.5f;
    float timeOffset = 3f;
    bool attacking = false;
    bool attacked = false;
    bool parrying = false;
    bool recovering = false;
    Rigidbody2D rb;
    Vector3 origin;
    Animator anim;
    PlayerControl player;
    public BehaviorTree bt;

    void Awake() {
        origin = transform.position;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        player = PlayerControl.Instance;
      
        BuildTree();
    }

    void BuildTree() {
        var rootRepeat = new NaiveRepeater("rootRepeat", bt);
        var rootSel = new SelectorNode("rootSel", bt);
        var parry = new ActionNode("parry", Parry, bt);
        var counter = new ActionNode("counter", Counter, bt);
        var defendSeq = new SequenceNode("defendSeq", bt);
        var attackAndRecover = new ActionNode("attackAndRecover", AttackAndRecover, bt);
        var attacked = new ActionNode("attacked", Attacked, bt);
        var dashSel = new SelectorNode("dashSel", bt);
        var back = new ActionNode("back", Back, bt);
        var attackSeq = new SequenceNode("attackSeq", bt);

        rootRepeat.Build(
            rootSel.Build(
                defendSeq.Build(
                    parry,
                    counter
                    ),
                attackSeq.Build(
                    dashSel.Build(
                        attackAndRecover,
                        attacked
                    ),
                    back
                )
            )
        );

        bt.Build(rootRepeat);
    }

    //void OnEnable() {
    //    PlayerControl.DashAttack += BeAttacked;  
    //}

    // Update is called once per frame
    void Update() {
        //bt.Tick();
        if (Input.GetKeyDown(KeyCode.T)) {
        //    bt.Tick();
        }
    }

    //called from player side
    public void BeAttacked() {
        if (parrying) {
            player.BeCountered();
            anim.SetTrigger("Counter");
            //Parry succeeds
            StopCoroutine("ParryCR");
            parrying = false;
            bt.Finish(NodeStatus.Success);
        }

        if (recovering) {
            anim.SetTrigger("Attacked");
            // hit recover failed, stop attack&recover, go to attacked
            recovering = false;
            StopCoroutine("AttackAndRecoverCR");
            bt.Finish(NodeStatus.Failure);
            Invoke("Back", 0.3f);
        }

    }

    void Parry() {
        StartCoroutine("ParryCR");
    }

    IEnumerator ParryCR() {
        float duration = Random.Range(1f, 3f);
        parrying = true;
        anim.SetTrigger("Parry");
        yield return new WaitForSecondsRealtime(duration);
        bt.Finish(NodeStatus.Failure);
        parrying = false;
    }

    void Counter() {
        StartCoroutine("CounterCR");
    }

    IEnumerator CounterCR() {
        anim.SetTrigger("Counter");
        yield return new WaitForSecondsRealtime(0.3f);
        bt.Finish(NodeStatus.Success);
    }

    void AttackAndRecover() {
        StartCoroutine("AttackAndRecoverCR");
    }

    IEnumerator AttackAndRecoverCR() {
        // prep move
        attacking = true;
        anim.SetTrigger("Attack");
        // wait for animation of attack move
        yield return new WaitForSecondsRealtime(0.2f);
        player.BeAttacked();
        recovering = true;
        // wait for player to attack, CR stopped if attacked by player
        yield return new WaitForSecondsRealtime(0.3f);
        // hit recover finished
        recovering = false;
        bt.Finish(NodeStatus.Success);
    }

    void Attacked() {
        StartCoroutine("AttackedCR");
    }

    IEnumerator AttackedCR() {
        anim.SetTrigger("Attacked");
        yield return new WaitForSecondsRealtime(0.2f);
        bt.Finish(NodeStatus.Success);
    }

    void Back() {
        StartCoroutine("BackCR");
    }

    IEnumerator BackCR() {
        anim.SetTrigger("Back");
        yield return new WaitForSecondsRealtime(0.3f);
        bt.Finish(NodeStatus.Success);
    }

}
