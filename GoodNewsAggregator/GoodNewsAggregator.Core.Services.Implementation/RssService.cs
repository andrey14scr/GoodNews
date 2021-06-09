using AutoMapper;
using AutoMapper.Configuration;

using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Repositories.Interfaces;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoodNewsAggregator.Core.Services.Implementation.Parsers;
using GoodNewsAggregator.DAL.Core.Entities;

namespace GoodNewsAggregator.Core.Services.Implementation
{
    public class RssService : IRssService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RssService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task Add(RssDto commentDto)
        {
            await AddRange(new[] { commentDto });
        }

        public async Task AddRange(IEnumerable<RssDto> rssDtos)
        {
            var rss = _mapper.Map<List<Rss>>(rssDtos);
            await _unitOfWork.Rss.AddRange(rss);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Remove(RssDto rssDto)
        {
            await RemoveRange(new[] { rssDto });
        }

        public async Task RemoveRange(IEnumerable<RssDto> rssDtos)
        {
            _unitOfWork.Rss.RemoveRange(_mapper.Map<List<Rss>>(rssDtos));
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<RssDto>> GetAll()
        {
            var rss = await _unitOfWork.Rss.GetAll();
            var rssDtos = _mapper.Map<List<RssDto>>(rss);

            return rssDtos;
        }

        public async Task<RssDto> GetById(Guid id)
        {
            var rss = await _unitOfWork.Rss.GetById(id);
            var rssDto = _mapper.Map<RssDto>(rss);

            return rssDto;
        }

        public async Task Update(RssDto rssDto)
        {
            var rss = _mapper.Map<Rss>(rssDto);
            _unitOfWork.Rss.Update(rss);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
