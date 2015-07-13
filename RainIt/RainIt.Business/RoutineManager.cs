﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
            if (!TryUpdateDevices(routineToAdd, routineDTO.DeviceDTOs))
                return StatusMessage.WriteError("The selected devices do not exist for the current user");
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
                RoutinePatterns =  new List<RoutinePattern>(),
                Devices = new List<Device>()
            };
            return routineToAdd;
        }

        public StatusMessage SetToTest(List<int> patternIdList, List<Guid> deviceIdentifierList)
        {
            foreach (var deviceIdentifier in deviceIdentifierList)
            {
                SetToTest(patternIdList, deviceIdentifier);
            }
            return StatusMessage.WriteMessage("Successfully testing routine in the selected devices");
        }
        private StatusMessage SetToTest(List<int> patternIdList, Guid deviceIdentifier)
        {
            Device deviceOut;
            if (!TryGetDeviceForUser(deviceIdentifier, out deviceOut))
                return StatusMessage.WriteError("the selected device does not exist for the current user");

            return StatusMessage.WriteMessage("Successfully testing in the selected device");
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

        public List<RoutineDTO> GetActiveRoutines()
        {
            return RainItContext.DeviceRoutineSet.Select(r => new RoutineDTO()
                {
                    RoutineId = r.RoutineId,
                    RoutinePatternDTOs = r.RoutinePatterns.Select(rp => new RoutinePatternDTO()
                    {
                        RoutinePatternId = rp.RoutinePatternId,
                        PatternDTO = new PatternDTO()
                        {
                            PatternId = rp.PatternId ?? 0,
                            Path = rp.Pattern.Path
                        }
                    }).ToList()
                }).ToList();
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
            if (!TryUpdateDevices(routineToUpdate, routineDTO.DeviceDTOs))
                return StatusMessage.WriteError("The selected devices do not exist for the current user");
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
        private bool TryUpdateDevices(Routine routine, List<DeviceDTO> deviceDTOList)
        {
            routine.Devices = new List<Device>();
            foreach (var device in deviceDTOList)
            {
                Device deviceOut;
                if (!TryGetDeviceForUser(device.Identifier, out deviceOut))
                    return false;
                routine.Devices.Add(deviceOut);
            }
            return true;
        }
        private bool TryGetDeviceForUser(Guid identifier, out Device deviceOut)
        {
            deviceOut = RainItContext.UserDeviceSet.SingleOrDefault(d => d.DeviceInfo.Identifier == identifier);
            return deviceOut != null;
        }
         private List<RoutineDTO> ToRoutineDTOList(IQueryable<Routine> routineQueryable)
        {
            return routineQueryable.Select(r => new RoutineDTO()
            {
                RoutineId = r.RoutineId,
                Name = r.Name,
                RoutinePatternDTOs = r.RoutinePatterns.Select(rp => new RoutinePatternDTO()
                {
                    PatternDTO = new PatternDTO()
                    {
                        PatternId = rp.PatternId ?? 0,
                        Name = rp.Pattern.Name,
                        Path = rp.Pattern.Path
                    }
                }).ToList(),
                DeviceDTOs = r.Devices.Select(d => new DeviceDTO()
                {
                    Identifier = d.DeviceInfo.Identifier
                }).ToList()
            }).ToList();
        }
        #endregion
    }
}
