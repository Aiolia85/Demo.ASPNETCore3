using Entity;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Reoository.EF
{
    public class UserInfoRepository : BaseRepository<UserInfo>, IUserInfoRepository
    {
        public UserInfoRepository(SQLiteDbContext context) : base(context)
        { }
    }
}
