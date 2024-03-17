using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.API.Errors;
using Talabat.Core.Repositories;
using Talabat.Repsitory.Data;

namespace Talabat.API.Controllers
{
    
    public class BuggyController : BaseApiController
    {
        private readonly StoreDbContext _dbContext;

        public BuggyController(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        [HttpGet("notfound")]
        public ActionResult GetNotFoundRequest()
        {
            var product = _dbContext.Products.Find(100);
            if(product == null)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(product);
        }
        [HttpGet("servererror")]
        public ActionResult GetServerError()
        {
            var product =_dbContext.Products.Find(100);
            var producttoreturn= product.ToString();
            return Ok(producttoreturn);
        }
        [HttpGet("badrequest")]
        public ActionResult BadRequest()
            {
            return BadRequest();
        }
        [HttpGet("badrequest/{id}")]
        public ActionResult GetBadRequest(int id)
        {
            return Ok();
        }
    }
}
