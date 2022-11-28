
using ProjectManagementTracketAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProjectManagementTracketAPI.DbContexts;
using ProjectManagementTracketAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace ProjectManagementTracketAPI.Repository
{
    public class MemberRepository : IMemberRepository
    {
        private readonly ApplicationDbContexts _db;
        private IMapper _mapper;
        public MemberRepository(ApplicationDbContexts db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }


        public async Task<IEnumerable<MemberDTO>> GetMemberDetails()
        {
           IEnumerable<Member> membersList =  await _db.Members.OrderByDescending(r=>r.ExperienceInYear).ToListAsync();
            IEnumerable<MemberDTO> memberDTOList = _mapper.Map<IEnumerable<MemberDTO>>(membersList);
            foreach (MemberDTO item in memberDTOList)
            {
                //List<SkillsTransaction> skillSetList =   await _db.SkillsTransaction.Where(r => r.MemberId == item.MemberId).ToListAsync();
                var result = await (from skillTrans in _db.SkillsTransaction
                              join skillMaster in _db.SkillsMaster
                              on skillTrans.SkillId equals skillMaster.SkillId
                              where skillTrans.MemberId == item.MemberId
                              select new
                              {
                                  SkillId = skillTrans.SkillId,
                                  SkillName = skillMaster.SkillName
                              }).ToListAsync();
                List<SkillSetDTO> skillSetDTOList = new List<SkillSetDTO>();
                foreach (var i in result)
                {
                    SkillSetDTO skillDTO = new SkillSetDTO()
                    {
                        SkillId = i.SkillId,
                        SkillName = i.SkillName
                    };
                    skillSetDTOList.Add(skillDTO);

                }
               // List<SkillSetDTO> skillSetDTOList = result;// _mapper.Map<List<SkillSetDTO>>(skillSetList);
               //if (skillSetList.Any())
               //{
                item.SkillSet = skillSetDTOList;
               //}
            }
            return memberDTOList;

        } 
        public async Task<MemberDTO> AddMember(MemberDTO memberDTO)
        {
            Member member = _mapper.Map<MemberDTO, Member>(memberDTO);
            List<SkillSetDTO> skillsSet = memberDTO.SkillSet;

            if (member.Id > 0)
            {
                _db.Members.Update(member);
             
            }
            else
            {
                _db.Members.Add(member);
                foreach (SkillSetDTO item in skillsSet)
                {
                    SkillsTransaction skillsTransaction = new SkillsTransaction
                    {
                        SkillId = item.SkillId,
                        MemberId = member.MemberId

                    };
                    _db.SkillsTransaction.Add(skillsTransaction);

                }
               
            }
            await _db.SaveChangesAsync();
            return _mapper.Map<Member, MemberDTO>(member);
            
        }

        public async Task<ResponseDTO> AssigningTask(AssigningTaskDTO assigningTaskDTO)
        {
            AssigningTask assigningTask = _mapper.Map<AssigningTaskDTO, AssigningTask>(assigningTaskDTO);
            _db.AssigningTask.Add(assigningTask);
            await _db.SaveChangesAsync();
            ResponseDTO responseDTO = new ResponseDTO()
            {
                IsSuccess = true,
                Message ="Task Assigned Successfully"

            };
            return responseDTO;
        }
        public async Task<AssigningTaskDTO> GetAssigedTask(int MemberId)
        {
            AssigningTask assigningTask = new AssigningTask();
            assigningTask =  await _db.AssigningTask.FirstOrDefaultAsync(r => r.MemberId == MemberId);
            AssigningTaskDTO assigningDTO=   _mapper.Map<AssigningTask, AssigningTaskDTO>(assigningTask);
            return assigningDTO;
            
        }
        public async Task<ResponseDTO> UpdateAllocation(RequestUpdateAllocationDTO requestUpdateAllocationDTO)
        {
          Member member =  _db.Members.Where(r => r.MemberId == requestUpdateAllocationDTO.MemberId).FirstOrDefault();
            member.AllocationPercentage = requestUpdateAllocationDTO.AllocationPercentage;
           await  _db.SaveChangesAsync();
            ResponseDTO responseDTO = new ResponseDTO()
            {
                IsSuccess = true,
                Message = "Task Assigned Successfully"

            };
            return responseDTO;

        }

        
    }
}
