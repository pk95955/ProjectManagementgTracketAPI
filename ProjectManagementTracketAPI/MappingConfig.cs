using AutoMapper;
using ProjectManagementTracketAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementTracketAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
             {
                 config.CreateMap<MemberDTO, Member>();
                 config.CreateMap<Member, MemberDTO> ();
                 config.CreateMap<AssigningTaskDTO, AssigningTask> ();
                 config.CreateMap<AssigningTask, AssigningTaskDTO> ();
             });
            return mappingConfig;


        }
    }
}
