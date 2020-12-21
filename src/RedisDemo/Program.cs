using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedisDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            var redis = new RedisClient("127.0.0.1", 6379);

            // 写入
            redis.SetKeyValue<string>("name", "dwayne");
            redis.SetKeyValue<int>("age", 26);

            // 读取
            Console.WriteLine($"name的数值：{redis.GetKeyValue<string>("name")}");
            Console.WriteLine($"age的数值：{redis.GetKeyValue<string>("age")}");



            // 订阅
            Task.Run(() =>
            {
                redis.Subscribe();
            });

            // 发布
            Console.WriteLine("是否启动Redis发布：Y（启动），N（不启动）");
            if (Console.ReadLine() == "Y")
            {
                redis.Publish();
            }

            Console.ReadKey();
        }
    }
}
