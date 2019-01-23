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
            Simulator.Instance.agents_[agentId].Destination = newDestination;    
        }
              
        public void Tick()
        {
            Parallel.ForEach(Simulator.Instance.agents_.Values, agent =>
            {
                agent.CalculatePrefVelocity();
            });

            Simulator.Instance.doStep();
        }

        public Dictionary<int, Vector2> GetAgentPositions()
        {
            return Simulator.Instance.agents_.ToDictionary(pair => pair.Key, pair => pair.Value.position_);
        }   
    }
}
