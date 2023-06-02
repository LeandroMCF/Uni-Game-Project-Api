using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Uni.Project.WebApi.Context;
using Uni.Project.WebApi.Domain;
using Uni.Project.WebApi.Interface;
using Uni.Project.WebApi.Migrations;

namespace Uni.Project.WebApi.Repository
{
    public class EvaluationRepository : IEvaluationInterface
    {
        UniContext ctx = new UniContext();

        public void AddEvaluation(EvaluationDomain evaluation)
        {
            EvaluationDomain evaluationCheck = ctx.Evaluations.FirstOrDefault(x => x.UserId == evaluation.UserId);
            if (evaluationCheck == null)
            {
                var user = ctx.Users.FirstOrDefault(x => x.UserId == evaluation.UserId);

                evaluation.UserIdNavigation = user;
                ctx.Evaluations.Add(evaluation);
                ctx.SaveChanges();
            }
        }

        public string AlloyAccess(string IdUser)
        {
            UserDomain user = ctx.Users.FirstOrDefault(x => x.UserId == new Guid(IdUser));

            if (user.Download != true)
            {
                return "notAllow-noDownload";
            }
            else
            {
                EvaluationDomain evaluation = ctx.Evaluations.FirstOrDefault(x => x.UserId.ToString() == IdUser);

                if (evaluation != null)
                {
                    return "notAllow-AllRdEv";
                }
                else
                {
                    return "Allow";
                }
            }
        }

        public string GetAllEvaluations(string userId)
        {
            var evaluations = ctx.Evaluations
                .Where(x => x.UserId != new Guid(userId))
                .Include(x => x.UserIdNavigation)
                .ToList();

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            };

            var json = JsonConvert.SerializeObject(evaluations, settings);
            return json;
        }

        public EvaluationDomain GetEvaluation(string idUser)
        {
            return ctx.Evaluations.FirstOrDefault(x => x.UserId.ToString() == idUser);
        }

        public void RemoveEvaluation(string IdUser)
        {
            EvaluationDomain evaluation = ctx.Evaluations.FirstOrDefault(x => x.UserId.ToString() == IdUser);
            ctx.Evaluations.Remove(evaluation);
            ctx.SaveChanges();
        }

        public void UpdateEvaluation(string IdEvaluation, string IdUser, EvaluationDomain evaluation)
        {
            EvaluationDomain evaluationToUpdate = ctx.Evaluations.FirstOrDefault(x => x.UserId.ToString() == IdUser && x.EvaluationId.ToString() == IdEvaluation);


            evaluationToUpdate.UpdateTime = DateTime.Now;

            if (evaluationToUpdate.Score != evaluation.Score)
            {
                evaluationToUpdate.Score = evaluation.Score;
            };
            if (evaluationToUpdate.Description != evaluation.Description)
            {
                evaluationToUpdate.Description = evaluation.Description;
            };

            ctx.Evaluations.Update(evaluationToUpdate);
            ctx.SaveChanges();
        }
    }
}
