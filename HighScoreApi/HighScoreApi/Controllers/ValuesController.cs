using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;

namespace HighScoreApi.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private const string SuperSecretAdminKey = "TheStrongestPasswordEver";
        private const string SuperSecretApplicationKey = "PeopleCantPostWithoutPlaying";
        readonly IDocumentStore _store;

        public ValuesController(IDocumentStore store)
        {
            _store = store;
        }

        [HttpGet]
        public List<HighScoreDocument> Get()
        {
            using (var session = _store.OpenSession())
            {
                return session.Query<HighScoreDocument>()
                    .Take(100)
                    .OrderByDescending(doc => doc.Score)
                    .ToList();
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]HighScoreDocument value, string password)
        {
            if (password != SuperSecretApplicationKey) return Forbid();

            value.TimeOfEntry = DateTime.Now;
            using (var session = _store.OpenSession())
            {
                session.Store(value);
                session.SaveChanges();
            }
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(string id, string password)
        {
            if (password != SuperSecretApplicationKey) return Forbid();
            
            using (var session = _store.OpenSession())
            {
                session.Delete(id);
                session.SaveChanges();
            }
            return Ok();
        }
    }
}
