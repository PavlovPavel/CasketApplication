using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;

namespace CasketApplication.Models
{
    public class SiteDateTimeIntervalModels
    {
        /// <summary>
        /// Идентификатор интервала
        /// </summary>
        public Guid Identifier { get; set; }

        /// <summary>
        /// Даты начала планируемых работ
        /// </summary>
        [DisplayName("Даты начала планируемых работ")]
        public DateTime BeginTime { get; set; }

        /// <summary>
        /// Даты окончания планируемых работ
        /// </summary>
        [DisplayName("Даты окончания планируемых работ")]
        public DateTime EndTime { get; set; }
    }
}