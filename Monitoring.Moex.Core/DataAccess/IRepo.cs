namespace Monitoring.Moex.Core.DataAccess
{
    public interface IRepo<TModel> where TModel : class
    {
        public Task<TModel?> GetAsync(Func<TModel, bool>? predicate);
        public Task<List<TModel>> ListAsync(Func<TModel, bool>? predicate = null);

        public Task AddAsync(TModel model);
        public Task AddRangeAsync(IEnumerable<TModel> models);

        public Task UpdateAsync(TModel model);
        public Task UpdateRangeAsync(IEnumerable<TModel> models);

        public Task DeleteAsync(TModel model);
        public Task DeleteRangeAsync(IEnumerable<TModel> models);
    }
}
