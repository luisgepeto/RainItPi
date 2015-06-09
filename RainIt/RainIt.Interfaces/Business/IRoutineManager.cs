using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RainIt.Domain.DTO;

namespace RainIt.Interfaces.Business
{
    public interface IRoutineManager
    {
        StatusMessage AddUserRoutine(RoutineList routineList);
        List<RoutineList> GetUserRoutines();
        RoutineList GetUserRoutine(int routineId);
        StatusMessage UpdateUserRoutine(RoutineList routineList);
        StatusMessage DeleteUserRoutine(int routineId);
    }
}
