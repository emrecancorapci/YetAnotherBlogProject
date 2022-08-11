﻿using BlogProject.DataAccess.Data;
using BlogProject.DataAccess.Repositories.Relations.Interfaces;
using BlogProject.Entities.Base;
using BlogProject.Entities.Relations;
using Microsoft.EntityFrameworkCore;

namespace BlogProject.DataAccess.Repositories.Relations;

public class EFUsersCommentReactionsRepository : IUsersCommentReactionsRepository
{
    private readonly BlogProjectDbContext context;
    public EFUsersCommentReactionsRepository(BlogProjectDbContext context) =>
        this.context = context;

    public async Task<IList<UsersCommentReactions>> GetAllAsync() =>
        await context.UsersCommentReactions.ToListAsync();

    public async Task<IList<Comment>> GetReactedCommentsByUserIdAsync(int userId)
    {
        var comments = new List<Comment>();
        var commentIds = context.UsersCommentReactions
            .Where(pe => pe.UserId == userId)
            .Select(p => p.CommentId);

        foreach (var id in commentIds)
            comments.Add(await context.Comments.FindAsync(id));

        return comments;
    }

    public async Task<IList<User>> GetReactedUsersByCommentIdAsync(int commentId)
    {
        var users = new List<User>();
        var userIds = context.UsersCommentReactions
            .Where(pe => pe.CommentId == commentId)
            .Select(p => p.UserId);

        foreach (var id in userIds)
            users.Add(await context.Users.FindAsync(id));

        return users;
    }

    public async Task<int> AddAsync(UsersCommentReactions entity)
    {
        await context.UsersCommentReactions.AddAsync(entity);
        var affectedRows = await context.SaveChangesAsync();

        return affectedRows;
    }

    public async Task<int> AddAsync(int userId, int commentId)
    {
        var entity = new UsersCommentReactions { UserId = userId, CommentId = commentId };
        await context.UsersCommentReactions.AddAsync(entity);

        var affectedRows = await context.SaveChangesAsync();
        return affectedRows;
    }

    public async Task<int> DeleteAsync(UsersCommentReactions entity)
    {
        context.UsersCommentReactions.Remove(entity);

        var affectedRows = await context.SaveChangesAsync();
        return affectedRows;
    }

    public async Task<int> DeleteAsync(int userId, int commentId)
    {
        var entity = new UsersCommentReactions { UserId = userId, CommentId = commentId };
        context.UsersCommentReactions.Remove(entity);

        var affectedRows = await context.SaveChangesAsync();
        return affectedRows;
    }

    public async Task<int> DeleteAllCommentReactsByUserIdAsync(int userId)
    {
        var usersCommentReactionsList = await context.UsersCommentReactions
            .Where(usersCommentReactions => usersCommentReactions.UserId == userId)
            .ToListAsync();

        foreach (var usersCommentReactions in usersCommentReactionsList)
            context.UsersCommentReactions.Remove(usersCommentReactions);

        var affectedRows = await context.SaveChangesAsync();
        return affectedRows;
    }


    public async Task<int> DeleteAllCommentReactsByCommentIdAsync(int commentId)
    {
        var usersLikesList = await context.UsersCommentReactions
            .Where(usersLikes => usersLikes.CommentId == commentId)
            .ToListAsync();

        foreach (var usersLikes in usersLikesList)
            context.UsersCommentReactions.Remove(usersLikes);

        var affectedRows = await context.SaveChangesAsync();
        return affectedRows;
    }
}