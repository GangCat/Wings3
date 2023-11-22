using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheKiwiCoder
{
    public class BehaviourTreeRunner : MonoBehaviour
    {
        // Start is called before the first frame update
        public void Init(
            Transform _playerTr, 
            BossCollider _bossCollider, 
            Transform _giantHomingMissileSpawnTr,
            GroupHomingMissileSpawnPos[] _arrGroupHomingMissileSpawnPos,
            BossController _bossCtrl)
        {
            context = CreateBehaviourTreeContext(_playerTr, _bossCollider, _giantHomingMissileSpawnTr, _arrGroupHomingMissileSpawnPos, _bossCtrl);
            tree = tree.Clone();
            tree.Bind(context);
            tree.blackboard.isPhaseEnd = false;
            tree.blackboard.curPhaseNum = 1;
            tree.blackboard.isShieldDestroy = false;
        }

        public void IsShieldDestroy(bool _value)
        {
            tree.blackboard.isShieldDestroy = _value;
        }

        public void FinishCurrentPhase()
        {
            tree.blackboard.isPhaseEnd = true;
        }

        public void StartNextPhase(int _newPhaseNum)
        {
            tree.blackboard.curPhaseNum = _newPhaseNum;
            tree.blackboard.isPhaseEnd = false;
        }

        // Update is called once per frame
        public void RunnerUpdate()
        {
            if (tree)
                tree.Update();
        }

        Context CreateBehaviourTreeContext(
            Transform _playerTr, 
            BossCollider _bossCollider, 
            Transform _giantHomingMissileSpawnTr,
            GroupHomingMissileSpawnPos[] _arrGroupHomingMissileSpawnPos,
            BossController _bossCtrl)
        {
            return Context.CreateFromGameObject(_playerTr, _bossCollider, _giantHomingMissileSpawnTr, _arrGroupHomingMissileSpawnPos, _bossCtrl);
        }

        private void OnDrawGizmosSelected()
        {
            if (!tree) return;

            BehaviourTree.Traverse(tree.rootNode, (n) =>
            {
                if (n.drawGizmos)
                {
                    n.OnDrawGizmos();
                }
            });
        }


        // The main behaviour tree asset
        public BehaviourTree tree;

        // Storage container object to hold game object subsystems
        Context context;
    }
}