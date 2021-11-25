﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Controllers
{
    public class LoginHistory
    {
        private int loginId, userId;
        private DateTime loginDate, loginTime;

        public LoginHistory()
        {
        }

        public LoginHistory(int loginId, int userId, DateTime loginDate, DateTime loginTime)
        {
            this.loginId = loginId;
            this.userId = userId;
            this.loginDate = loginDate;
            this.loginTime = loginTime;
        }

        public int LoginId { get => loginId; set => loginId = value; }
        public int UserId { get => userId; set => userId = value; }
        public DateTime LoginDate { get => loginDate; set => loginDate = value; }
        public DateTime LoginTime { get => loginTime; set => loginTime = value; }
    }
}
