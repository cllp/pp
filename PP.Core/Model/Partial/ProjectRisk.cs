using PP.Core.Attributes;
using PP.Core.Helpers;
using PP.Core.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PP.Core.Model
{
   public partial class ProjectRisk
    {
       [DapperIgnore]
       public int Consequence
       {
           get
           {
               return this.ConsequenceId;
           }
       }
       [DapperIgnore]
       public int Probability
       {
           get
           {
               return this.ProbabilityId;
           }
       }

       [DapperIgnore]
       public int Effect {
           get
           {
                   
                    int result = 0;
                    if (Consequence == 1 && Probability == 1) {
                        result = 1;
                    }
                    if (Consequence == 1 && Probability != 1) {
                        result = Probability;
                    }
                    if (Probability == 1 && Consequence != 1) {
                        result = Consequence;
                    }
                    if (Probability != 1 && Consequence != 1) {
                        result = Probability * Consequence;
                    }
                    return result;
           }
       }
    }
}
