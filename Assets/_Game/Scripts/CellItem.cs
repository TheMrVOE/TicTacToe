using UnityEngine;

namespace TicTacToe
{
    public enum CellType { None = 0, Cross = 1, Circle = 2}

    public class CellItem : MonoBehaviour
    {
        [SerializeField]
        private Material _matCross;
        [SerializeField]
        private Material _matCircle;

        [HideInInspector]
        public CellType cellType = CellType.None;
        public int x { get; set; }
        public int y { get; set; }

        private Animator _animator;
        private Animator animator
        {
            get
            {
                if(_animator == null)
                {
                    _animator = GetComponent<Animator>();
                }
                return _animator;
            }
        }
        
        public void GetOwnIndex(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        public void ChangeState(bool isCross, Material myNewMat)
        {
            if(isCross)
            {
                cellType = CellType.Cross;
                animator.SetTrigger("ChangeToCross");
            }
            else
            {
                animator.SetTrigger("ChangeToCircle");
                cellType = CellType.Circle;
            }
            GetComponent<Collider>().enabled = false;
        }
        public void HandlerAnimation()
        {
            MeshRenderer renderer = GetComponent<MeshRenderer>();
            if(cellType == CellType.Cross)
            {
                renderer.material = _matCross;
            }
            else
            {
                renderer.material = _matCircle;
            }
        }
    }
}