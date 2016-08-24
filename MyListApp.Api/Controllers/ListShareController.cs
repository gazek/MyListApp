using MyListApp.Api.Services;
using System.Web.Http;

namespace MyListApp.Api.Controllers
{
    [Authorize]
    [RoutePrefix("api/Share")]
    public class ListShareController : ApiController
    {
        private ListShareRepository _repo { get; set; }
        private ListAuthChecker _auth { get; set; }

        public ListShareController()
        {
            _repo = new ListShareRepository(User.Identity);
            _auth = new ListAuthChecker(User.Identity);
        }

        // DELETE api/<controller>/5
        [Route("{id}")]
        public IHttpActionResult Delete(int id)
        {
            if (!_auth.IsListOwnerByShareId(id))
            {
                return Unauthorized();
            }

            bool result = _repo.Delete(id);

            if (result)
            {
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }
    }
}