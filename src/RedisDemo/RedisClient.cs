using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisDemo
{
    public class RedisClient
    {
        private ServiceStack.Redis.RedisClient client = null;

        // 发布消息的客户端
        private ServiceStack.Redis.RedisClient client_pub = null;

        public RedisClient(string ip,int port,string password=null,int db=0)
        {
            client     = new ServiceStack.Redis.RedisClient(ip, port, password, db);
            client_pub = new ServiceStack.Redis.RedisClient(ip, port, password, db);
            Console.WriteLine($"redis启动成功！");
        }

        /// <summary>
        /// 读取
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetKeyValue<T>(string key)
        {

            return client.Get<T>(key);
        }

        /// <summary>
        /// 存储
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool SetKeyValue<T>(string key,T value)
        {
           return  client.Set<T>(key,value);
        }


        /// <summary>
        /// 订阅
        /// </summary>
        public void Subscribe()
        {
            //创建订阅
            IRedisSubscription subscription = client.CreateSubscription();

            //接受到消息时
            subscription.OnMessage += (channel,cmd)=>
            {
                Console.WriteLine($"接受到消息时：消息通道：{channel}，消息内容：{cmd}");
            };

            //订阅频道时
            subscription.OnSubscribe = (channel) =>
            {
                Console.WriteLine($"订阅客户端：开始订阅消息通道：{channel}");
            };

            //取消订阅频道时
            subscription.OnUnSubscribe = (channel) => 
            { 
                Console.WriteLine($"订阅客户端：取消订阅消息通道：{channel}");
            };

            //订阅频道
            subscription.SubscribeToChannels("channel1", "channel2");
        }

        public void Publish()
        {
            client_pub.PublishMessage("channel1", $"{DateTime.Now.ToShortTimeString()} 通道 channel1 的消息");
            client_pub.PublishMessage("channel2", $"{DateTime.Now.ToShortTimeString()} 通道 channel1 的消息");
        }
    }
}
