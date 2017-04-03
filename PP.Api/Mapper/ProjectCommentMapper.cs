using PP.Api.Models;
using PP.Core.Context;
using PP.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PP.Core.Helpers;

namespace PP.Api.Mapper
{
    public static class ProjectCommentMapper
    {
        public static ProjectCommentViewModel MapProjectComment(ProjectCommentView projectComment)
        {
            var model = new ProjectCommentViewModel();
            model.Id = projectComment.Id;
            model.AreaId = projectComment.AreaId;
            model.Area = projectComment.Area;
            model.TypeId = projectComment.TypeId;
            model.Type = projectComment.Type;
            model.ProjectId = projectComment.ProjectId;
            model.Date = projectComment.Date.GetMyLocalTime();
            model.Text = projectComment.Text;
            model.Writer = projectComment.Writer;
            model.MyComment = projectComment.UserId == IdentityContext.Current.User.Id;
            return model;
        }

        public static ProjectCommentAreaModel MapProjectCommentArea(ProjectCommentArea projectCommentArea, int index)
        {
            var model = new ProjectCommentAreaModel();
            model.AreaId = projectCommentArea.Id;
            model.Area = projectCommentArea.Name;
            model.Index = index;
            return model;
        }

        public static ProjectComment MapProjectCommentModel(ProjectCommentModel model)
        {
            var coreModel = new ProjectComment();
            coreModel.Id = model.Id;
            coreModel.ProjectCommentAreaId = model.ProjectCommentAreaId;
            coreModel.ProjectCommentTypeId = model.ProjectCommentTypeId;
            coreModel.ProjectId = model.ProjectId;
            coreModel.Date = model.Date.GetMyLocalTime();
            coreModel.Text = model.Text;

            return coreModel;
        }

        public static IEnumerable<ProjectCommentTypeModel> MapProjectCommentTypeModels(IEnumerable<ProjectCommentType> coreModels)
        {
            int index = 1;

            List<ProjectCommentTypeModel> models = new List<ProjectCommentTypeModel>();

            foreach(var coreModel in coreModels.OrderBy(m => m.Id))
            {
                var model = new ProjectCommentTypeModel();
                model.Index = index;
                model.Name = coreModel.Name;
                model.TypeId = coreModel.Id;

                models.Add(model);
                index++;
            }

            return models.AsEnumerable().OrderBy(m => m.Index);
        }
    }
}