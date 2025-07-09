using Microsoft.EntityFrameworkCore;
using Topic.QueryService.Domain.Dao;
using Topic.QueryService.Domain.Entities;
using Topic.QueryService.Infrastructure.Data;

namespace Topic.QueryService.Infrastructure.Dao;

public class CommentStorage: ICommentStorage
{
    private readonly ApplicationContext appContext;
    
    public CommentStorage(DbContextFactory dbContextFactory)
    {
        appContext = dbContextFactory.CreateDbContext();
    }
    
    public async Task AddCommentAsync(CommentEntity comment)
    {
        await appContext.Comments.AddAsync(comment);
        await appContext.SaveChangesAsync();
        
    }

    public async Task<CommentEntity> GetCommentByIdAsync(Guid commentId)
    {
        var result = await appContext.Comments
            .FirstOrDefaultAsync(c => c.Id == commentId);
        return result!;
    }

    public async Task UpdateCommentAsync(CommentEntity comment)
    {
        appContext.Comments.Update(comment);
        await appContext.SaveChangesAsync();
    }

    public async Task DeleteCommentAsync(Guid commentId)
    {
        await appContext.Comments.Where(c => c.Id == commentId).ExecuteDeleteAsync();
    }
}