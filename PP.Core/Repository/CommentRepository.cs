using PP.Core.Helpers;
using PP.Core.Interfaces;
using PP.Core.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using PP.Core.Context;
using System.Data;

namespace PP.Core.Repository
{
    public class CommentRepository : ICommentRepository
    {
        public IEnumerable<ProjectCommentView> GetComments(int projectId)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                return conn.Query<ProjectCommentView>("SELECT * FROM [ProjectCommentView] WHERE ProjectId = @ProjectId", new { ProjectId = projectId });
            }
        }

        public IEnumerable<ProjectCommentArea> GetCommentAreas()
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                return conn.Query<ProjectCommentArea>("SELECT * FROM [ProjectCommentArea]");
            }
        }

        public ProjectCommentView GetComment(int commentId)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                return conn.Query<ProjectCommentView>("SELECT * FROM [ProjectCommentView] WHERE Id = @Id", new { Id = commentId }).FirstOrDefault();
            }
        }

        //public int CreateComment(ProjectComment comment)
        //{
        //    using (SqlConnection conn = SqlHelper.GetOpenConnection())
        //    {
        //        var par = new DynamicParameters();
        //        par.Add("@ProjectId", comment.ProjectId);
        //        par.Add("@UserId", IdentityContext.Current.User.Id);
        //        par.Add("@ProjectCommentTypeId", comment.ProjectCommentTypeId);
        //        par.Add("@ProjectCommentAreaId", comment.ProjectCommentAreaId);
        //        par.Add("@Text", comment.Text);

        //        conn.Execute("INSERT INTO [ProjectComment] (ProjectId, UserId, ProjectCommentTypeId, ProjectCommentAreaId, [Text]) VALUES (@ProjectId, @UserId, @ProjectCommentTypeId, @ProjectCommentAreaId, @Text)", par);

        //        var commentId = -1;
        //        SqlHelper.SetIdentity<int>(conn, id => commentId = id);

        //        return commentId;
        //    }
        //}

        public void EditComment(int commentId, string text)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                conn.Query<ProjectComment>("UPDATE [ProjectComment] SET Text = @Text WHERE Id = @Id", new { Id = commentId });
            }
        }

        public void DeleteComment(int commentId)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                conn.Execute("DELETE FROM [ProjectComment] WHERE Id = @Id", new { Id = commentId });
            }
        }

        public IEnumerable<UnreadProjectComment> GetUnreadProjectComments(int projectId)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                var par = new DynamicParameters();
                par.Add("@ProjectId", projectId);
                par.Add("@UserId", IdentityContext.Current.User.Id);
                return conn.Query<UnreadProjectComment>("[GetUnreadProjectComments]", par, commandType: CommandType.StoredProcedure);
            }
        }

        public void UpdateProjectCommentActivity(int projectId, int projectCommentAreaId, int projectCommentTypeId)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                var par = new DynamicParameters();
                par.Add("@ProjectId", projectId);
                par.Add("@UserId", IdentityContext.Current.User.Id);
                par.Add("@ProjectCommentAreaId", projectCommentAreaId);
                par.Add("@ProjectCommentTypeId", projectCommentTypeId);
                conn.Execute("[UpdateProjectCommentActivity]", par, commandType: CommandType.StoredProcedure);
            }
        }

        public ProjectCommentView CreateComment(ProjectComment comment)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                var par = new DynamicParameters();
                par.Add("@ProjectId", comment.ProjectId);
                par.Add("@UserId", IdentityContext.Current.User.Id);
                par.Add("@ProjectCommentAreaId", comment.ProjectCommentAreaId);
                par.Add("@ProjectCommentTypeId", comment.ProjectCommentTypeId);
                par.Add("@Text", comment.Text);
                return conn.Query<ProjectCommentView>("[CreateProjectComment]", par, commandType: CommandType.StoredProcedure).FirstOrDefault();
            }
        }


        public IEnumerable<ProjectCommentType> GetCommentTypes(int projectId)
        {
            using (SqlConnection conn = SqlHelper.GetOpenConnection())
            {
                var par = new DynamicParameters();
                par.Add("@ProjectId", projectId);
                par.Add("@UserId", IdentityContext.Current.User.Id);
                return conn.Query<ProjectCommentType>("[GetProjectCommentTypes]", par, commandType: CommandType.StoredProcedure);
            }
        }
    }
}
