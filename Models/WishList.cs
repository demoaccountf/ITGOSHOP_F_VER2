using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Models
{
    public class WishList
    {
        private int userId, productId;
        private DateTime creatdAt;

        public WishList()
        {
        }

        public WishList(int userId, int productId, DateTime creatdAt)
        {
            this.userId = userId;
            this.productId = productId;
            this.creatdAt = creatdAt;
        }

        public int UserId { get => userId; set => userId = value; }
        public int ProductId { get => productId; set => productId = value; }
        public DateTime CreatdAt { get => creatdAt; set => creatdAt = value; }
    }
}
