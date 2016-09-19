using Microsoft.AspNet.Identity;
using MyListApp.Api.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;

namespace MyListApp.Api.Services
{
    public abstract class AppRepositoryBase<T> : IDisposable where T : class
    {
        protected AppDbContext _context;
        protected IIdentity _user;
        protected string _userId;
        protected List<string> _updateFields;
        protected bool disposedValue = false;

        public IIdentity User
        {
            set
            {
                _user = value;
                _userId = _user.GetUserId();
            }
        }

        public AppRepositoryBase()
        {
            _context = new AppDbContext();
        }

        public AppRepositoryBase(IIdentity user)
        {
            _context = new AppDbContext();
            _user = user;
            _userId = user.GetUserId();
        }

        public virtual T Add(T item)
        {
            // Before calling base, derived classes must:
            // - manipulate item (add current userId to appropriate field)
            T result = _context.Set<T>().Add(item);
            _context.SaveChanges();
            return result;
        }

        public virtual bool Delete(int id)
        {
            // Before calling base, derived classes must:
            // - verify authorization for current user

            // look for the item
            T item = _context.Set<T>().Find(id);

            // check if the item exists
            if (item == null)
            {
                return false;
            }
            
            // delete item
            _context.Set<T>().Remove(item);
            _context.SaveChanges();

            return true;
        }

        public virtual IEnumerable<T> Get(string userIdField = "userId")
        {
            return _context.Set<T>()
                .Where(GetLambdaExpression<string>(userIdField, _userId));
        }

        public virtual T Get(int id)
        {
            // Before calling base, derived classes must:
            // - verify authorization for current user
            return _context.Set<T>().Find(id);
        }

        public virtual bool Update(int id, T newItem)
        {
            // Before calling base, derived classes must:
            // - set the field names to be updated in _updateFields
            T currentItem = _context.Set<T>().Find(id);

            // check if item exists
            if (currentItem == null)
            {
                return false;
            }

            // update properties
            foreach (string f in _updateFields)
            {
                currentItem.GetType().GetProperty(f).SetValue(currentItem, newItem.GetType().GetProperty(f).GetValue(newItem));
            }

            _context.SaveChanges();

            return true;

        }

        protected virtual Expression<Func<T, bool>> GetLambdaExpression<TParam>(string propName, TParam value)
        {
            var item = Expression.Parameter(typeof(T), "item");
            var prop = Expression.Property(item, propName);
            var compareValue = Expression.Constant(value);
            var equal = Expression.Equal(prop, compareValue);
            return Expression.Lambda<Func<T, bool>>(equal, item);
        }

        protected void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _context.Dispose();
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }

    
}
