
using ProjectManagementTracketAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ProjectManagementTracketAPI.DbContexts;
using ProjectManagementTracketAPI.Models.DTO;
using Microsoft.EntityFrameworkCore;
using ProjectManagementTracketAPI.RabbitMQSender;
using System;
using ProjectManagementTrackerAPI.BL;

namespace ProjectManagementTracketAPI.Repository
{
    public class MemberRepository : IMemberRepository
    {
        private readonly ApplicationDbContexts _db;
        private IMapper _mapper;
        private string _rabbitMQQueueName;
        private string _rabbitMQConstring;
        public MemberRepository(ApplicationDbContexts db, IMapper mapper, string rabbitMQQueueName, string rabbitMQConstring)
        {
            _db = db;
            _mapper = mapper;
            _rabbitMQQueueName = rabbitMQQueueName;
            _rabbitMQConstring = rabbitMQConstring;
        }
        public async Task<IEnumerable<MemberDTO>> GetMemberDetails()
        {
           IEnumerable<Member> membersList =  await _db.Members.OrderByDescending(r=>r.ExperienceInYear).ToListAsync();
            IEnumerable<MemberDTO> memberDTOList = _mapper.Map<IEnumerable<MemberDTO>>(membersList);
            foreach (MemberDTO item in memberDTOList)
            {
                var result = await (from skillTrans in _db.SkillsTransaction
                              join skillMaster in _db.SkillsMaster
                              on skillTrans.SkillId equals skillMaster.SkillId
                              where skillTrans.MemberId == item.MemberId
                              select new
                              {
                                  SkillsId = skillTrans.SkillId,
                                  Skills = skillMaster.SkillName
                              }).ToListAsync();
                List<SkillSetDTO> skillSetDTOList = new List<SkillSetDTO>();
                foreach (var i in result)
                {
                    SkillSetDTO skillDTO = new SkillSetDTO()
                    {
                        SkillId = i.SkillsId,
                        SkillName = i.Skills
                    };
                    skillSetDTOList.Add(skillDTO);
                }               
                item.SkillSet = skillSetDTOList;
                
            }
            return memberDTOList;

        } 
        public async Task<MemberDTO> AddMember(RequestMemberDTO rmemberDTO)
        {

            Member member = _mapper.Map<RequestMemberDTO, Member>(rmemberDTO);
            List<RequestSkillSetDTO> skillsSet = rmemberDTO.SkillSet;
                _db.Members.Add(member);
                foreach (RequestSkillSetDTO item in skillsSet)
                {
                    SkillsTransaction skillsTransaction = new SkillsTransaction
                    {
                        SkillId = item.SkillId,
                        MemberId = member.MemberId

                    };
                    _db.SkillsTransaction.Add(skillsTransaction);
                }              
           
            await _db.SaveChangesAsync();
            return _mapper.Map<Member, MemberDTO>(member);
            
        }
        public async Task<ResponseDTO> AssigningTask(AssigningTaskDTO assigningTaskDTO)
        {
           
           Member member =  _db.Members.Where(r => r.MemberId == assigningTaskDTO.MemberId).FirstOrDefault();
            Validation.ValidateProjectAndTaskEnddate(member.EndDate, assigningTaskDTO.TaskEndDate);
            AssigningTask assigningTask = _mapper.Map<AssigningTaskDTO, AssigningTask>(assigningTaskDTO);
            _db.AssigningTask.Add(assigningTask);
            await _db.SaveChangesAsync();
            ResponseDTO responseDTO = new ResponseDTO()
            {
                IsSuccess = true,
                Message ="Task Assigned Successfully"

            };
            // publish message to rabbitmq queue
            new RabbitMessageSender().PublishMessage(_rabbitMQConstring, _rabbitMQQueueName, assigningTask);
          
            return responseDTO;
        }
        public async Task<ResponseDTO> GetAssigedTask()
        {

          var userName =   Users.Current;
           User user =  _db.User.FirstOrDefault(r => r.UserName == userName);
            AssigningTask assigningTask = new AssigningTask();
            ResponseDTO responseDTO = new ResponseDTO();
            assigningTask =  await _db.AssigningTask.FirstOrDefaultAsync(r => r.MemberId == user.MemberId);
            if (assigningTask != null)
            {
                AssigningTaskDTO assigningDTO = _mapper.Map<AssigningTask, AssigningTaskDTO>(assigningTask);
                responseDTO.Result = assigningDTO;
                responseDTO.IsSuccess = true;
            }
            else
            {
                responseDTO.IsSuccess = false;
                responseDTO.Message = "There is no record found.";
            }
           
            return responseDTO;
            
        }
        public async Task<ResponseDTO> UpdateAllocation(RequestUpdateAllocationDTO requestUpdateAllocationDTO)
        {
            short allocationPercentage;
            Member member = _db.Members.Where(r => r.MemberId == requestUpdateAllocationDTO.MemberId).FirstOrDefault();
            if(member.EndDate > DateTime.Now)
            {
                allocationPercentage = 100;
            }
            else
            {
                allocationPercentage = 0;
            }
            //Member member =  _db.Members.Where(r => r.MemberId == requestUpdateAllocationDTO.MemberId).FirstOrDefault();
            member.AllocationPercentage = allocationPercentage;
           await  _db.SaveChangesAsync();
            ResponseDTO responseDTO = new ResponseDTO()
            {
                IsSuccess = true,
                Message = "Task Assigned updated Successfully"

            };
            return responseDTO;

        }

        
    }
}
