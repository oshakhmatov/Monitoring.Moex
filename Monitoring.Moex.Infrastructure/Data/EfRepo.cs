using Microsoft.EntityFrameworkCore;
using Monitoring.Moex.Core.Common;
using Monitoring.Moex.Core.DataAccess;
using Monitoring.Moex.Core.Models;
using System.Linq.Expressions;

namespace Monitoring.Moex.Infrastructure.Data
{
    public class EfRepo<TModel> : IRepo<TModel> where TModel : class
    {
        private protected readonly AppDbContext _dbContext;
        private protected readonly DbSet<TModel> _set;

        public EfRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            _set = dbContext.Set<TModel>();
        }

        public virtual async Task<TModel> GetAsync(Func<TModel, bool>? predicate = null)
        {
            if (predicate == null)
                return await _set.FirstOrDefaultAsync();

            var expression = Expression.Lambda<Func<TModel, bool>>(Expression.Call(predicate.Method));
            return await _set.FirstOrDefaultAsync(expression);
        }

        public virtual async Task<List<TModel>> ListAsync(Func<TModel, bool>? predicate = null)
        {
            if (predicate == null)
                return await _set.ToListAsync();

            var expression = Expression.Lambda<Func<TModel, bool>>(Expression.Call(predicate.Method));
            return await _set.Where(expression).ToListAsync();
        }

        public virtual async Task AddAsync(TModel model)
        {
            Guard.NotNull(model, nameof(model));

            _set.Add(model);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task AddRangeAsync(IEnumerable<TModel> models)
        {
            Guard.NotNull(models, nameof(models));

            _set.AddRange(models);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TModel model)
        {
            Guard.NotNull(model, nameof(model));

            _set.Update(model);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<TModel> models)
        {
            Guard.NotNull(models, nameof(models));

            _set.UpdateRange(models);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TModel model)
        {
            Guard.NotNull(model, nameof(model));

            _set.Remove(model);
            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<TModel> models)
        {
            Guard.NotNull(models, nameof(models));

            _set.RemoveRange(models);
            await _dbContext.SaveChangesAsync();
        }
    }
}
