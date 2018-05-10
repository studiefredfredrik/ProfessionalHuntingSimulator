using System;
using System.Collections.Generic;
using System.Linq;
using HighScoreApi.Configuration;
using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;

namespace HighScoreApi.Controllers
{
    [Route("api/[controller]")]
    public class HighScoreController : Controller
    {
        readonly IDocumentStore _store;
        readonly HighScoreSettings _highScoreSettings;

        public HighScoreController(IDocumentStore store, HighScoreSettings highScoreSettings)
        {
            _store = store;
            _highScoreSettings = highScoreSettings;
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
        public IActionResult Post([FromBody]HighScoreDocument score, string password)
        {
            if (password != _highScoreSettings.SuperSecretApplicationKey) return Forbid();

            score.TimeOfEntry = DateTime.Now;
            using (var session = _store.OpenSession())
            {
                session.Store(score);
                session.SaveChanges();
            }
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(string id, string password)
        {
            if (password != _highScoreSettings.SuperSecretApplicationKey) return Forbid();
            
            using (var session = _store.OpenSession())
            {
                session.Delete(id);
                session.SaveChanges();
            }
            return Ok();
        }
    }
}
