using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Controllers
{
    public class devvn_quanhuyen
    {
        private string maqh;
        private string name, type, matp;

        public devvn_quanhuyen()
        {
        }

        public devvn_quanhuyen(string maqh, string name, string type, string matp)
        {
            this.maqh = maqh;
            this.name = name;
            this.type = type;
            this.matp = matp;
        }

        [Key]
        public string Maqh { get => maqh; set => maqh = value; }
        public string Name { get => name; set => name = value; }
        public string Type { get => type; set => type = value; }
        public string Matp { get => matp; set => matp = value; }
    }
}
