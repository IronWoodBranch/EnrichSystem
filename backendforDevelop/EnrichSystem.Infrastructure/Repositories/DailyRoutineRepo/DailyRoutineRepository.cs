using EnrichSystem.Domain.DailyRoutines;
using EnrichSystem.Infrastructure.DbContexts;
using EnrichSystem.Usecase.Interfaces.Repositories.DailyRoutineRepoInterface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnrichSystem.Infrastructure.Repositories.DailyRoutineRepo
{
    public class DailyRoutineRepository : IDailyRoutineReopository
    {
        private readonly ApplicationDbContext _context;
        public DailyRoutineRepository(ApplicationDbContext dbcontext) { 
            _context = dbcontext;
        }

        public async Task CreateNewRoutine(DailyRoutine dailyRoutine)
        {
            _context.Add(dailyRoutine);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// todo:做空值的异常处理
        /// </summary>
        /// <param name="dailyRoutine"></param>
        /// <returns></returns>
        public async Task DeleteRoutine(DailyRoutine dailyRoutine)
        {
            var obj = _context.DailyRoutines.Where(x => x.Id == dailyRoutine.Id).FirstOrDefault();
            if (obj != null)
            {
                _context.Remove(obj);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<DailyRoutine>> GetAllRoutines()
        {
            var result = await _context.DailyRoutines.ToListAsync<DailyRoutine>();
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dailyRoutine"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task UpdateRoutine(DailyRoutine dailyRoutine)
        {
            var obj = _context.DailyRoutines.Where(x => x.Id == dailyRoutine.Id).FirstOrDefault();
            if (obj != null)
            {
                obj = dailyRoutine;
            }
            await _context.SaveChangesAsync();
        }
    }
}
