using Microsoft.AspNet.Identity;
using MyListApp.Api.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;

namespace MyListApp.Api.Services
{
    public abstract class AppRepositoryBase<T> where T : class
    {
        protected AppDbContext _context;
        protected IIdentity _user;
        protected string _userId;
        protected IQueryable<T> _set;
        protected T _item;
        protected List<string> _updateFields;

        public AppRepositoryBase(IIdentity user)
        {
            _context = new AppDbContext();
            _user = user;
            _userId = user.GetUserId();
        }

        public virtual T Add(T item)
        {
            // Before calling base, derived classes must:
            // - verify authorization for current user
            // - manipulate item (add current userId to appropriate field)
            T result = _context.Set<T>().Add(item);
            _context.SaveChanges();
            return result;
        }

        public virtual bool Delete(int id)
        {
            // Before calling base, derived classes must:
            // - verify authorization for current user
            // - find the item and store in _item
            if (_item == null)
            {
                return false;
            }

            _context.Set<T>().Remove(_item);
            _context.SaveChanges();
            return true;
        }

        public virtual IEnumerable<T> Get()
        {
            // Before calling base, derived classes must:
            // - verify authorization for current user
            // - find the set and store in _set
            return _set;
        }

        public virtual T Get(int id, string idField = "Id")
        {
            // Before calling base, derived classes must:
            // - verify authorization for current user
            // - find the item and store in _item
            return _set.Where(GetLambda<T, int>(idField, id)).FirstOrDefault<T>();
        }

        public virtual bool Update(int id, T item, string idField = "Id")
        {
            // Before calling base, derived classes must:
            // - verify authorization for current user
            // - find the item and store in _item
            // - set the field names to be updated in _updateFields
            _item = _set.Where(GetLambda<T, int>(idField, id)).FirstOrDefault<T>();
            if (_item != null)
            {
                foreach (string f in _updateFields)
                {
                    _item.GetType().GetProperty(f).SetValue(_item, item.GetType().GetProperty(f).GetValue(item));
                }
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }
        private Expression<Func<T, bool>> GetLambda<TItem, TValue>(string propName, TValue value)
        {
            var param = Expression.Parameter(typeof(TItem));
            var body = Expression.Equal(Expression.Property(param, propName),
                Expression.Constant(value));
            return Expression.Lambda<Func<T, bool>>(body, param);
        }
    }
}
