using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace LibreriaGenericaPropia.Generic
{
    public abstract class GenericCode<T> where T : class
    {
        protected DbContext _dataContext;

        public IQueryable<T> BuildQuery(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            int? skip = null,
            int? take = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null
        )
        {
            IQueryable<T> query = _dataContext.Set<T>();

            if (predicate != null)
                query = query.Where(predicate);

            if (include != null)
                query = include(query);

            if (orderBy != null)
                query = orderBy(query);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);

            return query;
        }

        public async Task<T?> FirsOrDefaultAsync(Expression<Func<T, bool>> predicate,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null)
        {
            var pred = BuildQuery(predicate, include);
            return await pred.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAllAsync(
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            int? skip = null,
            int? take = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            var pred = BuildQuery(predicate, include, skip, take, orderBy);
            return await pred.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetPagedAsync(
            int page,
            int take,
            Expression<Func<T, bool>>? predicate = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>>? include = null,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            if (page < 1) throw new ArgumentOutOfRangeException(nameof(page), "Page number must be greater than 0.");
            if (take < 1) throw new ArgumentOutOfRangeException(nameof(take), "Take must be greater than 0.");

            var query = BuildQuery(predicate, include, ((page - 1) * take), (take), orderBy);
            return await query.ToListAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await _dataContext.Set<T>().AddAsync(entity);
            await _dataContext.SaveChangesAsync();
            return entity;
        }

        public async Task<int> AddRangeAsync(IEnumerable<T> entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            await _dataContext.Set<T>().AddRangeAsync(entity);
            return await _dataContext.SaveChangesAsync();
        }

        public async Task<T> UpdateAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dataContext.Set<T>().Update(entity);
            await _dataContext.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(IEnumerable<T> entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dataContext.Set<T>().UpdateRange(entity);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsyncNoTracker(Expression<Func<T, bool>> pred, Expression<Func<SetPropertyCalls<T>, SetPropertyCalls<T>>> toChange)
        {
            return await _dataContext.Set<T>().Where(pred).ExecuteUpdateAsync(toChange) > 0;
        }

        public async Task<bool> DeleteAsync(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dataContext.Set<T>().Remove(entity);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(IEnumerable<T> entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            _dataContext.Set<T>().RemoveRange(entity);
            return await _dataContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsyncNoTracker(Expression<Func<T, bool>> pred) => await _dataContext.Set<T>().Where(pred).ExecuteDeleteAsync() > 0;

        public async Task<int> CountAsync(Expression<Func<T, bool>> pred) => await _dataContext.Set<T>().AsNoTracking().CountAsync(pred);

        public async Task<int> CountAsync() => await _dataContext.Set<T>().AsNoTracking().CountAsync();

        public async Task<IEnumerable<T>> FromSql(FormattableString query) => await _dataContext.Set<T>().FromSql(query).ToListAsync();

        public async Task<int> ExecuteSqlAsync(FormattableString query) => await _dataContext.Database.ExecuteSqlAsync(query);

    }
}
