using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using GoodNewsAggregator.Core.DTO;
using GoodNewsAggregator.Core.Services.Interfaces;
using GoodNewsAggregator.DAL.Core;
using GoodNewsAggregator.DAL.Repositories.Interfaces;

namespace GoodNewsAggregator.Core.Services.Implementation
{
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public RoleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleDto>> GetAll()
        {
            var roles = await _unitOfWork.Roles.GetAll();
            var roleDtos = _mapper.Map<List<RoleDto>>(roles);

            return roleDtos;
        }

        public async Task<RoleDto> GetById(Guid id)
        {
            var role = await _unitOfWork.Roles.GetById(id);
            var roleDto = _mapper.Map<RoleDto>(role);

            return roleDto;
        }

        public async Task Add(RoleDto commentDto)
        {
            await AddRange(new[] { commentDto });
        }

        public async Task AddRange(IEnumerable<RoleDto> roleDtos)
        {
            var roles = _mapper.Map<List<Role>>(roleDtos.ToList());
            await _unitOfWork.Roles.AddRange(roles);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Update(RoleDto roleDto)
        {
            var role = _mapper.Map<Role>(roleDto);
            await _unitOfWork.Roles.Update(role);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task Remove(RoleDto roleDto)
        {
            await RemoveRange(new[] { roleDto });
        }

        public async Task RemoveRange(IEnumerable<RoleDto> roleDtos)
        {
            var roles = _mapper.Map<List<Role>>(roleDtos.ToList());
            await _unitOfWork.Roles.RemoveRange(roles);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
