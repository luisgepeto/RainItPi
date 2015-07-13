using System;
using System.Collections.Generic;
using RainIt.Domain.DTO;

namespace RainIt.Interfaces.Business
{
    public interface IRoutineManager
    {
        StatusMessage AddUserRoutine(RoutineUploadModel routineUploadModel);
        StatusMessage SetToTest(RoutineUploadModel routineUploadModel);
        List<RoutineDTO> GetUserRoutines();
        RoutineDTO GetUserRoutine(int routineId);
        List<RoutineDTO> GetActiveRoutines();
        StatusMessage UpdateUserRoutine(RoutineUploadModel routineUploadModel);
        StatusMessage DeleteUserRoutine(int routineId);
    }
}
