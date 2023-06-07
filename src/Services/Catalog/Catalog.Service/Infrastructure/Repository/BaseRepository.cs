using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using catalog.service.Infrastructure.DataStore;
using Catalog.API.Contracts;
using Catalog.API.Infrastructure.DataStore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Catalog.API.Infrastructure.Repository
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        private readonly DataContext _ctx;
        protected readonly DataContext Context;
        

        // Used for testing if we explicitly pass in DbContext object
        protected BaseRepository(DataContext ctx)
        {
            _ctx = ctx;
            Context = _ctx;
        }

        /// <summary>
        ///     Adds given domain class <T> to underlying database.
        /// </summary>
        /// <param name="entity">Instance of Entity Class<T></param>
        public virtual async Task Add(T entity)
        {
            _ctx.Set<T>().Add(entity);
            await Save();
        }

        /// <summary>
        ///     Deletes given domain class <T> from underlying database.
        /// </summary>
        /// <param name="entity">Instance of Entity Class</param>
        public virtual async Task Remove(T entity)
        {
            _ctx.Set<T>().Remove(entity);
            await Save();
        }

        /// <summary>
        ///     Edits given domain class <T> in underlying database.
        /// </summary>
        /// <param name="entity">Instance of Entity Class<T></param>
        public virtual async Task Update(T entity)
        {
            // Type DBEntityEntry exposes information about target Entity Class<T>.
            // DbContext.Entry() returns DbEntityEntry object for given Entity Class<T>.
            EntityEntry entry = _ctx.Entry(entity);

            if (entry != null)
                switch (entry.State)
                {
                    case EntityState.Detached:
                        AttachAndMarkModfied(entity);
                        break;

                    case EntityState.Deleted:
                        // CurrentValues returns current property values for given Entity Class<T>. 
                        // CurrentValues is exposed as DbPropertyValue class, which is a 
                        // collection of all properties for the underlying object.
                        // DbPropertyValues.setValues() sets value for dictionary property
                        // collection by reading values from another dictionary.
                        // Source dictaionary must be the same type as target dictionary,
                        // or type derived from this dictionary.
                        entry.CurrentValues.SetValues(entity);
                        entry.State = EntityState.Modified;
                        break;

                    default:
                        entry.CurrentValues.SetValues(entity);
                        break;
                }
            else
                AttachAndMarkModfied(entity);

            await Save();
        }

        /// <summary>
        ///     Instructs Entity Framework to iterate through Context object,
        ///     generating appropriate Insert, Edit and Delete statements.
        /// </summary>
        /// <returns>Number of records changed</returns>
        public virtual async Task<int> Save()
        {
            var returnValue = 0;
            try
            {
                returnValue = _ctx.SaveChanges();
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Could not Save in BaseRepository : {ex.Message}");
            }
            catch (Exception ex)
            {
                //var traveredMessage = ExceptionHandlingUtilties.TraverseException(ex);
                //throw new Exception($"Could not Save in BaseRepository : {traveredMessage}");
                throw new Exception($"Could not Save in BaseRepository : {ex.Message}");
            }

            //_ctx.ChangeTracker.DetectChanges();
            return returnValue;
        }

        /// <summary>
        ///     Retrieves all records from given domain class
        ///     <T>
        ///         .
        ///         Query returns type IQueryable so that the query
        ///         can be filtered with additional query operators.
        /// </summary>
        /// <returns></returns>
        protected virtual IQueryable<T> Get()
        {
            return _ctx.Set<T>().AsQueryable();
        }

        protected virtual bool IsEmpty()
        {
            return !_ctx.Set<T>().Any();
        }

        /// <summary>
        ///     Executes Linq query expression as a predicate
        ///     against given domain class <T>.
        /// </summary>
        /// <param name="predicate">Linq Query Expression</param>
        /// <returns></returns>
        protected virtual async Task<IQueryable<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return _ctx.Set<T>().Where(predicate).AsQueryable();
        }
             
        /// <summary>
        ///     Leverages Entity Framework's Find method.
        ///     Find will first attempt to find given domain class
        ///     <T>
        ///         in the instantiated Context object. If not found, Find()
        ///         then queries underlying database for given domain class <T>.
        /// </summary>
        /// <param name="entityId">Id of requested entity class</param>
        /// <returns></returns>
        protected virtual async Task<T> FindById(int entityId)
        {
            return _ctx.Set<T>().Find(entityId);
        }

        /// <summary>
        ///     Helper method to attach given Entity Class
        ///     <T>
        ///         to context object and
        ///         set the state to 'modified' so that Entity Framework will generate
        ///         SQL Update statement.
        /// </summary>
        /// <param name="entity"></param>
        private void AttachAndMarkModfied(T entity)
        {
            _ctx.Set<T>().Attach(entity);
            _ctx.Entry(entity).State = EntityState.Modified;
        }

        // Seed database with prouductinitialier
        //public async Task SeedData(string correlationToken)
        //{
        //    try
        //    {
        //        var productInitailizer = new ProductInitializer(_ctx);
        //        await productInitailizer.InitializeDatabaseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception($"Could not run Product Initializer : {ex.Message}");
        //    }
        //}


        public async Task ClearData(string correlationToken)
        {
            try
            {
                // Delete data from all tables
                await _ctx.Database.ExecuteSqlRawAsync("DELETE FROM Products");
                await _ctx.Database.ExecuteSqlRawAsync("DELETE FROM Artists");
                await _ctx.Database.ExecuteSqlRawAsync("DELETE FROM Genres");
                await _ctx.Database.ExecuteSqlRawAsync("DELETE FROM Conditions");
                await _ctx.Database.ExecuteSqlRawAsync("DELETE FROM Mediums");
                await _ctx.Database.ExecuteSqlRawAsync("DELETE FROM Descriptions");
                await _ctx.Database.ExecuteSqlRawAsync("DELETE FROM Status");
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Could not Clear Data in BaseRepository : {ex.Message}");
            }
            catch (Exception ex)
            {
                //var traveredMessage = ExceptionHandlingUtilties.TraverseException(ex);
                //throw new Exception($"Could not Save in BaseRepository : {traveredMessage}");
                throw new Exception($"Could not Clear Data in BaseRepository : {ex.Message}");
            }

            try
            {
                // Reset identity columns
                await _ctx.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Products', RESEED, 1);");
                await _ctx.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Artists', RESEED, 1);");
                await _ctx.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Genres', RESEED, 1);");
                await _ctx.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Conditions', RESEED, 1);");
                await _ctx.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Mediums', RESEED, 1);");
                await _ctx.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Descriptions', RESEED, 1);");
                await _ctx.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Status', RESEED, 1);");
            }
            catch (DbUpdateException ex)
            {
                throw new Exception($"Could not reset identity value in BaseRepository : {ex.Message}");
            }
            catch (Exception ex)
            {
                //var traveredMessage = ExceptionHandlingUtilties.TraverseException(ex);
                //throw new Exception($"Could not Save in BaseRepository : {traveredMessage}");
                throw new Exception($"Could not reset identity value in BaseRepository : {ex.Message}");
            }

        }
    }
}