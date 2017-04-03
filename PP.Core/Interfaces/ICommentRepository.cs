using PP.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PP.Core.Interfaces
{
    public interface ICommentRepository
    {
        IEnumerable<ProjectCommentView> GetComments(int projectId);

        IEnumerable<ProjectCommentArea> GetCommentAreas();

        IEnumerable<ProjectCommentType> GetCommentTypes(int projectId);

        ProjectCommentView GetComment(int commentId);

        ProjectCommentView CreateComment(ProjectComment comment);

        void EditComment(int commentId, string text);

        void DeleteComment(int commentId);

        IEnumerable<UnreadProjectComment> GetUnreadProjectComments(int projectId);

        void UpdateProjectCommentActivity(int projectId, int projectCommentAreaId, int projectCommentTypeId);
    }
}
