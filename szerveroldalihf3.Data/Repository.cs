using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using szerveroldalihf3.Entities.Helpers;
using szerveroldalihf3.Entities.Entity;

namespace szerveroldalihf3.Data
{
    public class Repository<T> where T : class, IIdEntity
    {
        JiraContext ctx;
        public Repository(JiraContext ctx)
        {
            this.ctx = ctx;
        }

        public void Create(T entity)
        {
            ctx.Set<T>().Add(entity);
            ctx.SaveChanges();
        }

        public async Task CreateAsync(T entity)
        {
            ctx.Set<T>().Add(entity);
            await ctx.SaveChangesAsync();
        }

        public T FindById(string id)
        {
            return ctx.Set<T>().First(t => t.Id == id);
        }

        public IQueryable<T> GetAll()
        {
            return ctx.Set<T>();
        }
    }
}
