using Microsoft.AspNet.WebHooks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Quiz.API.Controllers
{
    public class SubscriptionsController : ApiController
    {
        private IWebHookManager _manager;
        private IWebHookStore _store;
        private IWebHookUser _user;


        // POST /api/subscriptions/
        public async Task Post([FromBody] WebHook webhook)
        {
            string username = getUserName();

            await _store.DeleteAllWebHooksAsync(username);
            var hooks = await _store.QueryWebHooksAsync(username, new string[] { "*" }, (wh, s) => wh.WebHookUri == webhook.WebHookUri);

            // Remove all existing webhooks (demo purposes)
            //if (hooks.Count() != 0)
            //{
            //    hooks.Clear();
            //}

            // Add new webhook
            webhook.Filters.Add("*");
            await _store.InsertWebHookAsync(username, webhook);
        }

        // DELETE /api/subscriptions/
        public async Task Delete(HttpRequestMessage webhookuriMsg)
        {
            string username = getUserName();
            var webhookuri = await webhookuriMsg.Content.ReadAsStringAsync();  // Expect just the webhook uri in the body, no json

            var hooks = await _store.QueryWebHooksAsync(username, new string[] { "*" }, (wh, s) => wh.WebHookUri.ToString() == webhookuri);
            if (hooks.Count() > 0)
            {
                // Delete webhooks associated with the given user and webhook uri
                foreach (var hook in hooks)
                {
                    await _store.DeleteWebHookAsync(username, hook.Id);
                }
            }
        }

        private string getUserName()
        {
            //var username = Request.Headers.GetValues("X-MS-CLIENT-PRINCIPAL-ID").FirstOrDefault(); ;
            //if (string.IsNullOrWhiteSpace(username)) { username = "nousername"; }

            //return username;
            //TODO: fix this so it also properly works with authentication
            return "nousername";
        }

        protected override void Initialize(HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);

            _manager = Configuration.DependencyResolver.GetManager();
            _store = Configuration.DependencyResolver.GetStore();
            _user = Configuration.DependencyResolver.GetUser();

        }


    }
}
