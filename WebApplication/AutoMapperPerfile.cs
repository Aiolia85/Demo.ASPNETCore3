using AutoMapper;
using Entity;
using Entity.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication
{
    public class AutoMapperPerfile:Profile
    {
        public AutoMapperPerfile()
        {
            CreateMap<UserInfo, UserDTO>().ForMember(des => des.DisplayName, mem => mem.MapFrom(c => c.Username));
        }
    }
}
