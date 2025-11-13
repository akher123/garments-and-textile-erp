using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using SCERP.DAL.IRepository;

namespace SCERP.DAL.Repository
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected SCERPDBContext Context = null;
        private ObjectContext _objectContext = null;
        public Repository(SCERPDBContext context)
        {
            Context = context;
            _objectContext = ((IObjectContextAdapter)context).ObjectContext;
            Context.Configuration.LazyLoadingEnabled = false;
            Context.Configuration.ProxyCreationEnabled = false;
        }

        /// <summary>
        /// Returns a DbSet instance for access to entities of the given type in the
        /// context, the ObjectStateManager, and the underlying store.
        /// </summary>
        protected DbSet<TEntity> ObjectSet
        {
            get
            {
                return Context.Set<TEntity>();
            }
        }

        public void Dispose()
        {
            if (Context != null)
            {
                Context.Dispose();
            }
        }

        public virtual TEntity GetById(int? id)
        {
            return ObjectSet.Find(id);
        }

      

        public virtual IQueryable<TEntity> All()
        {
            return ObjectSet.AsQueryable();
        }

        public virtual IQueryable<TEntity> Filter(Expression<Func<TEntity, bool>> predicate)
        {
            return ObjectSet.Where(predicate).AsQueryable<TEntity>();
        }

        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>
        /// An List<TEntity> that contains elements from the input sequence 
        /// that satisfy the condition specified by predicate.
        /// </returns>
        public virtual IQueryable<TEntity> Filter<T>(Expression<Func<TEntity, bool>> filter, out int total)
        {

            var _resetSet = filter != null ? ObjectSet.Where(filter).AsQueryable() :
                ObjectSet.AsQueryable();
            total = _resetSet.Count();
            return _resetSet.AsQueryable();
        }

        public bool Contains(Expression<Func<TEntity, bool>> predicate)
        {
            return ObjectSet.Count(predicate) > 0;
        }

        public virtual TEntity FindOne(Expression<Func<TEntity, bool>> predicate)
        {
            return ObjectSet.FirstOrDefault(predicate);
        }

        public virtual int Count()
        {
            return ObjectSet.Count();
        }

        public TEntity Add(TEntity entity)
        {
            TEntity addResult = ObjectSet.Add(entity);
            SaveChanges();
            return addResult;
        }

        public virtual int Count(Expression<Func<TEntity, bool>> predicate)
        {
            return ObjectSet.Count(predicate);
        }

        public virtual int Save(TEntity entity)
        {
            var entry = Context.Entry(entity);

            if (entry.State == EntityState.Detached)
            {
                ObjectSet.Add(entity);
                entry.State = EntityState.Added;
            }
            else
            {
                ObjectSet.Attach(entity);
                entry.State = EntityState.Modified;
            }

            return SaveChanges();
        }

        public virtual int Edit(TEntity entity)
        {
            var entry = Context.Entry(entity);

            ObjectSet.Attach(entity);
            entry.State = EntityState.Modified;
            
            return SaveChanges();
        }
        public virtual void Delete(TEntity entity)
        {
            ObjectSet.Remove(entity);
            SaveChanges();
        }

        public virtual int Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var objects = Filter(predicate);
            foreach (var obj in objects)
            {
                ObjectSet.Remove(obj);
            }

            return SaveChanges(); 
        }

        public virtual int DeleteOne(TEntity entity)
        {
            ObjectSet.Remove(entity);
            return SaveChanges();
        }

        public IQueryable<TEntity> Filter<Key>(Expression<Func<TEntity, bool>> filter, out int total, int index = 0, int size = 50)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether any element of a sequence satisfies a condition.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool Exists(Expression<Func<TEntity, bool>> predicate)
        {
            return ObjectSet.Any(predicate);
        }

        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns>
        /// An List<TEntity> that contains elements from the input sequence 
        /// that satisfy the condition specified by predicate.
        /// </returns>
        public List<TEntity> Search(Expression<Func<TEntity, bool>> predicate)
        {
            var query = ObjectSet.Where(predicate);

            var result = query.ToList();
            return result;
        }

        public List<TEntity> Search(Expression<Func<TEntity, bool>> predicate, out int total, int index = 0, int size = 50)
        {
            var query = ObjectSet.Where(predicate);

            total = query.Count();
            query = query.Skip(index * size).Take(size);

            var result = query.ToList();
            return result;
        }

        /// <summary>
        /// Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <returns>The number of objects written to the underlying database.</returns>
        public int SaveChanges()
        {
            try
            {
                return Context.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                var newException = new FormattedDbEntityValidationException(ex);
                throw newException;
            }
        }
        public virtual TEntity EditEnity(TEntity entity)
        {
            var entry = Context.Entry(entity);
            ObjectSet.Attach(entity);
            entry.State = EntityState.Modified;
            SaveChanges();
            return entity;
        }

        public virtual int SaveList(List<TEntity> entities)
        {
            ObjectSet.AddRange(entities);
            return SaveChanges();
        }

        public virtual DataTable ExecuteQuery(string sqlQuery)
        {
            var table = new DataTable();
            var connection = (SqlConnection)Context.Database.Connection;
            string cmdText =sqlQuery;
            try
            {
                if (connection != null && connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                    var adapter = new SqlDataAdapter(cmdText, connection);
                    adapter.Fill(table);
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }
            finally
            {
                if (connection != null && connection.State == ConnectionState.Open) connection.Close();
            }

            return table;
        }

        public IQueryable<TEntity> GetWithInclude(Expression<Func<TEntity, bool>> predicate, params string[] include)
        {
            IQueryable<TEntity> query = this.ObjectSet;
            query = include.Aggregate(query, (current, inc) => current.Include(inc));
            return query.Where(predicate);
        }

        public IQueryable<T> ExecuteQuery<T>(string sql)
        {
            return Context.Database.SqlQuery<T>(sql).AsQueryable();
        }
    }
}
