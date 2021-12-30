using Microsoft.EntityFrameworkCore;
using Monitoring.Moex.Core.DataAccess;
using System.Linq.Expressions;

namespace Monitoring.Moex.Infrastructure.Data
{
    public class EfRepo<TModel> : IRepo<TModel> where TModel : class
    {
        private protected readonly AppDbContext _dbContext;
        private protected readonly DbSet<TModel> _set;

        public EfRepo(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

            _set = dbContext.Set<TModel>();
        }

        public virtual async Task<TModel?> GetAsync(Func<TModel, bool>? predicate = null)
        {
            IQueryable<TModel> models = _set;

            if (predicate != null)
            {
                var expression = Expression.Lambda<Func<TModel, bool>>(Expression.Call(predicate.Method));

                models = _set.Where(expression);
            }

            return await models.FirstOrDefaultAsync();
        }

        public virtual async Task<List<TModel>> ListAsync(Func<TModel, bool>? predicate = null)
        {
            IQueryable<TModel> models = _set;

            if (predicate != null)
            {
                var expression = Expression.Lambda<Func<TModel, bool>>(Expression.Call(predicate.Method));

                models = _set.Where(expression);
            }

            return await models.ToListAsync();
        }

        public virtual async Task AddAsync(TModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _set.Add(model);

            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task AddRangeAsync(IEnumerable<TModel> models)
        {
            if (models is null)
            {
                throw new ArgumentNullException(nameof(models));
            }

            _set.AddRange(models);

            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task UpdateAsync(TModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _set.Update(model);

            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task UpdateRangeAsync(IEnumerable<TModel> models)
        {
            if (models is null)
            {
                throw new ArgumentNullException(nameof(models));
            }

            _set.UpdateRange(models);

            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(TModel model)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _set.Remove(model);

            await _dbContext.SaveChangesAsync();
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<TModel> models)
        {
            if (models is null)
            {
                throw new ArgumentNullException(nameof(models));
            }

            _set.RemoveRange(models);

            await _dbContext.SaveChangesAsync();
        }
    }
}
