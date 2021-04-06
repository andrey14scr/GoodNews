using AutoMapper;

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

namespace GoodNewsAggregator.Core.Services.Implementation
{
    public class RssService : IRssService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly Mapper _mapper = new Mapper(new MapperConfiguration(mc => mc.CreateMap<Rss, RssDto>()));

        public RssService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Task<int> Add(RssDto rssDto)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<RssDto>> AddRange(IEnumerable<RssDto> rssDtos)
        {
            throw new NotImplementedException();
        }

        public Task<int> Delete(RssDto rss)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<RssDto>> GetAll()
        {
            var rss = await _unitOfWork.Rss.Get().ToListAsync();
            var rssDtos = _mapper.Map<List<Rss>, List<RssDto>>(rss);

            return rssDtos;

            /*
            return await _unitOfWork.RssSources.FindBy(sourse => !string.IsNullOrEmpty(sourse.Name))
                .Select(sourse => new RssSourseDto()
                {
                    Id = sourse.Id,
                    Name = sourse.Name,
                    Url = sourse.Url
                }).ToListAsync();
            */
        }

        public Task<RssDto> GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<int> Update(RssDto rss)
        {
            throw new NotImplementedException();
        }
    }
}
