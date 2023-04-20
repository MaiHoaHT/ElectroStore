using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectroWeb.Models.EntityFramework
{
    // This class contains common attribute fields 
    public abstract class GeneralDataAbstract
    {
        public DateTime CreateDate { get; set; }
        public DateTime ModifierDate { get; set; }
    }
}