﻿namespace EKE.Data.Entities.Gyopar
{
    public class MagazineTag
    {
        public int MagazinId { get; set; }
        public Magazine Magazin { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
