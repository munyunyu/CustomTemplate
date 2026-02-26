using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Template.Business.Interfaces.System;
using Template.Database.Context;
using Template.Database.Extensions;
using Template.Library.Extensions;
using Template.Library.Tables;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Template.Business.Services.System
{
    public class DatabaseService : IDatabaseService
    {
        private readonly ApplicationContext context;

        public DatabaseService(ApplicationContext context)
        {
            this.context = context;
        }

        public T Add<T>(T model) where T : BaseEntity
        {
            try
            {
                model.Hash = model.GenerateHash();

                context.Add(model);
                context.SaveChanges();
                return model;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.GetAllMessages());
            }
        }
        public async Task<T> AddAsync<T>(T model, bool unsaveChanges = false) where T : BaseEntity
        {
            try
            {
                model.Hash = model.GenerateHash();

                context.Add(model);
                if(unsaveChanges == false) await context.SaveChangesAsync();
                return model;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.GetAllMessages());
            }
            catch (OperationCanceledException ex)
            {
                throw new Exception(ex.GetAllMessages());
            }
        }
        public bool Exist<T>(Expression<Func<T, bool>> func) where T : BaseEntity
        {
            bool result = context.Set<T>().AsNoTracking().Any(func);
            return result;
        }
        public async Task<bool> ExistAsync<T>(Expression<Func<T, bool>> func) where T : BaseEntity
        {
            bool result = await context.Set<T>().AsNoTracking().AnyAsync(func);
            return result;
        }
        public int Count<T>(Expression<Func<T, bool>> func) where T : BaseEntity
        {
            var result = context.Set<T>().AsNoTracking().Count(func);
            return result;
        }
        public async Task<int> CountAsync<T>(Expression<Func<T, bool>> func) where T : BaseEntity
        {
            var result = await context.Set<T>().AsNoTracking().CountAsync(func);
            return result;
        }
        public async Task<T?> GetAsync<T>(Expression<Func<T, bool>> func, params string[] includes) where T : BaseEntity
        {
            IQueryable<T> query = context.Set<T>().AsNoTracking().IncludeMultiple(includes);

            var result = await query.SingleOrDefaultAsync(func);

            return result;
        }
        public async Task<T?> GetAsync<T>(Expression<Func<T, bool>> func, int maxDepth = 0) where T : BaseEntity
        {
            IQueryable<T> query;

            if (maxDepth <= 0) query = context.Set<T>().AsNoTracking();
            else query = context.Set<T>().AsNoTracking().IncludeAllRecursively(maxDepth);

            var result = await query.SingleOrDefaultAsync(func);

            return result;
        }


        public async Task<T?> GetFirstAsync<T>(Expression<Func<T, bool>> func,int maxDepth = 0) where T : BaseEntity
        {
            IQueryable<T> query = maxDepth <= 0
                ? context.Set<T>().AsNoTracking()
                : context.Set<T>().AsNoTracking().IncludeAllRecursively(maxDepth);

            return await query.FirstOrDefaultAsync(func);
        }

        public async Task<T?> GetLastAsync<T>(Expression<Func<T, bool>> func,Expression<Func<T, object>> orderBy,int maxDepth = 0) where T : BaseEntity
        {
            IQueryable<T> query = maxDepth <= 0
                ? context.Set<T>().AsNoTracking()
                : context.Set<T>().AsNoTracking().IncludeAllRecursively(maxDepth);

            return await query
                .Where(func)
                .OrderByDescending(orderBy)
                .FirstOrDefaultAsync();
        }



        public T? GetAll<T>(Expression<Func<T, bool>> func, params string[] includes) where T : BaseEntity
        {
            IQueryable<T> query = context.Set<T>().AsNoTracking().IncludeMultiple(includes);

            var result = query.SingleOrDefault(func);

            return result;
        }
        public async Task<IEnumerable<T>> GetAllAsync<T>(int maxDepth = 0, int skip = 0, int count = 50, params string[] includes) where T : BaseEntity
        {
            IQueryable<T> query = context.Set<T>().AsNoTracking().IncludeMultiple(includes);

            if (maxDepth > 0) query = query.IncludeAllRecursively(maxDepth);

            var result = await query.OrderByDescending(o => o.CreatedDate).Skip(skip).Take(count).ToListAsync();

            return result;
        }
        public async Task<IEnumerable<T>> GetAllAsync<T>(Expression<Func<T, bool>> func, int maxDepth = 0, int skip = 0, int count = 50, params string[] includes) where T : BaseEntity
        {
            IQueryable<T> query = context.Set<T>().AsNoTracking().IncludeMultiple(includes);

            if (maxDepth > 0) query = query.IncludeAllRecursively(maxDepth);

            var result = await query.Where(func).OrderByDescending(o => o.CreatedDate).Skip(skip).Take(count).ToListAsync();

            return result;
        }
        public async Task<T> UpdateAsync<T>(T model, bool unsaveChanges = false) where T : BaseEntity
        {
            try
            {
                model.Hash = model.GenerateHash();

                context.Update(model);
                if (unsaveChanges == false) await context.SaveChangesAsync();
                return model;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.GetAllMessages());
            }

        }
        public async Task<IEnumerable<T>> UpdateRangeAsync<T>(IEnumerable<T> model) where T : BaseEntity
        {
            try
            {
                context.UpdateRange(model);
                //context.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return model;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.GetAllMessages());
            }
        }

        public async Task<IEnumerable<T>> AddRangeAsync<T>(IEnumerable<T> model) where T : BaseEntity
        {
            try
            {
                await context.AddRangeAsync(model);
                //context.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return model;
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.GetAllMessages());
            }
        }
        public T? SqlQueryRawCommand<T>(string query)
        {
            try
            {
                context.Database.OpenConnection();

                using var command = context.Database.GetDbConnection().CreateCommand();
                command.CommandText = query;
                command.CommandType = CommandType.Text;

                using var reader = command.ExecuteReader();
                var result = new StringBuilder();

                result.Append('[');
                var first = true;
                while (reader.Read())
                {
                    if (!first) result.Append(',');
                    first = false;

                    result.Append('{');
                    for (var i = 0; i < reader.FieldCount; i++)
                    {
                        if (i > 0) result.Append(',');
                        var name = reader.GetName(i);
                        var value = reader.IsDBNull(i) ? "null" : JsonSerializer.Serialize(reader.GetValue(i));
                        result.Append($"\"{name}\":{value}");
                    }
                    result.Append('}');
                }
                result.Append(']');

                var json = result.ToString();

                return JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            finally
            {
                context.Database.CloseConnection();
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.GetAllMessages());
            }
            catch(OperationCanceledException ex)
            {
                throw new Exception(ex.GetAllMessages());
            }            
        }

        public async Task DeleteAsync<T>(T model) where T : BaseEntity
        {
            try
            {
                context.Remove(model);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.GetAllMessages());
            }
        }

        public async Task DeleteRangeAsync<T>(IEnumerable<T> models) where T : BaseEntity
        {
            try
            {
                context.RemoveRange(models);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.GetAllMessages());
            }
        }

        public async Task SoftDeleteAsync<T>(Expression<Func<T, bool>> func) where T : BaseEntity
        {
            try
            {
                var entity = await context.Set<T>().FirstOrDefaultAsync(func);
                if (entity == null) return;

                entity.IsDeleted = true;
                entity.LastUpdatedDate = DateTime.UtcNow;
                context.Update(entity);
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception(ex.GetAllMessages());
            }
        }
    }
}
