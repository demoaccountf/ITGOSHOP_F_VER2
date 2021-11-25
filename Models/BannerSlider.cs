using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ITGoShop_F_Ver2.Controllers
{
    public class BannerSlider
    {
        private int sliderId;
        private string sliderName, sliderImage;
        private int sliderStatus, blogId;
        private DateTime createdAt, updatedAt;
        private int campaignId, campaignDiscount, saleQuantity;

        public BannerSlider()
        {
        }

        public BannerSlider(int sliderId, string sliderName, string sliderImage, int sliderStatus, int blogId, DateTime createdAt, DateTime updatedAt, int campaignId, int campaignDiscount, int saleQuantity)
        {
            this.sliderId = sliderId;
            this.sliderName = sliderName;
            this.sliderImage = sliderImage;
            this.sliderStatus = sliderStatus;
            this.blogId = blogId;
            this.createdAt = createdAt;
            this.updatedAt = updatedAt;
            this.campaignId = campaignId;
            this.campaignDiscount = campaignDiscount;
            this.saleQuantity = saleQuantity;
        }

        public int SliderId { get => sliderId; set => sliderId = value; }
        public string SliderName { get => sliderName; set => sliderName = value; }
        public string SliderImage { get => sliderImage; set => sliderImage = value; }
        public int SliderStatus { get => sliderStatus; set => sliderStatus = value; }
        public int BlogId { get => blogId; set => blogId = value; }
        public DateTime CreatedAt { get => createdAt; set => createdAt = value; }
        public DateTime UpdatedAt { get => updatedAt; set => updatedAt = value; }
        public int CampaignId { get => campaignId; set => campaignId = value; }
        public int CampaignDiscount { get => campaignDiscount; set => campaignDiscount = value; }
        public int SaleQuantity { get => saleQuantity; set => saleQuantity = value; }
    }
}
