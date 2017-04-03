using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace PP.Core.Model.Report
{
    public class ProjectReportItem : ReportItemBase
    {
        /// <summary>
        /// projektledare
        /// </summary>
        public string ProjectLeader { get; set; }

        /// <summary>
        /// ProjectPlan bakgrund&syfte
        /// </summary>
        public string IntroductionBackground { get; set; }

        /// <summary>
        /// ProjectPlan definitioner
        /// </summary>
        public string IntroductionDefinition { get; set; }

        /// <summary>
        /// ProjectPlan övriga kom
        /// </summary>
        public string IntroductionComments { get; set; }

        /// <summary>
        /// ProjectPlan omfattning
        /// </summary>
        public string DescriptionExtent { get; set; }

        /// <summary>
        /// ProjectPlan avgränsning
        /// </summary>
        public string DescriptionDelimitation { get; set; }

        /// <summary>
        /// ProjectPlan fv&drift
        /// </summary>
        public string DescriptionManagement { get; set; }

        /// <summary>
        /// Implementation_Description genomförande
        /// </summary>
        public string ImplementationDescription { get; set; }

        /// <summary>
        /// Intressentanalys kommunikationsplan
        /// </summary>
        public string CommunicationInterestAnalysis { get; set; }

        /// <summary>
        /// Communication_Definition övr kom
        /// </summary>
        [XmlElement(ElementName = "Kommunikationsdefinition")]
        public string CommunicationDefinition { get; set; }

        /// <summary>
        /// Project Plan Description_Evaluation avslut&utv
        /// </summary>
        [XmlElement(ElementName = "Avslut och utveckling")]
        public string DescriptionEvaluation { get; set; }

    }
}
