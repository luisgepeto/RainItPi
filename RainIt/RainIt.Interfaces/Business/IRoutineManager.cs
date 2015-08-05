using System;
using System.Collections.Generic;
using RainIt.Domain.DTO;

namespace RainIt.Interfaces.Business
{
    public interface IRoutineManager
    {
        StatusMessage AddUserRoutine(RoutineUploadModel routineUploadModel);
        StatusMessage SetToTest(RoutineUploadModel routineUploadModel);
        UploadConstraintParameters GetUploadConstraintParameters();
        List<RoutineDTO> GetUserRoutines();
        RoutineDTO GetUserRoutine(int routineId);
        List<RoutineDTO> GetActiveRoutines();
        RoutineDTO GetTestRoutine();
        StatusMessage UpdateUserRoutine(RoutineUploadModel routineUploadModel);
        StatusMessage DeleteUserRoutine(int routineId);
    }
}
