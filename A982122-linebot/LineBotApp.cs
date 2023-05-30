using Line.Messaging;
using Line.Messaging.Webhooks;

namespace A982122_linebot.Models;
public class LineBotApp : WebhookApplication
{
    private readonly LineMessagingClient _messagingClient;

    private static Dictionary<string, string> _pool = new Dictionary<string, string>();
    public LineBotApp(LineMessagingClient lineMessagingClient)
    {
        _messagingClient = lineMessagingClient;
    }

    protected override async Task OnMessageAsync(MessageEvent ev)
    {
        var result = null as List<ISendMessage>;

        switch (ev.Message)
        {
            //文字訊息
            case TextEventMessage textMessage:
            {
                //頻道Id
                var channelId = ev.Source.Id;
                //使用者Id
                var userId = ev.Source.UserId;
                //使用者輸入的文字
                var  text = ((TextEventMessage)ev.Message).Text;

                if (PoolHasMsg(text))
                {
                    // 從記憶體池查詢資料
                    string response = GetResponse(text);
                    result = new List<ISendMessage>
                    {
                        new TextMessage(response)
                    };
                }
                else
                {
                    if (CheakFormat(text))
                    {
                        //將資料寫入記憶體池
                        TeachDog(text);
                    }
                }
                
                /*
                //回傳 hellow
                result = new List<ISendMessage>
                {
                    new TextMessage("hellow" + text)
                    
                };
                */
            }
                break;
        }

        if (result != null)
            await _messagingClient.ReplyMessageAsync(ev.ReplyToken, result);
    }

    //確認是否已經學過這個名詞
    private bool PoolHasMsg(string inputMsg)
    {
        return _pool.ContainsKey(inputMsg);
        
    }
    
    //
    private string GetResponse(string inputMsg)
    {
        return _pool[inputMsg];
    }
    
    private bool CheakFormat(string inputMsg)
    {
        bool result = false;

        try
        {
            string[] subs = inputMsg.Split(';');

            if (subs.Length == 3)
            {
                if (subs[0]=="拉巴拉修狗")
                {
                    result = true;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return result;
    }
    private void TeachDog(string inputMsg)
    {
        try
        {
            string[] subs = inputMsg.Split(';');

            if (subs.Length == 3)
            {
                if (subs[0]=="拉巴拉修狗")
                {
                    _pool.Add(subs[1],subs[2]);                
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}