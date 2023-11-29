using SpaceShipAPI.DTO;
using SpaceShipAPI.Model.Mission;
using SpaceShipAPI.Model.Ship;
using SpaceShipAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using SpaceShipAPI.Model.DTO.Mission;

namespace SpaceShipAPI.Services
{
    public class MissionService
    {
        private readonly IMissionRepository _missionRepository;
        private readonly MissionFactory _missionFactory;
        private readonly ISpaceShipRepository _spaceShipRepository;
        private readonly ILocationRepository _locationRepository;

        public MissionService(
            IMissionRepository missionRepository, 
            MissionFactory missionFactory, 
            ISpaceShipRepository spaceShipRepository,
            ILocationRepository locationRepository)
        {
            _missionRepository = missionRepository;
            _missionFactory = missionFactory;
            _spaceShipRepository = spaceShipRepository;
            _locationRepository = locationRepository;
        }

        public async Task<IEnumerable<MissionDTO>> GetAllActiveMissionsForCurrentUserAsync(ClaimsPrincipal user)
        {
            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var missions = await _missionRepository.GetMissionsByUserIdAndCurrentStatusNotAsync(userId, MissionStatus.ARCHIVED);
            return missions.Select(m => UpdateAndConvert(m));
        }

        // Other methods like GetAllActiveMissionsByUserIdAsync, GetAllArchivedMissionsForCurrentUserAsync, etc.

        public async Task<MissionDetailDTO> GetMissionByIdAsync(long id, ClaimsPrincipal user)
        {
            var mission = await GetMissionByIdAndCheckAccessAsync(id, user);
            var missionManager = _missionFactory.GetMissionManager(mission);
            if ( missionManager.UpdateStatus())
            {
                await _missionRepository.UpdateAsync(mission);
            }
            return missionManager.GetDetailedDTO();
        }


        private MissionDTO UpdateAndConvert(Mission mission)
        {
            var missionManager = _missionFactory.GetMissionManager(mission);
            if (missionManager.UpdateStatus())
            {
                _missionRepository.UpdateAsync(mission);
            }
            return new MissionDTO(mission);
        }
        
        private Mission Update(Mission mission)
        {
            var missionManager = _missionFactory.GetMissionManager(mission);
            _missionRepository.UpdateAsync(mission);
            return mission;
        }

        private async Task<Mission> GetMissionByIdAndCheckAccessAsync(long id, ClaimsPrincipal user)
        {
            var mission = await _missionRepository.GetByIdAsync(id);
            if (mission == null)
            {
                throw new KeyNotFoundException($"No mission found with id {id}");
            }

            var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (user.IsInRole("Admin") || mission.UserId == userId)
            {
                return mission;
            }
            else
            {
                throw new UnauthorizedAccessException("You don't have authority to access this mission");
            }
        }
    }
}
