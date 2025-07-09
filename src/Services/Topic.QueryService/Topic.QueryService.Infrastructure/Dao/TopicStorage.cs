using Microsoft.EntityFrameworkCore;
using Topic.QueryService.Domain.Dao;
using Topic.QueryService.Domain.Entities;
using Topic.QueryService.Infrastructure.Data;

namespace Topic.QueryService.Infrastructure.Dao;

public class TopicStorage: ITopicStorage
{
    private readonly ApplicationContext appContext;
    
    public TopicStorage(DbContextFactory dbContextFactory)
    {
        appContext = dbContextFactory.CreateDbContext();
    }
    
    public async Task AddTopicAsync(TopicEntity topic)
    {
        await appContext.Topics.AddAsync(topic);
        await appContext.SaveChangesAsync();
    }

    public async Task<TopicEntity> GetTopicByIdAsync(Guid topicId)
    {
        var result = await appContext.Topics
            .Include(t => t.Comments)
            .FirstOrDefaultAsync(t => t.Id == topicId);
        return result;
    }

    public async Task UpdateTopicAsync(TopicEntity topic)
    {
        appContext.Topics.Update(topic);
        await appContext.SaveChangesAsync();
    }

    public async Task DeleteTopicAsync(Guid topicId)
    {
        await appContext.Topics.Where(t => t.Id == topicId)
            .ExecuteDeleteAsync();
    }

    public async Task<List<TopicEntity>> GetAllTopicsAsync()
    {
        return await appContext.Topics
            .AsNoTracking()
            .Include(i => i.Comments)
            .ToListAsync();
    }

    public async Task<List<TopicEntity>> GetTopicsWithCommentsAsync()
    {
        return await appContext.Topics
            .AsNoTracking()
            .Where(t => t.Comments != null && t.Comments.Any())
            .ToListAsync();
    }

    public async Task<List<TopicEntity>> GetTopicsByAuthorAsync(string authorName)
    {
        return await appContext.Topics
            .AsNoTracking()
            .Include(t => t.Comments)
            .Where(t => t.AuthorName == authorName)
            .ToListAsync();
    }

    public async Task<List<TopicEntity>> GetTopicsWithMinLikesAsync(int numberOfLikes)
    {
        return await appContext.Topics
            .Include(t => t.Comments)
            .Where(t => t.Likes >= numberOfLikes)
            .ToListAsync();
    }
}