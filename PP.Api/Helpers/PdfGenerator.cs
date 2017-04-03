using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using PP.Core.Helpers;
using PP.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using PP.Core.Logic;

namespace PP.Api.Helpers
{
    public static class PdfGenerator
    {
        public static void GeneratePdf(Project project, string author, int versionId, bool draft, IEnumerable<ProjectRole> projectRoles)
        {
            ITextReplace replace;


            replace = new TextReplace();
            replace.Add("%Startdate%", project.StartDate.HasValue ? project.StartDate.Value.ToString("yyyy-MM-dd") : "");
            replace.Add("%Description%", project.Description);
            string projectidea = replace.ReplaceFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Templates\Project\", "projectidea.html"));

            replace = new TextReplace();
            replace.Add("%estimated%", project.FundingEstimate.ToString());
            replace.Add("%own%", project.FundingActual.ToString());
            replace.Add("%external%", project.FundingExternal.ToString());
            replace.Add("%stimula%", project.FundingStimulus.ToString());
            replace.Add("%notfinanced%", (project.FundingEstimate - (project.FundingExternal + project.FundingStimulus)).ToString());
            string finance = replace.ReplaceFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Templates\Project\", "finance.html"));

            //MEMBERS
            var memberString = string.Empty;


            //foreach role 
            foreach (var role in projectRoles)
            {

                foreach (var member in project.ProjectMembers)
                {
                    if (member.ProjectRoles.Find(r => r.ProjectRoleId.Equals(role.Id)) != null)
                    {
                        memberString += string.Format("<tr><td class=\"text\" colspan=\"3\"><b>{0}</b></td></tr>", role.Name.Trim());

                        memberString += string.Format("<tr><td class=\"text\">{0}</td><td class=\"text\">{1}</td><td class=\"text\">{2}</td></tr>", member.UserName.Trim(), member.UserEmail.Trim(), member.Organization.Trim());
                        break;
                    }
                }

                //var projectRoleMembers = project.ProjectMembers.Where(r => r.RoleId.Value.Equals(role.Id));

                //if (projectRoleMembers.Count() > 0)
                //{
                //    memberString += string.Format("<tr><td class=\"text\" colspan=\"3\"><b>{0}</b></td></tr>", role.Name.Trim());

                //    foreach (var member in projectRoleMembers)
                //    {
                //        memberString += string.Format("<tr><td class=\"text\">{0}</td><td class=\"text\">{1}</td><td class=\"text\">{2}</td></tr>", member.UserName.Trim(), member.UserEmail.Trim(), member.Organization.Trim());
                //    }
                //}
            }

            replace = new TextReplace();
            replace.Add("%members%", memberString.ToString());
            string members = replace.ReplaceFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Templates\Project\", "members.html"));

            //PLAN
            replace = new TextReplace();

            //description
            replace.Add("%PlanDescriptionDelimitation%", project.PlanDescriptionDelimitation != null ? project.PlanDescriptionDelimitation : "");
            replace.Add("%PlanDescriptionEvaluation%", project.PlanDescriptionEvaluation != null ? project.PlanDescriptionEvaluation : "");
            replace.Add("%PlanDescriptionExtent%", project.PlanDescriptionExtent != null ? project.PlanDescriptionExtent : "");
            replace.Add("%PlanDescriptionManagement%", project.PlanDescriptionManagement != null ? project.PlanDescriptionManagement : "");
            //communication
            replace.Add("%PlanCommunicationDefinition%", project.PlanCommunicationDefinition != null ? project.PlanCommunicationDefinition : "");
            replace.Add("%PlanCommunicationInterestAnalysis%", project.PlanCommunicationInterestAnalysis != null ? project.PlanCommunicationInterestAnalysis : "");
            replace.Add("%PlanCommunicationPlan%", project.PlanCommunicationPlan != null ? project.PlanCommunicationPlan : "");
            //implementation
            replace.Add("%PlanImplementationDescription%", project.PlanImplementationDescription != null ? project.PlanImplementationDescription : "");
            //introduction
            replace.Add("%PlanIntroductionBackground%", project.PlanIntroductionBackground != null ? project.PlanIntroductionBackground : "");
            replace.Add("%PlanIntroductionComments%", project.PlanIntroductionComments != null ? project.PlanIntroductionComments : "");
            replace.Add("%PlanIntroductionDefinition%", project.PlanIntroductionDefinition != null ? project.PlanIntroductionDefinition : "");
            string plan = replace.ReplaceFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Templates\Project\", "plan.html"));

            //GOAL
            var goalString = string.Empty;
            replace = new TextReplace();

            if (project.ProjectGoals.Count() > 0)
                goalString += "<table>";

            foreach (var projectgoal in project.ProjectGoals)
            {
                goalString += string.Format("<tr><td class=\"label\" style=\"width:100px;\">{0}</td><td class=\"label\">{1}</td></tr>", "Måltyp", "Uppnått");
                goalString += string.Format("<tr><td class=\"text\">{0}</td><td class=\"text\">{1}</td></tr>", projectgoal.Type.ToString(), projectgoal.Achieved == 1 ? "Ja" : "Nej");
                goalString += string.Format("<tr><td>{0}</td><td class=\"label\">{1}</td></tr>", "&nbsp;", "Målformulering");
                goalString += string.Format("<tr><td>{0}</td><td class=\"text\">{1}</td></tr>", "&nbsp;", projectgoal.GoalDefinition != null ? projectgoal.GoalDefinition : "&nbsp;");
                goalString += string.Format("<tr><td>{0}</td><td class=\"label\">{1}</td></tr>", "&nbsp;", "Mätmetod");
                goalString += string.Format("<tr><td>{0}</td><td class=\"text\">{1}</td></tr>", "&nbsp;", projectgoal.MesaureMethod != null ? projectgoal.MesaureMethod : "&nbsp;");
            }

            if (project.ProjectGoals.Count() > 0)
                goalString += "</table>";

            replace.Add("%goal%", goalString);
            string goal = replace.ReplaceFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Templates\Project\", "goal.html"));

            //ACTIVITY
            replace = new TextReplace();


            string activityRows = string.Empty;

            var activityInternalHoursSum = 0;
            var activityExternalHoursSum = 0;
            var activityOtherCost = 0;
            decimal activitySum = 0;

            foreach (var activityItem in project.ProjectActivities)
            {
                decimal sum = ProjectCalculationLogic.CalculateActivityRow(activityItem);

                activityInternalHoursSum = activityInternalHoursSum + activityItem.InternalHours;
                activityExternalHoursSum = activityInternalHoursSum + activityItem.ExternalHours;
                activityOtherCost = activityOtherCost + activityItem.Cost;
                activitySum = activitySum + sum;

                activityRows += string.Format(@"
                            <tr>
                                <td class='text'>{0}</td>
                                <td class='text'>{1}</td>
                                <td class='text'>{2}</td>
                                <td class='text'>{3}</td>
                                <td class='text'>{4}</td>
                                <td class='text'>{5}</td>
                            </tr>", activityItem.Name, activityItem.Status.ToString(), activityItem.InternalHours.ToString(), activityItem.ExternalHours.ToString(), activityItem.Cost.ToString(), sum.ToString());

                if (!string.IsNullOrEmpty(activityItem.Notes))
                {
                    activityRows += string.Format(@"
                            <tr>
                                <td>&nbsp;</td>
                                <td class='text' colspan='4'>{0}</td>
                                <td>&nbsp;</td>
                            </tr>", activityItem.Notes);
                }
            }

            replace.Add("%activityRows%", activityRows);
            replace.Add("%SummaryInternalHours%", activityInternalHoursSum.ToString());
            replace.Add("%SummaryExternalHours%", activityExternalHoursSum.ToString());
            replace.Add("%SummaryCost%", activityOtherCost.ToString());
            replace.Add("%SummaryTotal%", activitySum.ToString());

            string activity = replace.ReplaceFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Templates\Project\", "activity.html"));

            //FOLLOWUP
            replace = new TextReplace();
            string projectFollowUpRows = string.Empty;
            project.ProjectFollowUps = ProjectCalculationLogic.CalculateFollowUpRow(project.ProjectFollowUps).ToList();
            foreach (var followUpItem in project.ProjectFollowUps)
            {

                var cssClass = "text";

                if (followUpItem.Canceled)
                    cssClass = "text-cancelled";

                projectFollowUpRows += string.Format(@"
                            <tr>
                                <td class='{6}'>{0}</td>
                                <td class='{7}'>{1}</td>
                                <td class='{8}'>{2}</td>
                                <td class='{9}'>{3}</td>
                                <td class='{10}'>{4}</td>
                                <td class='{11}'>{5}</td>
                            </tr>",
                                  followUpItem.Date.HasValue ? followUpItem.Date.Value.ToString("yyyy-MM-dd HH:mm") : "",
                                  followUpItem.InternalHours.ToString(),
                                  followUpItem.ExternalHours.ToString(),
                                  followUpItem.OtherCosts.ToString(),
                                  followUpItem.ActivityTotal.HasValue ? followUpItem.ActivityTotal.Value.ToString() : "",
                                  (followUpItem.Balance == 0) ? "Makulerad" : followUpItem.Balance.ToString(),
                                  cssClass,
                                  cssClass,
                                  cssClass,
                                  cssClass,
                                  cssClass,
                                  cssClass);

                projectFollowUpRows += string.Format(@"<tr><td class='label' colspan='6'>{0}</td></tr>", "Kommentar");
                projectFollowUpRows += string.Format(@"<tr><td class='text' colspan='6'>{0}</td></tr>", followUpItem.Notes != null ? followUpItem.Notes : "");
            }

            replace.Add("%ProjectFollowUps%", projectFollowUpRows);

            string followup = replace.ReplaceFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Templates\Project\", "followup.html"));

            //DEBRIEFING
            string debriefing = string.Empty;
            if (!string.IsNullOrEmpty(project.Debriefing))
            {
                replace = new TextReplace();
                replace.Add("%Comment%", project.Debriefing != null ? project.Debriefing : "");
                debriefing = replace.ReplaceFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Templates\Project\", "debriefing.html"));
            }

            //use a template and replace content
            replace = new TextReplace();
            replace.Add("%Name%", project.Name);
            replace.Add("%Program%", project.Program.Name);
            replace.Add("%Area%", project.ProjectArea != null ? project.ProjectArea.Name : "-");
            replace.Add("%Organization%", project.Organization != null ? project.Organization.Name : "-");
            replace.Add("%Phase%", project.Phase.ToString()); //todo: get the description from enum
            replace.Add("%CreatedBy%", project.CreatedBy != null ? project.CreatedBy.Name : "-");
            replace.Add("%Published%", project.PublishedDate.ToString("yyyy-MM-dd HH:mm"));

            //areas
            replace.Add("%projectidea%", projectidea);
            replace.Add("%finance%", finance);
            replace.Add("%members%", members);
            replace.Add("%plan%", plan);
            replace.Add("%goal%", goal);
            replace.Add("%activity%", activity);
            replace.Add("%followup%", followup);
            replace.Add("%debriefing%", debriefing);

            replace.Add("%url%", "http://projektplaneraren.se");
            string htmlcontent = replace.ReplaceFromFile(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "Project.html"));

            htmlcontent = htmlcontent.Replace("\r\n", "");
            htmlcontent = htmlcontent.Replace("\0", "");

            //htmlcontent = Regex.Replace(htmlcontent,
            //@"\s*(?<capture><(?<markUp>\w+)>.*<\/\k<markUp>>)\s*", "${capture}", RegexOptions.Singleline);


            HtmlToPdf(htmlcontent, project.Name, author, project.Id, versionId, draft);

            //// step 3: we parse the document
            ////HtmlParser.Parse(document, @"c:\temp\test.html");
            //HtmlParser.Parse(document, content);
        }

        private static void HtmlToPdf(string HTML, string title, string author, int projectId, int versionId, bool draft)
        {
            string fileName = projectId.ToString() + "." + versionId.ToString();

            Document document = new Document(PageSize.A4);
            document.AddAuthor(author);
            document.AddCreationDate();
            document.AddCreator(author);
            document.AddTitle(title);
            PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(EnvironmentHelper.PdfDir + fileName + ".pdf", FileMode.Create));
            document.Open();

            if (draft)
            {
                //Draft image background
                string imageFilePath = EnvironmentHelper.ImgDir + "watermark.png";
                iTextSharp.text.Image jpg = iTextSharp.text.Image.GetInstance(imageFilePath);
                //Resize image
                //For give the size to image
                jpg.ScaleToFit(3000, 770);

                //Set image as background
                jpg.Alignment = iTextSharp.text.Image.UNDERLYING;
                //absolute/specified fix position to image.
                jpg.SetAbsolutePosition(7, 69);
                //add the draft image
                document.Add(jpg);
            }

            StringReader sr = new StringReader(HTML);
            XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, sr);

            //to add a new page
            //document.NewPage();
            //XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, new StringReader("<b>New Page</b>"));

            document.Close();
        }
    }
}