using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RainIt.Domain.DTO;
using RainIt.Domain.Repository;
using RainIt.Interfaces.Business;
using RainIt.Interfaces.Repository;

namespace RainIt.Business
{
    public class RoutineManager : IRoutineManager
    {
        public IRainItContext RainItContext { get; set; }

        public RoutineManager(IRainItContext rainItContext)
        {
            RainItContext = rainItContext;
        }

        #region CREATE Methods

        public StatusMessage AddUserRoutine(RoutineList routineList)
        {
            if (!IsPatternCountValid(routineList.RoutinePatterns))
                return StatusMessage.WriteError("The number of patterns exceeds the maximum for a routine");
            var routineToAdd = GetRoutineToAdd(routineList);
            if (!TryUpdateRoutinePatterns(routineToAdd, routineList.RoutinePatterns))
                return StatusMessage.WriteError("The selected pattern does not exist for the current user");
            RainItContext.RoutineSet.Add(routineToAdd);
            RainItContext.SaveChanges();
            return StatusMessage.WriteMessage("The routine was created successfully");
        }

        private Routine GetRoutineToAdd(RoutineList routineList)
        {
            var routineToAdd = new Routine()
            {
                Description = routineList.Description,
                Name = routineList.Name,
                UserId = RainItContext.UserSet.Single(u => u.Username == RainItContext.CurrentUser.Username).UserId,
                RoutinePatterns =  new List<RoutinePattern>()
            };
            return routineToAdd;
        }

        #endregion

        #region READ Methods

        public List<RoutineList> GetUserRoutines()
        {
            return RainItContext.UserRoutineSet.Select(r => new RoutineList()
            {
                RoutineId = r.RoutineId,
                IsActive = r.IsActive,
                Name = r.Name,
                RoutinePatterns = r.RoutinePatterns.Select(p => new RoutinePatternList()
                {
                    PatternId = p.PatternId
                }).ToList()
            }).ToList();
        }

        public RoutineList GetUserRoutine(int routineId)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region UPDATE Methods
        public StatusMessage UpdateUserRoutine(RoutineList routineList)
        {
            if (!IsPatternCountValid(routineList.RoutinePatterns))
                return StatusMessage.WriteError("The number of patterns exceeds the maximum for a routine");
            var routineToUpdate = GetRoutineToUpdate(routineList);
            if (!TryDeleteRoutinePatterns(routineToUpdate))
                return StatusMessage.WriteError("The patterns for the selected routine could not be deleted");

            if (!TryUpdateRoutinePatterns(routineToUpdate, routineList.RoutinePatterns))
                return StatusMessage.WriteError("The selected pattern does not exist for the current user");
            RainItContext.SaveChanges();
            return StatusMessage.WriteMessage("The routine was updated successfully");
        }

        private Routine GetRoutineToUpdate(RoutineList routineList)
        {
            var routine = RainItContext.UserRoutineSet.SingleOrDefault(r => r.RoutineId == routineList.RoutineId);
            if (routine == null) return null;

            routine.Description = routineList.Description;
            routine.Name = routineList.Name;
            routine.RoutinePatterns = new List<RoutinePattern>();
            routine.IsActive = routineList.IsActive;
            return routine;
        }

        private bool TryDeleteRoutinePatterns(Routine routineToUpdate)
        {
            try
            {
                var allRoutinePatterns = routineToUpdate.RoutinePatterns.ToList();
                foreach (var routinePattern in allRoutinePatterns)
                {
                    RainItContext.RoutinePatternSet.Attach(routinePattern);
                    RainItContext.RoutinePatternSet.Remove(routinePattern);
                }
                RainItContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion

        #region DELETE Methods
        public StatusMessage DeleteUserRoutine(int routineId)
        {
            try
            {
                var routineToDelete = RainItContext.UserRoutineSet.Single(r => r.RoutineId == routineId);
                RainItContext.RoutineSet.Attach(routineToDelete);
                RainItContext.RoutineSet.Remove(routineToDelete);
                RainItContext.SaveChanges();
                return StatusMessage.WriteMessage("The routine was successfully deleted");
            }
            catch (Exception ex)
            {
                return StatusMessage.WriteError("An unexpected error occurred. Please try again.");
            }
        }
        #endregion

        #region Helper Methods

        public bool IsPatternCountValid(List<RoutinePatternList> patternList)
        {
            return patternList.Count <= int.Parse(ConfigurationManager.AppSettings["MaxPatternCountPerRoutine"]);
        }

        public bool DoesPatternExistsForUser(int patternId)
        {
            return RainItContext.PatternSet.Any(p => p.PatternId == patternId);
        }

        private bool TryUpdateRoutinePatterns(Routine routine, List<RoutinePatternList> patternList)
        {
            foreach (var pattern in patternList)
            {
                if (!DoesPatternExistsForUser(pattern.PatternId))
                    return false;
                var routinePattern = new RoutinePattern()
                {
                    PatternId = pattern.PatternId,
                    RoutineId = routine.RoutineId
                };
                routine.RoutinePatterns.Add(routinePattern);
            }
            return true;
        }
        #endregion
    }
}
