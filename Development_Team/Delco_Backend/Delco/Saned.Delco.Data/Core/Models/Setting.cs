﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saned.Delco.Data.Core.Models
{
    public class Setting
    {

        public int Id { get; set; }

        public string UnSupportedPathMessage { get; set; }
        public string SuspendAgentMessage { get; set; }
        public decimal MinimumPrice { get; set; }
        public decimal StartPrice { get; set; }
        public decimal KiloMeterPrice { get; set; }
        public decimal MinutePrice { get; set; }
        public decimal ManagementPercentage { get; set; }
        public string AbuseEmail { get; set; }
        public string ContactUsEmail { get; set; }
        public string TermsOfConditions { get; set; }



    }
}