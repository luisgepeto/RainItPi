using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
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
            if (DoesUserRoutineNameExist(routineUploadModel.Name))
                return StatusMessage.WriteError("The specified routine name already exists");
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
                if (sampleRoutineOut != null)
                    sampleRoutineOut.UpdateUTCDateTime = DateTime.UtcNow;
                if (sampleRoutineOut == null)
                {
                    sampleRoutineOut = new SampleRoutine()
                    {
                        DeviceId = deviceOut.DeviceId,
                        Device = deviceOut,
                        UpdateUTCDateTime = DateTime.UtcNow,
                        RoutinePatterns = new List<RoutinePattern>()
                    };
                    RainItContext.SampleRoutineSet.Add(sampleRoutineOut);
                    RainItContext.SaveChanges();
                }
            }
            return sampleRoutineOut != null;
        }

        #endregion

        #region READ Methods

        public UploadConstraintParameters GetUploadConstraintParameters()
        {
            return new UploadConstraintParameters()
            {
                MaxPatternCountPerRoutine = int.Parse(ConfigurationManager.AppSettings["MaxPatternCountPerRoutine"]),
                MaxNumberOfRepetitionsPerPattern =
                    int.Parse(ConfigurationManager.AppSettings["MaxNumberOfRepetitionsPerPattern"]),
                MaxNameLength = 50
            };
        }
        public List<RoutineDTO> GetUserRoutines()
        {
            var allUserRoutines = RainItContext.UserRoutineSet.Where(r => r.RoutinePatterns.Any());
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
                            Path = rp.Pattern.Path,
                            ConversionParameterDTO = new ConversionParameterDTO()
                            {
                                BWeight = rp.Pattern.ConversionParameter.BWeight,
                                GWeight = rp.Pattern.ConversionParameter.GWeight,
                                RWeight = rp.Pattern.ConversionParameter.RWeight,
                                IsInverted = rp.Pattern.ConversionParameter.IsInverted,
                                ThresholdPercentage = rp.Pattern.ConversionParameter.ThresholdPercentage
                            }
                        },
                        Repetitions = rp.Repetitions
                    }).ToList()
                }).ToList();
        }

        public RoutineDTO GetTestRoutine()
        {
            var expireTimeInMinutes = int.Parse(ConfigurationManager.AppSettings["SampleExpireTimeInMinutes"]);
            var maxExpireDate = DateTime.UtcNow.AddMinutes(-expireTimeInMinutes);
            return RainItContext.DeviceSampleRoutineSet
                .Where(sr => sr.UpdateUTCDateTime >= maxExpireDate)
                .OrderByDescending(sr => sr.UpdateUTCDateTime)
                .Select(r => new RoutineDTO()
                {
                    RoutineId = r.SampleRoutineId,
                    RoutinePatternDTOs = r.RoutinePatterns.Select(rp => new RoutinePatternDTO()
                    {
                        RoutinePatternId = rp.RoutinePatternId,
                        PatternDTO = new PatternDTO()
                        {
                            PatternId = rp.PatternId ?? 0,
                            Path = rp.Pattern.Path,
                            ConversionParameterDTO = new ConversionParameterDTO()
                            {
                                BWeight = rp.Pattern.ConversionParameter.BWeight,
                                GWeight = rp.Pattern.ConversionParameter.GWeight,
                                RWeight = rp.Pattern.ConversionParameter.RWeight,
                                IsInverted = rp.Pattern.ConversionParameter.IsInverted,
                                ThresholdPercentage = rp.Pattern.ConversionParameter.ThresholdPercentage
                            }
                        },
                        Repetitions = rp.Repetitions
                    }).ToList(),
                    SampleTimeStamp = r.UpdateUTCDateTime
                }).FirstOrDefault();
        }

        #endregion

        #region UPDATE Methods
        public StatusMessage UpdateUserRoutine(RoutineUploadModel routineUploadModel)
        {
            if (!IsPatternCountValid(routineUploadModel.RoutinePatternDTOList))
                return StatusMessage.WriteError("The number of patterns exceeds the maximum for a routine");
            var routineToUpdate = GetRoutineToUpdate(routineUploadModel);
            if (routineToUpdate.Name != routineUploadModel.Name)
            {
                if (DoesUserRoutineNameExist(routineUploadModel.Name))
                    return StatusMessage.WriteError("The specified routine name already exists");
                routineToUpdate.Name = routineUploadModel.Name;
            }
            if (!TryDeleteRoutinePatterns(routineToUpdate))
                return StatusMessage.WriteError("The patterns for the selected routine could not be deleted");
            if (!TryUpdateRoutinePatterns(routineToUpdate, routineUploadModel.RoutinePatternDTOList))
                return StatusMessage.WriteError("The selected pattern does not exist for the current user");
            if (!TryDeleteDevices(routineToUpdate))
                return StatusMessage.WriteError("The devices for the selected routine could not be deleted");
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
            routine.RoutinePatterns = new List<RoutinePattern>();
            return routine;
        }
        private bool TryDeleteRoutinePatterns(SampleRoutine sampleRoutineToUpdate)
        {
            var allRoutinePatterns = sampleRoutineToUpdate.RoutinePatterns.ToList();
            if (allRoutinePatterns.Any())
            {
                foreach (var routinePattern in allRoutinePatterns)
                {
                    RainItContext.RoutinePatternSet.Attach(routinePattern);
                    RainItContext.RoutinePatternSet.Remove(routinePattern);
                }
                RainItContext.SaveChanges();    
            }
            return true;
        }
        private bool TryDeleteRoutinePatterns(Routine routineToUpdate)
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

        #endregion

        #region DELETE Methods
        public StatusMessage DeleteUserRoutine(int routineId)
        {
            var routineToDelete = RainItContext.UserRoutineSet.Single(r => r.RoutineId == routineId);
            DeleteRoutinePatterns(routineToDelete);
            if(!TryDeleteDevices(routineToDelete))
                return StatusMessage.WriteError("The devices for the selected routine could not be deleted");
            RainItContext.RoutineSet.Attach(routineToDelete);
            RainItContext.RoutineSet.Remove(routineToDelete);
            RainItContext.SaveChanges();
            return StatusMessage.WriteMessage("The routine was successfully deleted");
        }

        public void DeleteRoutinePatterns(Routine routine)
        {
            var allRoutinePatterns = routine.RoutinePatterns.ToList();
            if (allRoutinePatterns.Any())
            {
                foreach (var routinePattern in allRoutinePatterns)
                {
                    RainItContext.RoutinePatternSet.Attach(routinePattern);
                    RainItContext.RoutinePatternSet.Remove(routinePattern);
                }
                RainItContext.SaveChanges();
            }
        }
        #endregion

        #region Helper Methods
        private bool DoesUserRoutineNameExist(string name)
        {
            return RainItContext.UserRoutineSet.Any(r => r.Name == name);
        }

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
        private bool TryDeleteDevices(Routine routineToUpdate)
        {
            var allDevices = routineToUpdate.Devices.ToList();
            if (allDevices.Any())
            {
                foreach (var device in allDevices)
                {
                    RainItContext.Entry(routineToUpdate).Collection("Devices").Load();
                    routineToUpdate.Devices.Remove(device);
                }
                RainItContext.SaveChanges();
            }
            return true;
        }

        private bool TryUpdateDevices(Routine routine, List<Guid> deviceidentifierList)
        {
            routine.Devices = new List<Device>();
            if (deviceidentifierList == null) return true;
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
