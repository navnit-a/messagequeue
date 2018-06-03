using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using MQMQ;

namespace Listener
{
    class Program
    {
        static void Main(string[] args)
        {
            var queueAddress = @".\private$\durable-queue";

            using (var queue = new MessageQueue(queueAddress))
            {
                while (true)
                {
                    Console.WriteLine("Listening on: {0}", queueAddress);
                    var message = queue.Receive();
                    if (message != null)
                    {
                        var messageBody = message.BodyStream.ReadFromRaw(message.Label);
                        Console.WriteLine(messageBody);
                    }
                    //TODO - would use a factory/IoC/MEF for this:
                    //var messageType = messageBody.GetType();
                    //if (messageType == typeof(UnsubscribeCommand))
                    //{
                    //    Unsubscribe((UnsubscribeCommand)messageBody);
                    //}
                    //else if (messageType == typeof(DoesUserExistRequest))
                    //{
                    //    CheckUserExists((DoesUserExistRequest)messageBody, message);
                    //}
                }
            }
        }
    }
}
