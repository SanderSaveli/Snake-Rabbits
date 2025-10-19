using System.Diagnostics;

namespace SanderSaveli.Snake
{
    public class EatCarrotState : IRabbitState
    {
        private Rabbit _rabbit;
        private FSM<IRabbitState> _fsm;

        public void Initialize(Rabbit rabbit, FSM<IRabbitState> fsm)
        {
            _rabbit = rabbit;
            _fsm = fsm;
        }

        public void OnEnter()
        { }

        public void OnExit()
        { }

        public void OnUpdate()
        {
            Cell fowardCell = _rabbit.GetFowardCell();

            if (fowardCell == null
                || !fowardCell.IsOccupied
                || fowardCell.Entity.EntityType != typeof(Carrot))
            {
                UnityEngine.Debug.Log("There is no carrot, go to anoter carrot");
                _fsm.ChangeState<MoveToCarrotState>();
                return;
            }
            else
            {
                Carrot carrot = fowardCell.Entity as Carrot;
                carrot.TakeDamage();
            }
        }
    }
}
