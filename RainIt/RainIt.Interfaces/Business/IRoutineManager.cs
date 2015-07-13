using System;
using System.Collections.Generic;
using RainIt.Domain.DTO;

namespace RainIt.Interfaces.Business
{
    public interface IRoutineManager
    {
        StatusMessage AddUserRoutine(RoutineDTO routineDTO);
        StatusMessage SetToTest(List<int> patternIdList, List<Guid> deviceIdentifierList);
        List<RoutineDTO> GetUserRoutines();
        RoutineDTO GetUserRoutine(int routineId);
        List<RoutineDTO> GetActiveRoutines();
        StatusMessage UpdateUserRoutine(RoutineDTO routineDTO);
        StatusMessage DeleteUserRoutine(int routineId);
    }
}
