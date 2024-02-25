using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2.Data.Enums;

public enum Gender
{


    [Description("Неизвестно")]
    Unknown = -1,

    [Description("Мужской")]
    Male = 1,

    [Description("Женский")]
    Female = 2,
   
}
