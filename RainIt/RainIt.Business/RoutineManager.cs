﻿using System;
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

        public StatusMessage AddUserRoutine(RoutineDTO routineDTO)
        {
            if (!IsPatternCountValid(routineDTO.RoutinePatternDTOs))
                return StatusMessage.WriteError("The number of patterns exceeds the maximum for a routine");
            var routineToAdd = GetRoutineToAdd(routineDTO);
            if (!TryUpdateRoutinePatterns(routineToAdd, routineDTO.RoutinePatternDTOs))
                return StatusMessage.WriteError("The selected pattern does not exist for the current user");
            RainItContext.RoutineSet.Add(routineToAdd);
            RainItContext.SaveChanges();
            return StatusMessage.WriteMessage("The routine was created successfully");
        }

        private Routine GetRoutineToAdd(RoutineDTO routineDTO)
        {
            var routineToAdd = new Routine()
            {
                Description = routineDTO.Description,
                Name = routineDTO.Name,
                UserId = RainItContext.UserSet.Single(u => u.Username == RainItContext.CurrentUser.Username).UserId,
                RoutinePatterns =  new List<RoutinePattern>()
            };
            return routineToAdd;
        }

        #endregion

        #region READ Methods

        public List<RoutineDTO> GetUserRoutines()
        {
            var allUserRoutines = RainItContext.UserRoutineSet;
            return ToRoutineDTOList(allUserRoutines);
        }

        public RoutineDTO GetUserRoutine(int routineId)
        {
            var userRoutines = RainItContext.UserRoutineSet.Where(r => r.RoutineId == routineId);
            return ToRoutineDTOList(userRoutines).Single();
        }

        public RoutineDTO GetActiveUserRoutine()
        {
            var activeRoutine = RainItContext.RoutineSet.Where(r => r.IsActive);
            return ToRoutineDTOList(activeRoutine).SingleOrDefault();
        }
       
        #endregion

        #region UPDATE Methods
        public StatusMessage UpdateUserRoutine(RoutineDTO routineDTO)
        {
            if (!IsPatternCountValid(routineDTO.RoutinePatternDTOs))
                return StatusMessage.WriteError("The number of patterns exceeds the maximum for a routine");
            var routineToUpdate = GetRoutineToUpdate(routineDTO);
            if (!TryDeleteRoutinePatterns(routineToUpdate))
                return StatusMessage.WriteError("The patterns for the selected routine could not be deleted");
            if (!TryUpdateRoutinePatterns(routineToUpdate, routineDTO.RoutinePatternDTOs))
                return StatusMessage.WriteError("The selected pattern does not exist for the current user");
            RainItContext.SaveChanges();
            return StatusMessage.WriteMessage("The routine was updated successfully");
        }

        private Routine GetRoutineToUpdate(RoutineDTO routineDTO)
        {
            var routine = RainItContext.UserRoutineSet.SingleOrDefault(r => r.RoutineId == routineDTO.RoutineId);
            if (routine == null) return null;

            routine.Description = routineDTO.Description;
            routine.Name = routineDTO.Name;
            routine.RoutinePatterns = new List<RoutinePattern>();
            routine.IsActive = routineDTO.IsActive;
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

        public StatusMessage SetActive(int routineId)
        {
            var allOtherActiveRoutines = RainItContext.UserRoutineSet.Where(r => r.RoutineId != routineId).ToList();
            if (!SetInactive(allOtherActiveRoutines))
                return StatusMessage.WriteError("Other user routines could not be set to inactive");
            var selectedRoutine = RainItContext.UserRoutineSet.SingleOrDefault(r => r.RoutineId == routineId);
            if(selectedRoutine == null)
                return StatusMessage.WriteError("The selected user routine does not exist");
            selectedRoutine.IsActive = true;
            RainItContext.SaveChanges();
            return StatusMessage.WriteError("Successfully set user routine as active");
        }

        private bool SetInactive(List<Routine> routineList)
        {
            routineList.ForEach(r =>
            {
                r.IsActive = false;
            });
            RainItContext.SaveChanges();
            return true;
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

        public bool IsPatternCountValid(List<RoutinePatternDTO> patternList)
        {
            return patternList.Count <= int.Parse(ConfigurationManager.AppSettings["MaxPatternCountPerRoutine"]);
        }

        public bool DoesPatternExistsForUser(int patternId)
        {
            return RainItContext.PatternSet.Any(p => p.PatternId == patternId);
        }

        private bool TryUpdateRoutinePatterns(Routine routine, List<RoutinePatternDTO> routinePatternDTOList)
        {
            foreach (var routinePatternDTO in routinePatternDTOList)
            {
                if (!DoesPatternExistsForUser(routinePatternDTO.PatternDTO.PatternId))
                    return false;
                var newRoutinePattern = new RoutinePattern()
                {
                    PatternId = routinePatternDTO.PatternDTO.PatternId,
                    RoutineId = routine.RoutineId,
                    UserId = RainItContext.UserSet.Single(u => u.Username == RainItContext.CurrentUser.Username).UserId,
                };
                routine.RoutinePatterns.Add(newRoutinePattern);
            }
            return true;
        }
         private List<RoutineDTO> ToRoutineDTOList(IQueryable<Routine> routineQueryable)
        {
            return routineQueryable.Select(r => new RoutineDTO()
            {
                RoutineId = r.RoutineId,
                IsActive = r.IsActive,
                Name = r.Name,
                RoutinePatternDTOs = r.RoutinePatterns.Select(rp => new RoutinePatternDTO()
                {
                    PatternDTO = new PatternDTO()
                    {
                        PatternId = rp.PatternId ?? 0,
                        Name = rp.Pattern.Name,
                        Path = rp.Pattern.Path
                    }
                }).ToList()
            }).ToList();
        }
        #endregion
    }
}
