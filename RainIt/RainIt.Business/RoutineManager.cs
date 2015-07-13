using System;
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

        public StatusMessage AddUserRoutine(RoutineUploadModel routineUploadModel)
        {
            if (!IsPatternCountValid(routineUploadModel.RoutinePatternDTOList))
                return StatusMessage.WriteError("The number of patterns exceeds the maximum for a routine");
            var routineToAdd = GetRoutineToAdd(routineUploadModel);
            if (!TryUpdateRoutinePatterns(routineToAdd, routineUploadModel.RoutinePatternDTOList))
                return StatusMessage.WriteError("The selected pattern does not exist for the current user");
            if (!TryUpdateDevices(routineToAdd, routineUploadModel.DeviceIdentifierList))
                return StatusMessage.WriteError("The selected devices do not exist for the current user");
            RainItContext.RoutineSet.Add(routineToAdd);
            RainItContext.SaveChanges();
            return StatusMessage.WriteMessage("The routine was created successfully");
        }

        private Routine GetRoutineToAdd(RoutineUploadModel routineUploadModel)
        {
            var routineToAdd = new Routine()
            {
                Description = routineUploadModel.Description,
                Name = routineUploadModel.Name,
                UserId = RainItContext.UserSet.Single(u => u.Username == RainItContext.CurrentUser.Username).UserId,
                RoutinePatterns =  new List<RoutinePattern>(),
                Devices = new List<Device>()
            };
            return routineToAdd;
        }

        public StatusMessage SetToTest(RoutineUploadModel routineUploadModel)
        {
            foreach (var deviceIdentifier in routineUploadModel.DeviceIdentifierList)
            {
                SetToTest(routineUploadModel, deviceIdentifier);
            }
            return StatusMessage.WriteMessage("The selected routines are being tested on the selected devices");
        }

        private StatusMessage SetToTest(RoutineUploadModel routineUploadModel, Guid deviceIdentifier)
        {
            if (!IsPatternCountValid(routineUploadModel.RoutinePatternDTOList))
                return StatusMessage.WriteError("The number of patterns exceeds the maximum for a routine");
            SampleRoutine sampleRoutineOut;
            if(!TryGetSampleRoutineFor(deviceIdentifier, out sampleRoutineOut)) 
                return StatusMessage.WriteError("The selected device does not exist");
            if (!TryDeleteRoutinePatterns(sampleRoutineOut))
                return StatusMessage.WriteError("The patterns for the selected routine could not be deleted");
            if (!TryUpdateRoutinePatterns(sampleRoutineOut, routineUploadModel.RoutinePatternDTOList))
                return StatusMessage.WriteError("The selected pattern does not exist for the current user");
            RainItContext.SaveChanges();
            return StatusMessage.WriteMessage("The routine was updated successfully");
        }

        private bool TryGetSampleRoutineFor(Guid deviceIdentifier, out SampleRoutine sampleRoutineOut)
        {
            sampleRoutineOut = null;
            Device deviceOut;
            if (TryGetDeviceForUser(deviceIdentifier, out deviceOut))
            {
                sampleRoutineOut = RainItContext.SampleRoutineSet.SingleOrDefault(r => r.Device.DeviceInfo.Identifier == deviceIdentifier);
                if (sampleRoutineOut == null)
                {
                    sampleRoutineOut = new SampleRoutine()
                    {
                        DeviceId = deviceOut.DeviceId,
                        UpdateDateTime = DateTime.UtcNow
                    };
                    RainItContext.SampleRoutineSet.Add(sampleRoutineOut);
                    RainItContext.SaveChanges();
                }
            }
            return sampleRoutineOut != null;
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
        public StatusMessage UpdateUserRoutine(RoutineUploadModel routineUploadModel)
        {
            if (!IsPatternCountValid(routineUploadModel.RoutinePatternDTOList))
                return StatusMessage.WriteError("The number of patterns exceeds the maximum for a routine");
            var routineToUpdate = GetRoutineToUpdate(routineUploadModel);
            if (!TryDeleteRoutinePatterns(routineToUpdate))
                return StatusMessage.WriteError("The patterns for the selected routine could not be deleted");
            if (!TryUpdateRoutinePatterns(routineToUpdate, routineUploadModel.RoutinePatternDTOList))
                return StatusMessage.WriteError("The selected pattern does not exist for the current user");
            if (!TryUpdateDevices(routineToUpdate, routineUploadModel.DeviceIdentifierList))
                return StatusMessage.WriteError("The selected devices do not exist for the current user");
            RainItContext.SaveChanges();
            return StatusMessage.WriteMessage("The routine was updated successfully");
        }

        private Routine GetRoutineToUpdate(RoutineUploadModel routineUploadModel)
        {
            var routine =
                    RainItContext.UserRoutineSet.SingleOrDefault(r => r.RoutineId == routineUploadModel.RoutineId);
            if (routine == null) return null;

            routine.Description = routineUploadModel.Description;
            routine.Name = routineUploadModel.Name;
            routine.RoutinePatterns = new List<RoutinePattern>();
            return routine;
        }
        private bool TryDeleteRoutinePatterns(SampleRoutine sampleRoutineToUpdate)
        {
            try
            {
                var allRoutinePatterns = sampleRoutineToUpdate.RoutinePatterns.ToList();
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

        public bool IsPatternCountValid(List<RoutinePatternDTO> routinePatternDTOList)
        {
            return routinePatternDTOList.Count <= int.Parse(ConfigurationManager.AppSettings["MaxPatternCountPerRoutine"]);
        }

        public bool DoesPatternExistsForUser(int patternId)
        {
            return RainItContext.PatternSet.Any(p => p.PatternId == patternId);
        }
        private bool TryUpdateRoutinePatterns(SampleRoutine sampleRoutine, List<RoutinePatternDTO> routinePatternDTOList)
        {
            foreach (var routinePattern in routinePatternDTOList)
            {
                if (!DoesPatternExistsForUser(routinePattern.PatternDTO.PatternId))
                    return false;
                var newRoutinePattern = new RoutinePattern()
                {
                    PatternId = routinePattern.PatternDTO.PatternId,
                    UserId = RainItContext.UserSet.Single(u => u.Username == RainItContext.CurrentUser.Username).UserId,
                    Repetitions = (routinePattern.Repetitions == 0 ? 1 : routinePattern.Repetitions),
                    SampleRoutineId = sampleRoutine.SampleRoutineId
                };
                sampleRoutine.RoutinePatterns.Add(newRoutinePattern);
            }
            return true;
        }
        private bool TryUpdateRoutinePatterns(Routine routine, List<RoutinePatternDTO> routinePatternDTOList)
        {
            foreach (var routinePattern in routinePatternDTOList)
            {
                if (!DoesPatternExistsForUser(routinePattern.PatternDTO.PatternId))
                    return false;
                var newRoutinePattern = new RoutinePattern()
                {
                    PatternId = routinePattern.PatternDTO.PatternId,
                    RoutineId = routine.RoutineId,
                    UserId = RainItContext.UserSet.Single(u => u.Username == RainItContext.CurrentUser.Username).UserId,
                    Repetitions = (routinePattern.Repetitions == 0 ? 1 : routinePattern.Repetitions)
                };
                routine.RoutinePatterns.Add(newRoutinePattern);
            }
            return true;
        }
        private bool TryUpdateDevices(Routine routine, List<Guid> deviceidentifierList)
        {
            if (deviceidentifierList == null) return true;
            routine.Devices = new List<Device>();
            foreach (var deviceIdentifier in deviceidentifierList)
            {
                Device deviceOut;
                if (!TryGetDeviceForUser(deviceIdentifier, out deviceOut))
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
                    },
                    Repetitions = rp.Repetitions
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
