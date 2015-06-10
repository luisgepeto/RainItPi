﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RainIt.Domain.DTO;

namespace RainIt.Interfaces.Business
{
    public interface IRoutineManager
    {
        StatusMessage AddUserRoutine(RoutineDTO routineDTO);
        List<RoutineDTO> GetUserRoutines();
        RoutineDTO GetUserRoutine(int routineId);
        RoutineDTO GetActiveUserRoutine();
        StatusMessage UpdateUserRoutine(RoutineDTO routineDTO);
        StatusMessage SetActive(int routineId);
        StatusMessage DeleteUserRoutine(int routineId);
    }
}
