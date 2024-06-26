using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
  public class CommentRepository(ApplicationDBContext context) : ICommentRepository
  {
    private readonly ApplicationDBContext _context = context;

        public async Task<Comment> CreateAsync(Comment commentModel)
    {
      await _context.Comments.AddAsync(commentModel);
      await _context.SaveChangesAsync();
      return commentModel;
    }

    public async Task<Comment?> DeleteAsync(int id)
    {
      var commentModel = await _context.Comments.FirstOrDefaultAsync(x => id == x.Id);

      if (null == commentModel)
      {
        return null;
      }

      _context.Comments.Remove(commentModel);
      await _context.SaveChangesAsync();
      return commentModel;
    }

    public async Task<List<Comment>> GetAllAsync()
    {
      return await _context.Comments.ToListAsync();
    }

    public async Task<Comment?> GetByIdAsync(int id)
    {
      return await _context.Comments.FindAsync(id);
    }

    public async Task<Comment?> UpdateAsync(int id, Comment commentModel)
    {
      var existingComment = await _context.Comments.FindAsync(id);

      if (null == existingComment)
      {
        return null;
      }

      existingComment.Title = commentModel.Title;
      existingComment.Content = commentModel.Content;

      await _context.SaveChangesAsync();

      return existingComment;
    }
  }

}