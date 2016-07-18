using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace CasketApplication.Models
{

    public class SiteStatusModels
    {
        [DisplayName("Признак активности сервера")]
        /// <summary>
        /// Состояние сервера (true - работает, false не работает)
        /// </summary>
        public bool Work { get; set; }

        [DisplayName("Даты планируемых работ")]
        /// <summary>
        /// Даты планируемых работ
        /// </summary>
        public List<SiteDateTimeIntervalModels> PlanWorkTime { get; set; }
    }
}