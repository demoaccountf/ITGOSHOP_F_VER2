using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Controllers
{
    public class ShipMethod
    {
        private int shipMethodId;
        private string shipMethodName;
        private int shipFee;
        private DateTime estimatedDeliveryTime;
        private int status;
        private DateTime CreatedAt;

        public ShipMethod()
        {
        }

        public ShipMethod(int shipMethodId, string shipMethodName, int shipFee, DateTime estimatedDeliveryTime, int status, DateTime createdAt)
        {
            this.shipMethodId = shipMethodId;
            this.shipMethodName = shipMethodName;
            this.shipFee = shipFee;
            this.estimatedDeliveryTime = estimatedDeliveryTime;
            this.status = status;
            CreatedAt = createdAt;
        }

        public int ShipMethodId { get => shipMethodId; set => shipMethodId = value; }
        public string ShipMethodName { get => shipMethodName; set => shipMethodName = value; }
        public int ShipFee { get => shipFee; set => shipFee = value; }
        public DateTime EstimatedDeliveryTime { get => estimatedDeliveryTime; set => estimatedDeliveryTime = value; }
        public int Status { get => status; set => status = value; }
        public DateTime CreatedAt1 { get => CreatedAt; set => CreatedAt = value; }
    }
}
