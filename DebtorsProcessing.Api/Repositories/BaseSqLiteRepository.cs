using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using DebtorsProcessing.DatabaseModel;
using DebtorsProcessing.DatabaseModel.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace DebtorsProcessing.Api.Repositories;

public abstract class BaseSqLiteRepository<T> : IOdataEntityRepository<T> where T: BaseEntity
{
    protected abstract Expression<Func<DebtorsContext, DbSet<T>>> DbSetSelector();
    private Func<DebtorsContext, DbSet<T>> dbSetFunc;
    protected Func<DebtorsContext, DbSet<T>> DbSetFunc => dbSetFunc ??= DbSetSelector().Compile();
    

    public IQueryable<T> GetAllEntities()
    {
        using DebtorsContext db = new();
        return DbSetFunc(db);
    }
    public IQueryable<T> GetEntity(Guid id)
    {
        using DebtorsContext db = new();
        return DbSetFunc(db).Where(x=>x.Id == id);
    }

    public async Task<T> GetEntityById(Guid id)
    {
        await using DebtorsContext db = new();
        return await DbSetFunc(db).SingleOrDefaultAsync(x=>x.Id == id);
    }

    public async Task UpdateEntity(T entity)
    {
        await using DebtorsContext db = new();
        db.Attach(entity).State = EntityState.Modified;
        await db.SaveChangesAsync();
    }

    public async Task AddEntity(T entity)
    {
        await using DebtorsContext context = new();
        BeforeAddingEntity(context, entity);
        await DbSetFunc(context).AddAsync(entity);
        await context.SaveChangesAsync();
    }

    protected virtual void BeforeAddingEntity(DbContext db, T entity)
    {
        
    }

    public async Task DeleteEntity(Guid id)
    {
        await using DebtorsContext context = new();
        T entity = await DbSetFunc(context).SingleAsync(x => x.Id == id);
        context.Entry(entity).State = EntityState.Deleted;
        await context.SaveChangesAsync();
    }

    public async Task DeleteEntity(T entity)
    {
        await using DebtorsContext context = new();
        context.Attach(entity).State = EntityState.Deleted;
        await context.SaveChangesAsync();
    }
}