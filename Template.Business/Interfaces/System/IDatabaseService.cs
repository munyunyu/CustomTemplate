using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Template.Library.Tables;

namespace Template.Business.Interfaces.System
{
    public interface IDatabaseService 
    {
        T Add<T>(T model) where T : BaseEntity;
        Task<T> AddAsync<T>(T model, bool unsaveChanges = false) where T : BaseEntity;
        Task<IEnumerable<T>> AddRangeAsync<T>(IEnumerable<T> model) where T : BaseEntity;
        int Count<T>(Expression<Func<T, bool>> func) where T : BaseEntity;
        Task<int> CountAsync<T>(Expression<Func<T, bool>> func) where T : BaseEntity;
        bool Exist<T>(Expression<Func<T, bool>> func) where T : BaseEntity;
        Task<bool> ExistAsync<T>(Expression<Func<T, bool>> func) where T : BaseEntity;
        T? GetAll<T>(Expression<Func<T, bool>> func, params string[] includes) where T : BaseEntity;
        Task<IEnumerable<T>> GetAllAsync<T>(int maxDepth = 0, int skip = 0, int count = 50, params string[] includes) where T : BaseEntity;
        Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> func, int maxDepth = 0, int skip = 0, int count = 50, params string[] includes) where T : BaseEntity;
        Task<T?> GetAsync<T>(Expression<Func<T, bool>> func, params string[] includes) where T : BaseEntity;
        Task<T?> GetAsync<T>(Expression<Func<T, bool>> func, int maxDepth = 0) where T : BaseEntity;
        T? SqlQueryRawCommand<T>(string query);
        Task<T> UpdateAsync<T>(T model, bool unsaveChanges = false) where T : BaseEntity;
        Task<IEnumerable<T>> UpdateRangeAsync<T>(IEnumerable<T> model) where T : BaseEntity;
        Task SaveChangesAsync();
        Task<T?> GetLastAsync<T>(Expression<Func<T, bool>> func, Expression<Func<T, object>> orderBy, int maxDepth = 0) where T : BaseEntity;
        Task<T?> GetFirstAsync<T>(Expression<Func<T, bool>> func, int maxDepth = 0) where T : BaseEntity;
    }
}
