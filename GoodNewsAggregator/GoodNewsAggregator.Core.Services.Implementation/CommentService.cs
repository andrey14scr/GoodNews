using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Core.Entities;
using GoodNewsAggregator.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GoodNewsAggregator.Core.Services.Implementation
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CommentDto>> GetAll()
        {
            var comments = await _unitOfWork.Comments.GetAll();
            var commentDtos = _mapper.Map<List<CommentDto>>(comments);

            return commentDtos;
        }

        public async Task<CommentDto> GetById(Guid id)
        {
            var comment = await _unitOfWork.Comments.GetById(id);
            var commentDto = _mapper.Map<CommentDto>(comment);

            return commentDto;
        }

        public async Task<IEnumerable<CommentDto>> GetByArticleId(Guid id)
        {
            var comment = await _unitOfWork.Comments.Get().Where(c => c.ArticleId == id).ToListAsync();
            var commentDto = _mapper.Map<List<CommentDto>>(comment);

            return commentDto;
        }

        public async Task Add(CommentDto commentDto)
        {
            await AddRange(new[] { commentDto });
        }

        public async Task AddRange(IEnumerable<CommentDto> commentDtos)
        {
            var comments = _mapper.Map<List<Comment>>(commentDtos.ToList());
            await _unitOfWork.Comments.AddRange(comments);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Update(CommentDto commentDto)
        {
            var comment = _mapper.Map<Comment>(commentDto);
            _unitOfWork.Comments.Update(comment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Remove(CommentDto commentDto)
        {
            await RemoveRange(new[] { commentDto });
        }

        public async Task RemoveRange(IEnumerable<CommentDto> commentDtos)
        {
            var comments = _mapper.Map<List<Comment>>(commentDtos.ToList());
            _unitOfWork.Comments.RemoveRange(comments);
            await _unitOfWork.SaveChangesAsync();
        }

        public IQueryable<Comment> Get()
        {
            return _unitOfWork.Comments.Get();
        }
    }
}
