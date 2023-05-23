using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using A982122_linebot.Models;
using Line.Messaging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace A982122_linebot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LineBotController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly HttpContext _httpContext;
        private readonly LineBotConfig _lineBotConfig;
        public LineBotController(IServiceProvider serviceProvider)
        {
            _httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
            _httpContext = _httpContextAccessor.HttpContext;
            _lineBotConfig = new LineBotConfig();
            _lineBotConfig.channelSecret = "8fe8548ba2991f869578a881ce7303dc";
            _lineBotConfig.accessToken = "eALIfZ01nIS5MVM2gHQ62yWcywb2urmFTs3uNIuvc1N+1KKyl9Ts4QOXdUCE9NoLRDwKvd7kCtNvIoNxLwNkK2LvXZbAwWxHJtPTxpLuZiULbS/49bijX88hb8qrhthAFys4dsuj2vqLmxGCoOj2bAdB04t89/1O/w1cDnyilFU=";

        }
        //完整的路由網址就是 https://xxx/api/linebot/run
        [HttpPost("run")]
        public async Task<IActionResult> Post()
        {
            try
            {
                var events = await _httpContext.Request.GetWebhookEventsAsync(_lineBotConfig.channelSecret);
                var lineMessagingClient = new LineMessagingClient(_lineBotConfig.accessToken);
                var lineBotApp = new LineBotApp(lineMessagingClient);
                await lineBotApp.RunAsync(events);
            }
            catch (Exception ex)
            {
                //需要 Log 可自行加入
                //_logger.LogError(JsonConvert.SerializeObject(ex));
            }
            
            return Ok();
        }
    }
}
