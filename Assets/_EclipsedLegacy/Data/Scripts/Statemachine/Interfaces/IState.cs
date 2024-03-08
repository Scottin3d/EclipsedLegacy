
namespace in3d.Utilities.StateMachine.interfaces
{
    public interface IState
    {
        void OnEnter();
        void OnExit();
        void Update();
        void FixedUpdate();
    }


}
