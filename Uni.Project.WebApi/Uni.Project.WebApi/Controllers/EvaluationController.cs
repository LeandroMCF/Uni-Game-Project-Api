using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Uni.Project.WebApi.Context;
using Uni.Project.WebApi.Domain;
using Uni.Project.WebApi.Interface;
using Uni.Project.WebApi.Migrations;
using Uni.Project.WebApi.Repository;

namespace Uni.Project.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EvaluationController : ControllerBase
    {
        private IEvaluationInterface _evaluation { get; set; }

        public EvaluationController()
        {
            _evaluation = new EvaluationRepository();
        }

        [HttpPost("evaluating/{score}/{description}/{idUser}")]
        public IActionResult Evaluating(int score, string description, string idUser)
        {
            EvaluationDomain evaluation = new EvaluationDomain(new Guid(idUser), DateTime.Now, DateTime.Now, score, description);

            try
            {
                _evaluation.AddEvaluation(evaluation);
                return Ok("Avaliação cadastrada com sucesso!");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPut("evaluating/{score}/{description}/{idUser}")]
        public IActionResult UpdateEvaluation(string IdUser, int score, string description)
        {
            UniContext ctx = new UniContext();
            EvaluationDomain existEvaluation = ctx.Evaluations.FirstOrDefault(x => x.UserId.ToString() == IdUser);

            EvaluationDomain evaluation = new EvaluationDomain(new Guid(IdUser), DateTime.Now, DateTime.Now, score, description);

            evaluation.EvaluationId = existEvaluation.EvaluationId;

            try
            {
                _evaluation.UpdateEvaluation(existEvaluation.EvaluationId.ToString(), IdUser, evaluation);
                return Ok("Avaliação editada com sucesso!");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpDelete("delete/evaluating/{IdUser}")]
        public IActionResult Evaluating(string IdUser)
        {
            try
            {
                _evaluation.RemoveEvaluation(IdUser);
                return Ok("Avaliação deletada com sucesso!");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("all/{userId}")]
        public string GetAllEvaluation(string userId)
        {
            try
            {
                return _evaluation.GetAllEvaluations(userId);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("{idUser}")]
        public EvaluationDomain GetEvaluation(string idUser)
        {
            try
            {
                return _evaluation.GetEvaluation(idUser);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpGet("verify/{idUser}")]
        public string IsAllow(string IdUser)
        {
            try
            {
                return _evaluation.AlloyAccess(IdUser);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
