using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BEPUutilities;
using ECS.RVO;
using FixMath.NET;                          

namespace ECS
{
    public class RVONavigationService : INavigationService  
    {
        private readonly Dictionary<int, Vector2> _goals = new Dictionary<int, Vector2>();

        public RVONavigationService()
        {
            Simulator.Instance.setTimeStep(0.5m);
            Simulator.Instance.setAgentDefaults(15, 10, 5, 5, 1, 2, new Vector2(0, 0));
        }
               
        public void AddAgent(int id, Vector2 position)
        {

            Simulator.Instance.addAgent(id, new Vector2(position.X, position.Y));
        }

        public void SetAgentDestination(int agentId, Vector2 newDestination)
        {
            _goals[agentId] = new Vector2(newDestination.X, newDestination.Y);
        }
              
        public void UpdateAgents()
        {
            Parallel.ForEach(Simulator.Instance.agents_, agent =>
            {
                if (_goals.ContainsKey(agent.id_))
                {     
                    var goalVector = _goals[agent.id_] - agent.position_;

                    if (RVOMath.absSq(goalVector) > Fix64.One)
                    {
                        goalVector = RVOMath.normalize(goalVector);
                    }

                    agent.prefVelocity_ = goalVector;
                }
            });

            Simulator.Instance.doStep();
        }      

        public Vector2 GetAgentPosition(int agentId)
        {                
            return Simulator.Instance.agents_.First(agent => agent.id_ == agentId).position_;
        }    
    }
}
