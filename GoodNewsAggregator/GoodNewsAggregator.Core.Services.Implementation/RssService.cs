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

        private readonly List<(IWebPageParser Parser, Guid Id)> _parsers = new List<(IWebPageParser parser, Guid id)>()
        {
            //(new TutbyParser(), new Guid("5932E5D6-AFE4-44BF-AFD7-8BC808D66A61")),
            (new OnlinerParser(), new Guid("7EE20FB5-B62A-4DF0-A34E-2DC738D87CDE")),
            (new TjournalParser(), new Guid("95AC927C-4BA7-43E8-B408-D3B1F4C4164F")),
            //(new S13Parser(), new Guid("EC7101DA-B135-4035-ACFE-F48F1970B4CB")),
            (new DtfParser(), new Guid("5707D1F0-6A5C-46FB-ACEC-0288962CB53F")),
        };

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
            await _unitOfWork.Rss.RemoveRange(_mapper.Map<List<Rss>>(rssDtos));
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<RssDto>> GetAll()
        {
            var rss = await _unitOfWork.Rss.GetAll();
            rss = rss.Where(r => _parsers.Exists(p => p.Id == r.Id));
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
            await _unitOfWork.Rss.Update(rss);
            await _unitOfWork.SaveChangesAsync();
        }

        public IWebPageParser GetParserById(Guid id)
        {
            return _parsers.FirstOrDefault(p => p.Id == id).Parser;
        }
    }
}
